using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using ULA.Quijano.SendMails.UltimusIntegrationLayer;
using Ultimus.DataAccessLayer;
using Ultimus.UtilityLayer;

namespace ULA.Quijano.SendMails
{
    public partial class Notificacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadParameters();

                    if (ViewState["process"] == null || ViewState["incident"] == null)
                        return;

                    using (UltimusIntegrationClient client = new UltimusIntegrationClient())
                    {
                        UltimusIncident ui;
                        string error;
                        client.GetTaskByFilters(string.Empty, Process(), "ResuelveSolicitud", Incident(), out ui, out error);

                        if (ui == null || ui.Status != 1)
                        {
                            return;
                        }
                    }

                    List<EstadoSolicitud> estados = GetEstadoSolicitud();
                    Servicio s = GetServicioByID(Incident());

                    txtIncidente.Text = Incident().ToString();
                    txtEnlace.NavigateUrl = GenerarEnlaceMensajeria(Process(), Incident().ToString());

                    if (s.Solicitud == "1")
                    {
                        txtTitulo.Text = "Solcitud de Pasante";
                        TxtSolicitadoPor.Text = s.SolicitadoPor;
                        txtEntidad.Text = s.SociedadEntidadEmpresa;
                        txtDireccion.Text = s.Direccion;
                        txtTramite.Text = s.Tramite;
                        txtDetalles.Text = s.Detalle;
                        divPasante.Visible = true;
                    }
                    else if (s.Solicitud == "2")
                    {
                        txtTitulo.Text = "Solcitud de Mensajero";
                        TxtSolicitadoPor2.Text = s.SolicitadoPor;
                        txtEntidad2.Text = s.SociedadEntidadEmpresa;
                        txtPara.Text = s.NombrePara;
                        txtDe.Text = s.NombreDe;
                        txtDireccion2.Text = s.Direccion;
                        txtPiso.Text = s.Piso;
                        txtCalle.Text = s.Calle;
                        txtReferenciaLugarEnvio.Text = s.Referencias;
                        txtHorarios.Text = s.Horario;
                        txtTelefonos.Text = s.Telefono;
                        txtAsunto.Text = s.Asunto;
                        txtAcciones.Text = s.Accion;
                        divMensajero.Visible = true;
                    }

                    txtEstado.Text = string.IsNullOrEmpty(s.Estado) ? "" : estados.FirstOrDefault(t => t.Codigo.ToString() == s.Estado).Nombre;
                    txtResponsable.Text = s.NombreResponsable;
                    txtComentario.Text = s.Comentarios;
                }
            }
            catch (Exception ex)
            {
                new LogFilecs().EscribirArchivo("Quijano.Internet", ex);
            }
        }

        private void LoadParameters()
        {
            if (string.IsNullOrEmpty(Request.QueryString.ToString()))
                return;

            string[] parameters = Server.UrlDecode(Request.QueryString.ToString()).Split('&');
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

        public Servicio GetServicioByID(int incidenteID)
        {
            DataTable dt = new DataTable();

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_SERVICIO.USP_SEL_SERVICIO_BY_ID";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Int32, ParameterDirection.Input, 255, incidenteID.ToString());
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                if (dt.Rows.Count == 0)
                    return null;

                DataRow rowProducto = dt.Rows[0];
                Servicio s = new Servicio();
                s.Incidente = Convert.ToInt32(rowProducto["INCIDENTE"]);
                s.Solicitud = rowProducto["SOLICITUD"].ToString();
                s.IdEntidad = Convert.IsDBNull(rowProducto["ID_ENTIDAD"]) ? 0 : Convert.ToInt32(rowProducto["ID_ENTIDAD"]);
                s.SociedadEntidadEmpresa = rowProducto["SOCIEDAD_ENTIDAD_EMPRESA"].ToString();
                s.NombrePara = rowProducto["NOMBRE_PARA"].ToString();
                s.NombreDe = rowProducto["NOMBRE_DE"].ToString();
                s.Tramite = rowProducto["TRAMITE"].ToString();
                s.Detalle = rowProducto["DETALLE"].ToString();
                s.Direccion = rowProducto["DIRECCION"].ToString();
                s.Piso = rowProducto["PISO"].ToString();
                s.Calle = rowProducto["CALLE"].ToString();
                s.Referencias = rowProducto["REFERENCIAS"].ToString();
                s.Horario = rowProducto["HORARIO"].ToString();
                s.Telefono = rowProducto["TELEFONO"].ToString();
                s.Asunto = rowProducto["ASUNTO"].ToString();
                s.IdAccion = Convert.IsDBNull(rowProducto["ID_ACCION"]) ? 0 : Convert.ToInt32(rowProducto["ID_ACCION"]);
                s.Accion = rowProducto["DESCRIPCION"].ToString();
                s.Responsable = Convert.IsDBNull(rowProducto["ID_RESPONSABLE"]) ? 0 : Convert.ToInt32(rowProducto["ID_RESPONSABLE"]);
                s.NombreResponsable = rowProducto["NOMBRE_COMPLETO"].ToString();
                s.CorreoResponsable = rowProducto["CORREO_ELECTRONICO"].ToString();
                s.Estado = rowProducto["ESTADO"].ToString();
                s.Comentarios = rowProducto["COMENTARIOS"].ToString();
                s.ComentariosResolucion = rowProducto["COMENTARIOS_RESOLUCION"].ToString();
                s.NombreEstado = rowProducto["NOMBRE_ESTADO"].ToString();
                s.FechaSeguimiento = rowProducto["FECHA_SEGUIMIENTO"].ToString();
                s.SolicitadoPor = rowProducto["SOLICITADO_POR"].ToString();
                return s;
            }
            catch (Exception ex)
            {
                new LogFilecs().EscribirArchivo("SendMails.WcfMensajeria", ex);
                return null;
            }
        }

        public List<EstadoSolicitud> GetEstadoSolicitud()
        {
            List<EstadoSolicitud> lista = new List<EstadoSolicitud>();
            DataTable dt = new DataTable();

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_SERVICIO.USP_SEL_ESTADO_SOLICITUD";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                foreach (DataRow rowProducto in dt.Rows)
                {
                    EstadoSolicitud p = new EstadoSolicitud();
                    p.Codigo = rowProducto["CODIGO"].ToString();
                    p.Nombre = rowProducto["NOMBRE_ESTADO"].ToString();
                    p.CompletarTarea = Convert.IsDBNull(rowProducto["COMPLETAR_TAREA"]) ? 0 : int.Parse(rowProducto["COMPLETAR_TAREA"].ToString());
                    lista.Add(p);
                }
            }
            catch (Exception ex)
            {
                new LogFilecs().EscribirArchivo("SendMails.WcfMensajeria", ex);
                return null;
            }

            return lista;
        }

        public string GenerarEnlaceMensajeria(string process, string incident)
        {
            return WebConfigurationManager.AppSettings["UrlMensajeriaInternet"] + "?" + Server.UrlEncode(new Crypt().EncryptString("x=" + Guid.NewGuid() + "&process=" + process + "&y=" + Guid.NewGuid() + "&incident=" + incident));
        }

    }
}