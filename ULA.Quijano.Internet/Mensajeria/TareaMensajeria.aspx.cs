using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using ULA.Quijano.Internet.AttachmentService;
using ULA.Quijano.Internet.UltimusIntegrationLayer;
using Ultimus.UtilityLayer;

namespace ULA.Quijano.Internet.Mensajeria
{
    public partial class TareaMensajeria : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LblError.Visible = false;
            try
            {
                if(!IsPostBack)
                {
                    LoadParameters();

                    if (ViewState["process"] == null || ViewState["incident"] == null)
                    {
                        ShowFullPageError("El enlace de acceso ha expirado.");
                        return;
                    }
                    Session["process"] = Process();
                    Session["incident"] = Incident();

                    using (UltimusIntegrationClient client = new UltimusIntegrationClient())
                    {
                        UltimusIncident ui;
                        string error;
                        client.GetTaskByFilters(string.Empty, Process(), "ResuelveSolicitud", Incident(), out ui, out error);

                        if(ui == null || ui.Status != 1)
                        {
                            ShowFullPageError("El enlace de acceso ha expirado.");
                            return;
                        }
                    }
                    
                    List<EstadoSolicitud> estados = GetEstadoSolicitud();
                    ViewState["_Estados"] = estados.Select(x => new KeyValuePair<string, int>(x.Codigo, x.CompletarTarea)).ToList();
                    ddlEstado.DataSource = estados;
                    ddlEstado.DataValueField = "Codigo";
                    ddlEstado.DataTextField = "Nombre";
                    ddlEstado.DataBind();
                    ddlEstado.Items.Insert(0, new ListItem());
                    
                    Servicio s = GetServicioByID(Incident());

                    if(s.Solicitud=="1")
                    {
                        txtTitulo.InnerHtml = "Solcitud de Pasante";
                        TxtSolicitadoPor.Value = s.SolicitadoPor;
                        txtEntidad.Value = s.SociedadEntidadEmpresa;
                        txtDireccion.Value = s.Direccion;
                        txtTramite.Value = s.Tramite;
                        txtDetalles.Value = s.Detalle;
                        divPasante.Visible = true;
                    }
                    else if (s.Solicitud == "2")
                    {
                        txtTitulo.InnerHtml = "Solcitud de Mensajero";
                        TxtSolicitadoPor2.Value = s.SolicitadoPor;
                        txtEntidad2.Value = s.SociedadEntidadEmpresa;
                        txtPara.Value = s.NombrePara;
                        txtDe.Value = s.NombreDe;
                        txtDireccion2.Value = s.Direccion;
                        txtPiso.Value = s.Piso;
                        txtCalle.Value = s.Calle;
                        txtReferenciaLugarEnvio.Value = s.Referencias;
                        txtHorarios.Value = s.Horario;
                        txtTelefonos.Value = s.Telefono;
                        txtAsunto.Value = s.Asunto;
                        txtAcciones.Value = s.Accion;
                        divMensajero.Visible = true;
                    }

                    using (AttachmentServiceClient client = new AttachmentServiceClient())
                    {
                        string[] adjuntos = client.GetFileList(Process(), Incident().ToString(), "ResuelveSolicitud");

                        if (adjuntos.Length > 0)
                        {
                            RptAdjuntos.DataSource = adjuntos;
                            RptAdjuntos.DataBind();
                        }
                        else
                        {
                            divAdjuntos.Visible = false;
                        }

                        RptFiles.DataSource = client.GetFileList(Process(), Incident().ToString() + " (Resolucion)", "ResuelveSolicitud");
                        RptFiles.DataBind();
                    }

                    txtEstado.Value = string.IsNullOrEmpty(s.Estado) ? "" :  estados.FirstOrDefault(t => t.Codigo.ToString() == s.Estado).Nombre;
                    txtResponsable.Value = s.NombreResponsable;
                    txtComentario.Value = s.Comentarios;
                }
            }
            catch(Exception ex)
            {
                new LogFilecs().EscribirArchivo("Quijano.Internet", ex);
                ShowFullPageError("Ocurrio un error. Intente nuevamente.");
            }
        }

        public Servicio GetServicioByID(int incidenteID)
        {
            try
            {
                using (WcfMensajeriaClient client = new WcfMensajeriaClient())
                {
                    return client.GetServicioByID(incidenteID);
                }
            }
            catch (Exception ex)
            {
                new LogFilecs().EscribirArchivo("Quijano.Internet", ex);
                return null;
            }
        }

        public bool ActualizarEstadoServicio(string proceso, string etapa, int incidenteID, string estado, string comentariosResolucion, int diasSeguimiento, string fechaSeguimiento)
        {
            bool confirm = false;
            try
            {
                List<KeyValuePair<string, int>> completarTarea = (List<KeyValuePair<string, int>>)ViewState["_Estados"];

                using (WcfMensajeriaClient client = new WcfMensajeriaClient())
                {
                    confirm = client.ActualizarEstadoServicio(incidenteID, estado, comentariosResolucion, fechaSeguimiento);
                }

                if (confirm && completarTarea.FirstOrDefault(t => t.Key == estado).Value > 0)
                {
                    using (UltimusIntegrationClient client = new UltimusIntegrationClient())
                    {
                        string error;
                        int incident;

                        UltimusIncident task = new UltimusIncident();
                        client.GetTaskByFilters(string.Empty, proceso, etapa, incidenteID, out task, out error);

                        List<NodeVariables> nodeVariables = new List<NodeVariables>();
                        nodeVariables.Add(new NodeVariables() {
                            NodeName = "TaskData.Global.Estado",
                            NodeValue = completarTarea.FirstOrDefault(t => t.Key == estado).Value.ToString()
                        });
                        nodeVariables.Add(new NodeVariables()
                        {
                            NodeName = "TaskData.Global.DiasSeguimiento",
                            NodeValue = diasSeguimiento.ToString()
                        });
                        nodeVariables.Add(new NodeVariables()
                        {
                            NodeName = "TaskData.Global.ComentarioCierre",
                            NodeValue = comentariosResolucion
                        });
                        nodeVariables.Add(new NodeVariables()
                        {
                            NodeName = "TaskData.Global.FechaSeguimiento",
                            NodeValue = fechaSeguimiento
                        });

                        client.CompleteTaskWithVariables(task.User, task.TaskId, string.Empty, task.Summary, nodeVariables.ToArray(), out incident, out error);
                        return string.IsNullOrEmpty(error);
                    }
                }

                return confirm;
            }
            catch(Exception ex)
            {
                new LogFilecs().EscribirArchivo("Quijano.Internet", ex);
                return false;
            }
        }

        public List<EstadoSolicitud> GetEstadoSolicitud()
        {
            try
            {
                using (WcfMensajeriaClient client = new WcfMensajeriaClient())
                {
                    return client.GetEstadoSolicitud();
                }
            }
            catch (Exception ex)
            {
                new LogFilecs().EscribirArchivo("Quijano.Internet", ex);
                return null;
            }
        }

        private void ShowErrorMessage(string message)
        {
            LblError.Visible = true;
            LblError.InnerText = message;
        }

        private void ShowFullPageError(string message)
        {
            divSolicitud.Visible = false;
            divCompletarTarea.Visible = false;
            ShowErrorMessage(message);
        }

        private void LoadParameters()
        {
            if (string.IsNullOrEmpty(Request.QueryString.ToString()))
                return;

            string[] parameters = new Crypt().DecryptString(Server.UrlDecode(Request.QueryString.ToString())).Split('&');
            foreach (string s in parameters)
            {
                string key = s.Substring(0, s.IndexOf("="));
                string value = s.Substring(key.Length + 1);
                ViewState[key] = value;
            }
        }

        private string Process()
        {
            return (string)ViewState["process"];
        }

        private int Incident()
        {
            return int.Parse((string)ViewState["incident"]);
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                int diasSeguimiento = 0;
                if (ddlEstado.Value == "4")
                    diasSeguimiento = (int)DateTime.ParseExact(TxtFechaSeguimiento.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).Subtract(DateTime.Now.Date).TotalDays;

                if (ActualizarEstadoServicio(Process(), "ResuelveSolicitud", Incident(), ddlEstado.Value, txtComentariosResolucion.Value, diasSeguimiento, TxtFechaSeguimiento.Value))
                {
                    divEnviar.Visible = false;
                    divEnviado.Visible = true;
                    txtEstado.Value = ddlEstado.Items[ddlEstado.SelectedIndex].Text;
                    Page.ClientScript.RegisterStartupScript(typeof(Page), Guid.NewGuid().ToString(), "setInterval(function () { window.open('','_self').close(); }, 5000);", true);
                }
                else
                {
                    ShowErrorMessage("Ocurrio un error. Intente nuevamente.");
                }
            }
            catch (Exception ex)
            {
                new LogFilecs().EscribirArchivo("Quijano.Internet", ex);
                ShowErrorMessage("Ocurrio un error. Intente nuevamente.");
            }
        }

        private string AttachmentParam()
        {
            Crypt crypt = new Crypt();
            return Server.UrlEncode(crypt.EncryptString("process=" + Process() + "&incident=" + Incident() + "&upload=0&delete=1&title=0&style=1&etapa=ResuelveSolicitud"));
        }

        protected void BtnSubirImagen_Click(object sender, EventArgs e)
        {
            try
            {
                using (AttachmentServiceClient client = new AttachmentServiceClient())
                {
                    RptFiles.DataSource = client.GetFileList(Process(), Incident().ToString() + " (Resolucion)", "ResuelveSolicitud");
                    RptFiles.DataBind();
                }
            }
            catch(Exception ex)
            {
                new LogFilecs().EscribirArchivo("Quijano.Internet", ex);
                ShowErrorMessage("Ocurrio un error. Intente nuevamente.");
            }
        }

    }
}