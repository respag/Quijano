using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using ULA.Quijano.Model;
using ULA.Quijano.SendMails.AttachmentService;
using ULA.Quijano.SendMails.UltimusIntegrationLayer;
using Ultimus.DataAccessLayer;
using Ultimus.UtilityLayer;

namespace ULA.Quijano.SendMails
{
    /// <summary>
    /// Summary description for WsComunicacion
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    [ScriptService]
    public class WsComunicacion : System.Web.Services.WebService
    {
        static char[] delimitadores = { ',', ';', ' ' };

        [WebMethod]
        public bool EnviarComunicacion(string proceso, int incidente)
        {
            try
            {
                List<Comunicacion> temp = GetComunicacionByID(incidente);

                if (temp.Count == 0 || string.IsNullOrEmpty(temp[0].Correo_Origen))
                    return false;

                Comunicacion c = temp[0];
                if (c.CorreoCC == null)
                    c.CorreoCC = "";

                return SendMail(c.Asunto, c.Respuesta, true, c.Correo_Origen.Split(delimitadores, StringSplitOptions.RemoveEmptyEntries), c.CorreoCC.Split(delimitadores, StringSplitOptions.RemoveEmptyEntries), proceso, incidente.ToString(), false);
            }
            catch (Exception ex)
            {
                new LogFilecs().EscribirArchivo("SendMails.WsComunicacion", ex);
                return false;
            }
        }

        [WebMethod]
        public bool EnviarNotificacionServiciosLegales(string proceso, int incidente)
        {
            try
            {
                List<Comunicacion> temp = GetComunicacionByIncidenteRelacionado(incidente);
                if (temp.Count == 0 || string.IsNullOrEmpty(temp[temp.Count - 1].Correo_Para))
                    return false;

                Comunicacion c = temp[temp.Count - 1];
                if (c.CorreoCC == null)
                    c.CorreoCC = "";

                return SendMail(c.Asunto, c.Contenido, true, c.Correo_Para.Split(delimitadores, StringSplitOptions.RemoveEmptyEntries), c.CorreoCC.Split(delimitadores, StringSplitOptions.RemoveEmptyEntries), proceso, incidente.ToString(), false);
            }
            catch (Exception ex)
            {
                new LogFilecs().EscribirArchivo("SendMails.WsComunicacion", ex);
                return false;
            }
        }

        [WebMethod]
        public bool EnviarEnlaceServicio(string proceso, int incidente)
        {
            try
            {
                List<Servicio> temp = GetServicioByID(incidente);

                if (temp.Count == 0 || string.IsNullOrEmpty(temp[0].CorreoResponsable))
                    return false;

                string asunto = WebConfigurationManager.AppSettings["AsuntoMensaje"].Replace("%I", incidente.ToString());
                //string cuerpo = GenerarEnlaceMensajeria(proceso, incidente.ToString());
                string urlNotificacion = Context.Request.Url.ToString().Substring(0, Context.Request.Url.ToString().LastIndexOf("WsComunicacion.asmx")) + "Notificacion.aspx?process=" + proceso + "&incident=" + incidente;
                string cuerpo = new StreamReader(WebRequest.Create(urlNotificacion).GetResponse().GetResponseStream()).ReadToEnd();

                return SendMail(asunto, cuerpo, true, new string[] { temp[0].CorreoResponsable }, null, proceso, incidente.ToString(), true);
            }
            catch (Exception ex)
            {
                new LogFilecs().EscribirArchivo("SendMails.WsComunicacion", ex);
                return false;
            }
        }

