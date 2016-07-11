using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using ULA.Quijano.Model;
using ULA.Quijano.ProcesoLegal.AttachmentService;
using ULA.Quijano.ProcesoLegal.Commons;
using ULA.Quijano.ProcesoLegal.UltimusIntegrationLayer;
using Ultimus.DataAccessLayer;

namespace ULA.Quijano.ProcesoLegal.ControllersWebApi
{
    public class ProcesosLegalesApiController : ApiController
    {

        #region LOGGER DEFINITION

        UltimusLogs UltimusLogs = new UltimusLogs("SolicitudAPIController");

        #endregion

        [System.Web.Http.AcceptVerbs("GET")]
        [HttpGet]
        public int CompletarTarea(string proceso, string tempIncident, string userID, string taskID)
        {
            List<NodeVariables> nodeList = null;
            int incidente = UltimusManager.CompleteTask(userID, taskID, String.Empty, String.Empty, nodeList);
            return incidente;
        }

        [System.Web.Http.AcceptVerbs("GET")]
        [HttpGet]
        public int CompletarTarea(string proceso, string tempIncident, string userID, string taskID, string codAreaLegal, string notificaCliente, string step)
        {
            try
            {
                List<NodeVariables> nodeList = null;

                if (!string.IsNullOrEmpty(codAreaLegal))
                {
                    nodeList = new List<NodeVariables>();
                    NodeVariables node = new NodeVariables();
                    node.NodeName = "TaskData.Global.CodAreaLegal";
                    node.NodeValue = codAreaLegal;
                    nodeList.Add(node);
                }

                if (!string.IsNullOrEmpty(notificaCliente))
                {
                    if (nodeList == null)
                        nodeList = new List<NodeVariables>();

                    NodeVariables node = new NodeVariables();
                    node.NodeName = "TaskData.Global.NotificaCliente";
                    node.NodeValue = notificaCliente;
                    nodeList.Add(node);
                }

                if (proceso == "ServiciosLegales" && step == "InstruccionAbogadoGestor")
                {
                    if (nodeList == null)
                        nodeList = new List<NodeVariables>();

                    nodeList.Add(new NodeVariables()
                    {
                        NodeName = "TaskData.Global.UserInstrAbogadoGestor",
                        NodeValue = userID.Substring(userID.IndexOf("/") + 1)
                    });
                }

                int incidente = UltimusManager.CompleteTask(userID, taskID, String.Empty, String.Empty, nodeList);
                return incidente;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return 0;
            }
        }

        [System.Web.Http.AcceptVerbs("GET")]
        [HttpGet]
        public int CompletarTarea(string proceso, string tempIncident, string userID, string taskID, string codAreaLegal, string notificaCliente, string step, string codJurisdiccion, string codProcedencia)
        {
            try
            {
                List<NodeVariables> nodeList = new List<NodeVariables>();

                if (!string.IsNullOrEmpty(codAreaLegal))
                {
                    NodeVariables node = new NodeVariables();
                    node.NodeName = "TaskData.Global.CodAreaLegal";
                    node.NodeValue = codAreaLegal;
                    nodeList.Add(node);
                }

                if (!string.IsNullOrEmpty(notificaCliente))
                {
                    NodeVariables node = new NodeVariables();
                    node.NodeName = "TaskData.Global.NotificaCliente";
                    node.NodeValue = notificaCliente;
                    nodeList.Add(node);
                }

                if (proceso == "ServiciosLegales" && step == "InstruccionAbogadoGestor")
                {
                    nodeList.Add(new NodeVariables()
                    {
                        NodeName = "TaskData.Global.UserInstrAbogadoGestor",
                        NodeValue = userID.Substring(userID.IndexOf("/") + 1)
                    });
                }

                if (proceso == "ServiciosLegales" && step == "AnalisisLegal")
                {
                    nodeList.Add(new NodeVariables()
                    {
                        NodeName = "TaskData.Global.CodJurisdiccion",
                        NodeValue = (codJurisdiccion == "1" || codJurisdiccion == "6" || codJurisdiccion == "7") ? "1" : "2"
                    });

                    nodeList.Add(new NodeVariables()
                    {
                        NodeName = "TaskData.Global.CodProcedencia",
                        NodeValue = codProcedencia
                    });
                }


                int incidente = UltimusManager.CompleteTask(userID, taskID, String.Empty, String.Empty, nodeList);
                return incidente;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return 0;
            }
        }

