using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using ULA.Quijano.SendMails.AttachmentService;
using ULA.Quijano.SendMails.UltimusIntegrationLayer;
using Ultimus.DataAccessLayer;
using Ultimus.UtilityLayer;

namespace ULA.Quijano.SendMails
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class WsLaunchIncidentbyemail : System.Web.Services.WebService
    {

        [WebMethod]
        public int LaunchIncidentbyemail(string displayName, string email, DateTime datetime, string subject, string body, string emaiId, string to, string cc, string[] attachments)
        {
            try
            {
                string usuarioIniciador = WebConfigurationManager.AppSettings["UserLaunchIncidentbyemail"];
                int i = CrearIncidenteComunicacion(usuarioIniciador, displayName);
                if (i == 0)
                    return 0;

                if (!IngresarComunicacion(displayName, email, datetime, subject, body, emaiId, to, cc, i, attachments))
                    return 0;

                //if (!GuardarAdjuntos(attachments, i))
                //    return 0;

                return i;
            }
            catch (Exception ex)
            {
                new LogFilecs().EscribirArchivo("WsLaunchIncidentbyemail", ex);
                new LogFilecs().EscribirArchivo("WsLaunchIncidentbyemail", "LaunchIncidentbyemail(\"" + displayName + "\", \"" + email + "\", " + datetime.ToString() + ", \"" + subject + "\", \"" + body + "\", \"" + emaiId + "\", \"" + to + "\", { " + string.Join(", ", attachments.Select(a => "\"" + a + "\"")) + " })");
                return 0;
            }
        }

        private int CrearIncidenteComunicacion(string user, string summary)
        {
            try
            {
                List<NodeVariables> nodeList = new List<NodeVariables>();
                nodeList.Add(new NodeVariables()
                {
                    NodeName = "TaskData.Global.RequiereRevision",
                    NodeValue = "1"
                });

                using (UltimusIntegrationClient client = new UltimusIntegrationClient())
                {
                    int incidente;
                    string error;
                    UltimusIncident ui;
                    client.GetTaskByFilters(user, "Comunicacion", "GestionaCorreo", 0, out ui, out error);

                    if (ui == null)
                    {
                        new LogFilecs().EscribirArchivo("WsLaunchIncidentbyemail", error);
                        return 0;
                    }

                    client.CompleteTaskWithVariables(ui.User, ui.TaskId, string.Empty, summary, nodeList.ToArray(), out incidente, out error);
                    if (!string.IsNullOrEmpty(error))
                    {
                        new LogFilecs().EscribirArchivo("WsLaunchIncidentbyemail", error);
                        return 0;
                    }

                    return incidente;
                }
            }
            catch (Exception ex)
            {
                new LogFilecs().EscribirArchivo("WsLaunchIncidentbyemail", ex);
                new LogFilecs().EscribirArchivo("WsLaunchIncidentbyemail", "CrearIncidenteComunicacion(\"" + user + "\", \"" + summary + "\")");
                return 0;
            }
        }

        private bool IngresarComunicacion(string displayName, string email, DateTime datetime, string subject, string body, string emaiId, string to, string cc, int incidente, string[] attachments)
        {
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_COMUNICACION.USP_INS_COMUNICACION";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Int32, ParameterDirection.Input, 255, incidente.ToString());
                Procedimiento.AddParametros("P_IDCLIENTE", OracleDbType.Int32, ParameterDirection.Input, 255, "0");
                Procedimiento.AddParametros("P_NOMBRE", OracleDbType.Varchar2, ParameterDirection.Input, 100, string.IsNullOrEmpty(displayName) ? "" : displayName);
                Procedimiento.AddParametros("P_INCIDENTE_RELACIONADO", OracleDbType.Varchar2, ParameterDirection.Input, 100, string.Empty);
                Procedimiento.AddParametros("P_CODIDIOMA", OracleDbType.Int32, ParameterDirection.Input, 255, string.Empty);
                Procedimiento.AddParametros("P_CODPROCEDENCIA", OracleDbType.Int32, ParameterDirection.Input, 255, string.Empty);
                Procedimiento.AddParametros("P_CODAREA", OracleDbType.Int32, ParameterDirection.Input, 255, string.Empty);
                Procedimiento.AddParametros("P_CODJURISDICCION", OracleDbType.Int32, ParameterDirection.Input, 255, string.Empty);
                Procedimiento.AddParametros("P_CODTRAMITE", OracleDbType.Int32, ParameterDirection.Input, 255, string.Empty);
                Procedimiento.AddParametros("P_ABOGADO_ASIGNADO", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.Empty);
                Procedimiento.AddParametros("P_CORREO_ORIGEN", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.IsNullOrEmpty(email) ? "" : email);
                Procedimiento.AddParametros("P_CORREO_PARA", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.IsNullOrEmpty(to) ? "" : to);
                Procedimiento.AddParametros("P_ASUNTO", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.IsNullOrEmpty(subject) ? "" : subject);
                Procedimiento.AddParametros("P_CONTENIDO", OracleDbType.Clob, ParameterDirection.Input, 1073741824, string.IsNullOrEmpty(body) ? "" : body);
                Procedimiento.AddParametros("P_ASUNTO_ORIGEN", OracleDbType.Varchar2, ParameterDirection.Input, 5, "0-0-0");
                Procedimiento.AddParametros("P_CORREO_INFRACTOR", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.Empty);
                Procedimiento.AddParametros("P_RESPUESTA", OracleDbType.Clob, ParameterDirection.Input, 1073741824, string.Empty);
                Procedimiento.AddParametros("P_ABOGADO_RESPONDE", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.Empty);
                Procedimiento.AddParametros("P_GENERA_INSTRUCCION", OracleDbType.Int32, ParameterDirection.Input, 255, string.Empty);
                Procedimiento.AddParametros("P_ETAPA", OracleDbType.Varchar2, ParameterDirection.Input, 255, "LaunchIncidentbyemail");
                Procedimiento.AddParametros("P_LISTA_ADJUNTOS", OracleDbType.Clob, ParameterDirection.Input, 1073741824, attachments.Length == 0 ? "" : string.Join("|", attachments));
                Procedimiento.AddParametros("P_CORREO_CC", OracleDbType.Varchar2, ParameterDirection.Input, 1000, string.IsNullOrEmpty(cc) ? "" : cc);

                Procedimientos.Add(Procedimiento);
                return DA.EjecutarProcedimientos(Procedimientos);
            }
            catch (Exception ex)
            {
                new LogFilecs().EscribirArchivo("WsLaunchIncidentbyemail", ex);
                new LogFilecs().EscribirArchivo("WsLaunchIncidentbyemail", "IngresarComunicacion(\"" + displayName + "\", \"" + email + "\", " + datetime.ToString() + ", \"" + subject + "\", \"" + body + "\", \"" + emaiId + "\", \"" + to + "\", " + incidente + ")");
                return false;
            }
        }

        private bool GuardarAdjuntos(string[] adjuntos, int incidente)
        {
            try
            {
                foreach (string url in adjuntos)
                {
                    byte[] bytes;

                    WebRequest webRequest = WebRequest.Create(url);

                    using (StreamReader reader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                    {
                        using (MemoryStream memstream = new MemoryStream())
                        {
                            reader.BaseStream.CopyTo(memstream);
                            bytes = memstream.ToArray();
                        }
                    }

                    using (AttachmentServiceClient client = new AttachmentServiceClient())
                    {
                        if (!client.UploadFile("Comunicacion", incidente.ToString(), "GestionaCorreo", Path.GetFileName(url), bytes))
                            return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                new LogFilecs().EscribirArchivo("WsLaunchIncidentbyemail", ex);
                new LogFilecs().EscribirArchivo("WsLaunchIncidentbyemail", "GuardarAdjuntos({ " + string.Join(", ", adjuntos.Select(a => "\"" + a + "\"")) + " }, " + incidente + ")");
                return false;
            }
        }

    }
}