        [WebMethod]
        public bool GenerarServicioLegal(string proceso, int incidente, string usuario)
        {
            try
            {
                List<Comunicacion> temp = GetComunicacionByID(incidente);

                if (temp.Count == 0)
                    return false;

                if (string.IsNullOrEmpty(temp[0].Incidente_Relacionado) == false && temp[0].Incidente_Relacionado != "0")
                    return true;

                using (UltimusIntegrationClient client = new UltimusIntegrationClient())
                {
                    int incidenteRelacionado;
                    string error;
                    UltimusIncident ui;
                    client.GetTaskByFilters(usuario, "ServiciosLegales", "RegistrarSolicitud", 0, out ui, out error);

                    if (ui == null)
                        return false;

                    client.CompleteTask(ui.User, ui.TaskId, string.Empty, temp[0].Nombre, string.Empty, out incidenteRelacionado, out error);
                    if (!string.IsNullOrEmpty(error) || incidenteRelacionado == 0)
                        return false;

                    return UpdateComunicacionIncidenteRelacionado(incidente, incidenteRelacionado);
                }
            }
            catch (Exception ex)
            {
                new LogFilecs().EscribirArchivo("SendMails.WsComunicacion", ex);
                return false;
            }
        }

        public List<Comunicacion> GetComunicacionByID(int incidenteID) //busca un correo por Id de incidente
        {
            List<Comunicacion> lista = new List<Comunicacion>();
            DataTable dt = new DataTable();

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_COMUNICACION.USP_SEL_COMUNICACION_BY_ID";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Int32, ParameterDirection.Input, 255, incidenteID.ToString());
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                foreach (DataRow rowProducto in dt.Rows)
                {
                    Comunicacion com = new Comunicacion();
                    com.Incidente = Convert.ToInt32(rowProducto["INCIDENTE"]);
                    com.IdCliente = Convert.ToInt32(rowProducto["IDCLIENTE"]);
                    com.Nombre = rowProducto["NOMBRE"].ToString();
                    com.Incidente_Relacionado = Convert.IsDBNull(rowProducto["INCIDENTE_RELACIONADO"]) ? "" : rowProducto["INCIDENTE_RELACIONADO"].ToString();
                    com.Fecha = Convert.IsDBNull(rowProducto["FECHA"]) ? "" : Convert.ToDateTime(rowProducto["FECHA"]).ToString("dd-MMMM-yyyy hh:mm tt");
                    com.CodIdioma = Convert.ToInt32(rowProducto["CODIDIOMA"]);
                    com.CodProcedencia = Convert.ToInt32(rowProducto["CODPROCEDENCIA"]);
                    com.CodArea = Convert.ToInt32(rowProducto["CODAREA"]);
                    com.CodJurisdiccion = Convert.ToInt32(rowProducto["CODJURISDICCION"]);
                    com.CodTramite = Convert.ToInt32(rowProducto["CODTRAMITE"]);
                    com.Abogado_Asignado = rowProducto["ABOGADO_ASIGNADO"].ToString();
                    com.Correo_Origen = rowProducto["CORREO_ORIGEN"].ToString();
                    com.Correo_Para = rowProducto["CORREO_PARA"].ToString();
                    com.Asunto = rowProducto["ASUNTO"].ToString();

                    string[] asuntoOrigen = Convert.IsDBNull(rowProducto["ASUNTO_ORIGEN"]) || string.IsNullOrEmpty(rowProducto["ASUNTO_ORIGEN"].ToString()) ? new string[0] : rowProducto["ASUNTO_ORIGEN"].ToString().Split('-');
                    com.Asunto_Origen_Queja = asuntoOrigen.Length < 1 ? 0 : Convert.ToInt32(asuntoOrigen[0]);
                    com.Asunto_Origen_Devuelta = asuntoOrigen.Length < 2 ? 0 : Convert.ToInt32(asuntoOrigen[1]);
                    com.Asunto_Origen_CPersonal = asuntoOrigen.Length < 3 ? 0 : Convert.ToInt32(asuntoOrigen[2]);

                    com.Contenido = rowProducto["CONTENIDO"].ToString();
                    com.Fecha_Resp = Convert.IsDBNull(rowProducto["FECHA_RESP"]) ? "" : Convert.ToDateTime(rowProducto["FECHA_RESP"]).ToString("dd-MMMM-yyyy hh:mm tt");
                    com.Correo_Infractor = rowProducto["CORREO_INFRACTOR"].ToString();
                    com.Respuesta = rowProducto["RESPUESTA"].ToString();
                    com.Abogado_Responde = rowProducto["ABOGADO_RESPONDE"].ToString();
                    com.Genera_Instruccion = Convert.IsDBNull(rowProducto["GENERA_INSTRUCCION"]) ? 0 : Convert.ToInt32(rowProducto["GENERA_INSTRUCCION"]);
                    com.CorreoCC = rowProducto["CORREO_CC"].ToString();

                    lista.Add(com);
                }
            }
            catch (Exception ex)
            {
                new LogFilecs().EscribirArchivo("SendMails.WsComunicacion", ex);
            }