        [System.Web.Http.AcceptVerbs("GET")]
        [HttpGet]
        public int CompletarTarea2(string process, string step, string incident, string userID, string taskID, string codJurisdiccion, string codProcedencia)
        {
            List<NodeVariables> nodeList = new List<NodeVariables>();

            if (process == "ServiciosLegales" && step == "AnalisisLegal")
            {
                nodeList.Add(new NodeVariables() {
                    NodeName = "TaskData.Global.UserAnalisisLegal",
                    NodeValue = userID.Substring(userID.IndexOf("/") + 1)
                });

                nodeList.Add(new NodeVariables()
                {
                    NodeName = "TaskData.Global.CodJurisdiccion",
                    NodeValue = (codJurisdiccion == "1" || codJurisdiccion == "6" || codJurisdiccion == "7") ? "1" : "2"
                });

                nodeList.Add(new NodeVariables()
                {
                    NodeName = "TaskData.Global.CodProcedencia",
                    NodeValue = codProcedencia
                });
            }

            int incidente = UltimusManager.CompleteTask(userID, taskID, String.Empty, String.Empty, nodeList);
            return incidente;
        }

        [HttpGet]
        public List<Instruccion> GetInstruccion(string proceso, string incidente, int tipo, int solicitud)
        {
            List<Instruccion> lista = new List<Instruccion>();
            DataTable dt = new DataTable();

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_INSTRUCCIONES.USP_SEL_INSTRUCCION";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_PROCESO", OracleDbType.Varchar2, ParameterDirection.Input, 100, proceso);
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                Procedimiento.AddParametros("P_TIPO", OracleDbType.Int32, ParameterDirection.Input, 255, tipo.ToString());
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, solicitud==0?null:solicitud.ToString());
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                foreach (DataRow rowProducto in dt.Rows)
                {
                    Instruccion i = new Instruccion();
                    i.IdInstruccion = Convert.ToInt32(rowProducto["ID_INSTRUCCION"]);
                    i.Proceso = rowProducto["PROCESO"].ToString();
                    i.Incidente = rowProducto["INCIDENTE"].ToString();
                    i.EjecutadoPor = rowProducto["EJECUTADO_POR"].ToString();
                    i.Tipo = Convert.ToInt32(rowProducto["TIPO"]);
                    i.Instrucciones = rowProducto["INSTRUCCIONES"].ToString();
                    i.Respuesta = rowProducto["RESPUESTA"].ToString();
                    lista.Add(i);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpPost]
        public bool RegistrarInstruccion(Instruccion instruccion)
        {
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_INSTRUCCIONES.USP_INS_INSTRUCCION";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_PROCESO", OracleDbType.Varchar2, ParameterDirection.Input, 100, instruccion.Proceso);
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, instruccion.Incidente);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, instruccion.Solicitud == 0 ? null : instruccion.Solicitud.ToString());
                Procedimiento.AddParametros("P_EJECUTADO_POR", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.Empty);
                Procedimiento.AddParametros("P_TIPO", OracleDbType.Int32, ParameterDirection.Input, 255, instruccion.Tipo.ToString());
                Procedimiento.AddParametros("P_INSTRUCCIONES", OracleDbType.Clob, ParameterDirection.Input, 10000, instruccion.Instrucciones);
                Procedimiento.AddParametros("P_RESPUESTA", OracleDbType.Clob, ParameterDirection.Input, 1000, string.Empty);

                Procedimientos.Add(Procedimiento);
                return DA.EjecutarProcedimientos(Procedimientos);
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.StackTrace);
                return false;
            }
        }

        public bool ActualizarInstruccion(string proceso, string incidente, int solicitud)
        {
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_INSTRUCCIONES.USP_UDP_INSTRUCCION";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_PROCESO", OracleDbType.Varchar2, ParameterDirection.Input, 100, proceso);
                //Procedimiento.AddParametros("P_INCIDENTE_ACTUAL", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidenteActual);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, solicitud == 0 ? null : solicitud.ToString());
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);

                Procedimientos.Add(Procedimiento);
                return DA.EjecutarProcedimientos(Procedimientos);
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return false;
            }
        }

        [HttpPost]
        public bool ActualizarInstruccionRespuesta(Instruccion instruccion)
        {
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_INSTRUCCIONES.USP_UDP_INSTRUCCION_RESPUESTA";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_ID_INSTRUCCION", OracleDbType.Int32, ParameterDirection.Input, 100, instruccion.IdInstruccion.ToString());
                Procedimiento.AddParametros("P_EJECUTADO_POR", OracleDbType.Varchar2, ParameterDirection.Input, 100, instruccion.EjecutadoPor);
                Procedimiento.AddParametros("P_RESPUESTA", OracleDbType.Clob, ParameterDirection.Input, 10000, instruccion.Respuesta);

                Procedimientos.Add(Procedimiento);
                return DA.EjecutarProcedimientos(Procedimientos);
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return false;
            }
        }

        public bool ActualizarIncidenteServiciosLegales(string incidenteActual, string incidenteNuevo, int solicitud)
        {
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_SOCIEDAD.USP_UDP_SERVICIOS_LEGALES";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
               // Procedimiento.AddParametros("P_INCIDENTE_ACTUAL", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidenteActual);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, solicitud == 0 ? null : solicitud.ToString());
                Procedimiento.AddParametros("P_INCIDENTE_NUEVO", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidenteNuevo);

                Procedimientos.Add(Procedimiento);
                return DA.EjecutarProcedimientos(Procedimientos);
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return false;
            }
        }

        public bool ActualizarIncidenteRelacionadoComunicacion(string incidenteActual, string incidenteNuevo)
        {
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_SOCIEDAD.USP_UPD_COMUNICACION_INCIDENTE";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE_RELACIONADO_ACTUAL", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidenteActual);
                Procedimiento.AddParametros("P_INCIDENTE_RELACIONADO_NUEVO", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidenteNuevo);

                Procedimientos.Add(Procedimiento);
                return DA.EjecutarProcedimientos(Procedimientos);
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return false;
            }
        }

        [HttpGet]
        public int DevuelveCodSolicitud(string codigo)
        {
            bool flag = false;
            OracleDataAccess DA = new OracleDataAccess();
            DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];
            ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
            OracleStoredName Procedimiento = new OracleStoredName();
            Procedimiento.Nombre = "PAQ_DATOS_SOCIEDAD.USP_SEL_CODIGOSOLICITUD";
            Procedimiento.IsPadre = Valores.IsNotPadre;
            Procedimiento.IsHereda = false;
            Procedimiento.AddParametros("CODIGOSOLICITUD_", OracleDbType.Int32, ParameterDirection.Output, 255, codigo);
            Procedimientos.Add(Procedimiento);
            flag = DA.EjecutarProcedimientos(Procedimientos);
            return Int32.Parse(Procedimiento.Parametros[0].Value.ToString());
        }

        [System.Web.Http.AcceptVerbs("GET")]
        [HttpGet]
        public int CrearServiciosLegales(string proceso, string tempIncident, string userID, string taskID, string codAreaLegal, string notificaCliente, string codJurisdiccion, string codProcedencia, string nombreCliente, string codSolicitud)
        {
            try
            {
              
                List<NodeVariables> nodeList = new List<NodeVariables>();

                if (!string.IsNullOrEmpty(codSolicitud))
                {
                    NodeVariables node = new NodeVariables();
                    node.NodeName = "TaskData.Global.CodigoSolicitud";
                    node.NodeValue = codSolicitud;
                    nodeList.Add(node);
                }

                if (!string.IsNullOrEmpty(codAreaLegal))
                {
                    NodeVariables node = new NodeVariables();
                    node.NodeName = "TaskData.Global.CodAreaLegal";
                    node.NodeValue = codAreaLegal;
                    nodeList.Add(node);
                }

                if (!string.IsNullOrEmpty(notificaCliente))
                {
                    NodeVariables node = new NodeVariables();
                    node.NodeName = "TaskData.Global.NotificaCliente";
                    node.NodeValue = notificaCliente;
                    nodeList.Add(node);
                }

                nodeList.Add(new NodeVariables()
                {
                    NodeName = "TaskData.Global.CodJurisdiccion",
                    NodeValue = (codJurisdiccion == "1" || codJurisdiccion == "6" || codJurisdiccion == "7") ? "1" : "2"
                });

                nodeList.Add(new NodeVariables()
                {
                    NodeName = "TaskData.Global.CodProcedencia",
                    NodeValue = codProcedencia
                });

                nodeList.Add(new NodeVariables()
                {
                    NodeName = "TaskData.Global.NoAnalisisLegal",
                    NodeValue = "1"
                });

                //Obtengo si tiene instrucciones del tipo 1 (en Datos del Cliente)
                List<Instruccion> lista = GetInstruccion("ServiciosLegales", tempIncident, 1, Int32.Parse(codSolicitud));

                SolicitudAPIController s = new SolicitudAPIController();
                DatosCliente c = s.ObtenerDatosCliente(null, Int32.Parse(codSolicitud));
                //Si es un cliente bloqueado o se trata de un cliente nuevo
                if (c.EstadoCumplimiento == "N" || c.ClienteExistente == "N")
                {
                    nodeList.Add(new NodeVariables()
                    {
                        NodeName = "TaskData.Global.NecesitaCumplimiento",
                        NodeValue = "true"
                    });
                }
                else if (c.EstadoCumplimiento == "S" && lista.Count > 0)
                {
                    nodeList.Add(new NodeVariables()
                    {
                        NodeName = "TaskData.Global.NecesitaCumplimiento",
                        NodeValue = "false"
                    });
                }
                else
                {
                    nodeList.Add(new NodeVariables()
                    {
                        NodeName = "TaskData.Global.NecesitaCumplimiento",
                        NodeValue = "0"
                    });
                }

                string summary = String.Empty;
                switch (codAreaLegal)
                {
                    case "1":
                        summary = "SOCIEDADES - " + nombreCliente;
                        break;
                    case "2":
                        summary = "FUNDACIONES - " + nombreCliente;
                        break;
                    case "3":
                        summary = "MIGRACIÓN - " + nombreCliente;
                        break;
                    case "5":
                        summary = "NAVES - " + nombreCliente;
                        break;
                    default:
                        summary = nombreCliente;
                        break;
                }

                int incidente = UltimusManager.CompleteTask(userID, taskID, String.Empty, summary, nodeList);

                //Adjuntos Cliente
                AttachmentServiceClient client = new AttachmentServiceClient();
                string attachmentFolder = "_tmp_" + userID.Replace("/", "_") + " (Datos del Cliente)";
                client.MoveFiles(proceso, attachmentFolder, incidente.ToString() + " (Datos del Cliente)");

                //Adjuntos Solicitud
                attachmentFolder = "_tmp_" + userID.Replace("/", "_") + " (Datos de Solicitud)";
                client.MoveFiles(proceso, attachmentFolder, incidente.ToString() + " (Datos de Solicitud)");

                //Bitacora
                GeneralesController g = new GeneralesController();
                g.ActualizarBitacoraComentarios(proceso, tempIncident, incidente.ToString());

                //Instrucciones
                ActualizarInstruccion(proceso, incidente.ToString(), Int32.Parse(codSolicitud));

                //ServiciosLegales
                ActualizarIncidenteServiciosLegales(tempIncident, incidente.ToString(), Int32.Parse(codSolicitud));

                //Comunicacion
                ActualizarIncidenteRelacionadoComunicacion(tempIncident, incidente.ToString());

                return incidente;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return 0;
            }
        }

        [HttpPost]
        public bool EditaInstruccion(Instruccion instruccion)
        {
            OracleDataAccess DA = new OracleDataAccess();
            bool flag = false;
            try
            {
               
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_INSTRUCCIONES.USP_UDP_INSTRUCCION2";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INSTRUCCIONES", OracleDbType.Clob, ParameterDirection.Input, 10000, instruccion.Instrucciones);
                Procedimiento.AddParametros("P_IDINSTRUCCION", OracleDbType.Int32, ParameterDirection.Input, 100, instruccion.IdInstruccion.ToString());

                Procedimientos.Add(Procedimiento);
                flag = DA.EjecutarProcedimientos(Procedimientos);
                if (!flag)
                    DA.Rollback();
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }
            finally
            {
                DA.Dispose();
            }
            return flag;
        }

        [HttpPost]
        public bool BorraInstruccion(Instruccion instruccion)
        {
            OracleDataAccess DA = new OracleDataAccess();
            bool flag = false;
            try
            {
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_INSTRUCCIONES.USP_DEL_INSTRUCCION";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_IDINSTRUCCION", OracleDbType.Int32, ParameterDirection.Input, 100, instruccion.IdInstruccion.ToString());

                Procedimientos.Add(Procedimiento);
                flag = DA.EjecutarProcedimientos(Procedimientos);
                if (!flag)
                    DA.Rollback();
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }
            finally
            {
                DA.Dispose();
            }
            return flag;
        }
    }
}