            return lista;
        }

        public List<Comunicacion> GetComunicacionByIncidenteRelacionado(int incidenteRelacionado) //busca un correo por Id de incidente
        {
            List<Comunicacion> lista = new List<Comunicacion>();
            DataTable dt = new DataTable();

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_COMUNICACION.USP_SEL_COMUNICACION_BY_INC_R";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE_RELACIONADO", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidenteRelacionado.ToString());
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                foreach (DataRow rowProducto in dt.Rows)
                {
                    Comunicacion com = new Comunicacion();
                    com.Incidente = Convert.IsDBNull(rowProducto["INCIDENTE"]) ? 0 : Convert.ToInt32(rowProducto["INCIDENTE"]);
                    com.IdCliente = Convert.IsDBNull(rowProducto["IDCLIENTE"]) ? 0 : Convert.ToInt32(rowProducto["IDCLIENTE"]);
                    com.Nombre = rowProducto["NOMBRE"].ToString();
                    com.Incidente_Relacionado = Convert.IsDBNull(rowProducto["INCIDENTE_RELACIONADO"]) ? "" : rowProducto["INCIDENTE_RELACIONADO"].ToString();
                    com.Fecha = Convert.IsDBNull(rowProducto["FECHA"]) ? "" : Convert.ToDateTime(rowProducto["FECHA"]).ToString("dd-MMMM-yyyy hh:mm tt");
                    com.CodIdioma = Convert.IsDBNull(rowProducto["CODIDIOMA"]) ? 0 : Convert.ToInt32(rowProducto["CODIDIOMA"]);
                    com.CodProcedencia = Convert.IsDBNull(rowProducto["CODPROCEDENCIA"]) ? 0 : Convert.ToInt32(rowProducto["CODPROCEDENCIA"]);
                    com.CodArea = Convert.IsDBNull(rowProducto["CODAREA"]) ? 0 : Convert.ToInt32(rowProducto["CODAREA"]);
                    com.CodJurisdiccion = Convert.IsDBNull(rowProducto["CODJURISDICCION"]) ? 0 : Convert.ToInt32(rowProducto["CODJURISDICCION"]);
                    com.CodTramite = Convert.IsDBNull(rowProducto["CODTRAMITE"]) ? 0 : Convert.ToInt32(rowProducto["CODTRAMITE"]);
                    com.CodFormasMig = Convert.IsDBNull(rowProducto["COD_FORMAS_MIG"]) ? 0 : Convert.ToInt32(rowProducto["COD_FORMAS_MIG"]);
                    com.Abogado_Asignado = rowProducto["ABOGADO_ASIGNADO"].ToString();
                    com.Correo_Origen = rowProducto["CORREO_ORIGEN"].ToString();
                    com.Correo_Para = rowProducto["CORREO_PARA"].ToString();
                    com.Asunto = rowProducto["ASUNTO"].ToString();

                    string[] asuntoOrigen = Convert.IsDBNull(rowProducto["ASUNTO_ORIGEN"]) || string.IsNullOrEmpty(rowProducto["ASUNTO_ORIGEN"].ToString()) ? new string[0] : rowProducto["ASUNTO_ORIGEN"].ToString().Split('-');
                    com.Asunto_Origen_Queja = asuntoOrigen.Length < 1 ? 0 : Convert.ToInt32(asuntoOrigen[0]);
                    com.Asunto_Origen_Devuelta = asuntoOrigen.Length < 2 ? 0 : Convert.ToInt32(asuntoOrigen[1]);
                    com.Asunto_Origen_CPersonal = asuntoOrigen.Length < 3 ? 0 : Convert.ToInt32(asuntoOrigen[2]);

                    com.Contenido = rowProducto["CONTENIDO"].ToString();
                    com.Fecha_Resp = Convert.IsDBNull(rowProducto["FECHA_RESP"]) ? "" : Convert.ToDateTime(rowProducto["FECHA_RESP"]).ToString("dd-MMMM-yyyy hh:mm tt");
                    com.Correo_Infractor = rowProducto["CORREO_INFRACTOR"].ToString();
                    com.Respuesta = rowProducto["RESPUESTA"].ToString();
                    com.Abogado_Responde = rowProducto["ABOGADO_RESPONDE"].ToString();
                    com.Genera_Instruccion = Convert.IsDBNull(rowProducto["GENERA_INSTRUCCION"]) ? 0 : Convert.ToInt32(rowProducto["GENERA_INSTRUCCION"]);
                    com.Etapa = Convert.IsDBNull(rowProducto["ETAPA"]) ? "" : rowProducto["ETAPA"].ToString();
                    lista.Add(com);
                }
            }
            catch (Exception ex)
            {
                new LogFilecs().EscribirArchivo("SendMails.WsComunicacion", ex);
            }

            return lista;
        }

        public List<Servicio> GetServicioByID(int incidenteID) //busca una solicitud de servicio por Id de incidente
        {
            List<Servicio> lista = new List<Servicio>();
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
                foreach (DataRow rowProducto in dt.Rows)
                {
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
                    lista.Add(s);
                }
            }
            catch (Exception ex)
            {
                new LogFilecs().EscribirArchivo("SendMails.WsComunicacion", ex); ;
            }

            return lista;
        }

        public bool UpdateComunicacionIncidenteRelacionado(int incidenteID, int incidenteRelacionado) //actuaizar el campo INCIDENTE_RELACIONADO en la tabla comunicacion
        {
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_COMUNICACION.USP_UPD_COMUNICACION_INCIDENTE";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Int32, ParameterDirection.Input, 255, incidenteID.ToString());
                Procedimiento.AddParametros("P_INCIDENTE_RELACIONADO", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidenteRelacionado.ToString());

                Procedimientos.Add(Procedimiento);
                return DA.EjecutarProcedimientos(Procedimientos);
            }
            catch (Exception ex)
            {
                new LogFilecs().EscribirArchivo("SendMails.WsComunicacion", ex);
                return false;
            }
        }

        public string GenerarEnlaceMensajeria(string process, string incident)
        {
            string enlace = WebConfigurationManager.AppSettings["UrlMensajeriaInternet"] + "?" + Server.UrlEncode(new Crypt().EncryptString("x=" + Guid.NewGuid() + "&process=" + process + "&y=" + Guid.NewGuid() + "&incident=" + incident));
            return WebConfigurationManager.AppSettings["TextoMensaje"].Replace("%I", incident.ToString()).Replace("%E", enlace);
        }

         [WebMethod]
        public bool SendMail(string subject, string body, bool isbodyhtml, string[] mailto, string[] cc, string process, string incident, bool enlaceServicio)
        {
            string imgPattern = "<img src=\"data:image/png;base64,(?<base64>[^'\"]*)\">";
            MailMessage ObjMail = new MailMessage();
            SmtpClient ObjSmpt = new SmtpClient();
            NetworkCredential ObjCredential = new NetworkCredential();
            ObtieneParametros2 Param = new ObtieneParametros2();
            try
            {
                bool modoPruebas = enlaceServicio ? (WebConfigurationManager.AppSettings["ModoPruebasEnviarEnlaceServicio"] == "1") : (WebConfigurationManager.AppSettings["ModoPruebasEnviarComunicacion"] == "1");
                string correoPruebas = WebConfigurationManager.AppSettings["CorreoPruebas"];

                ObjMail.From = new MailAddress(Param.GetValueKeyRegedit("SmtpFromEmail", false));
                ObjMail.Subject = System.Uri.UnescapeDataString(subject);
                ObjMail.Body = System.Uri.UnescapeDataString(body);
                ObjMail.IsBodyHtml = isbodyhtml;
                ObjMail.Priority = MailPriority.High;
                ObjMail.Bcc.Add(new MailAddress(WebConfigurationManager.AppSettings["BCCMailAddress"]));

                if (Regex.IsMatch(ObjMail.Body, imgPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline))
                {
                    string texto = ObjMail.Body;
                    Match match = Regex.Match(ObjMail.Body, imgPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    string img = match.Value;
                    string base64 = match.Groups["base64"].Value;

                    Stream attachmentStream = new MemoryStream(Convert.FromBase64String(base64));
                    Attachment attachment = new Attachment(attachmentStream, new ContentType("image/png")) {
                        ContentId = Guid.NewGuid().ToString(),
                        TransferEncoding = TransferEncoding.Base64
                    };

                    ObjMail.Attachments.Add(attachment);
                    texto = texto.Replace(img, "<img src=\"cid:" + attachment.ContentId + "\" >");
                    ObjMail.Body = texto;
                }

                if (modoPruebas)
                {
                    ObjMail.To.Add(correoPruebas);
                }
                else
                {
                    foreach (string mail in mailto)
                        ObjMail.To.Add(mail);

                    if (cc != null)
                    {
                        foreach (string mail in cc)
                            ObjMail.CC.Add(mail);
                    }
                }

                if (string.IsNullOrEmpty(process) == false && string.IsNullOrEmpty(incident) == false)
                {
                    using (AttachmentServiceClient client = new AttachmentServiceClient())
                    {
                        foreach (string fileName in client.GetFileList(process, incident.ToString(), ""))
                        {
                            Stream attachment = new MemoryStream(client.GetFile(process, incident, "", fileName));
                            ObjMail.Attachments.Add(new Attachment(attachment, fileName));
                        }
                    }
                }

                if (bool.Parse(Param.GetValueKeyRegedit("SmtpUseSsl", false)))
                    ObjSmpt.EnableSsl = true;

                if (bool.Parse(Param.GetValueKeyRegedit("SmtpUseCredentials", false)))
                {
                    ObjCredential.Domain = Param.GetValueKeyRegedit("SmtpDominio", false);
                    ObjCredential.UserName = Param.GetValueKeyRegedit("SmtpUserName", false);
                    ObjCredential.Password = Param.GetValueKeyRegedit("SmtpPassword", false);
                }

                ObjSmpt.Credentials = ObjCredential;
                ObjSmpt.Host = Param.GetValueKeyRegedit("SmtpServer", false);
                ObjSmpt.Port = int.Parse(Param.GetValueKeyRegedit("SmtpPort", false));
                ObjSmpt.Timeout = int.Parse(Param.GetValueKeyRegedit("SmtpTimeOut", false));
                ObjSmpt.Send(ObjMail);

                return true;
            }
            catch (Exception ex)
            {
                new LogFilecs().EscribirArchivo("SendMails.WsComunicacion", ex);
                return false;
            }
            finally
            {
                ObjCredential.Password = null;
                ObjMail.Dispose();
                ObjSmpt = null;
            }
        }
    }
}
