using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ULA.Quijano.ProcesoLegal.Commons;
using ULA.Quijano.ProcesoLegal.Json;
using NLog;
using ULA.Quijano.ProcesoLegal.Helpers;
using System.Globalization;
using System.Data.Entity.Validation;
using ULA.Quijano.Model;
using Ultimus.DataAccessLayer;
using System.Web.Configuration;
using Oracle.DataAccess.Client;
using System.Data;
using Ultimus.UtilityLayer;
using Ultimus.Utilitarios;
using ULA.Quijano.ProcesoLegal.UltimusIntegrationLayer;
using ULA.Quijano.ProcesoLegal.AttachmentService;

namespace ULA.Quijano.ProcesoLegal.ControllersWebApi
{
    public class MesaOperacionController : ApiController
    {
        #region LOGGER DEFINITION

        UltimusLogs UltimusLogs = new UltimusLogs("SolicitudAPIMesaOperacion");

        #endregion

        [HttpGet]
        public List<CatalogoTipoSolicitud> GetTipoSolicitud()
        {
            List<CatalogoTipoSolicitud> lista = new List<CatalogoTipoSolicitud>();
            DataTable dt = new DataTable();

            try
            {

                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_SERVICIO.USP_SEL_TIPO_SOLICITUD";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                foreach (DataRow rowProducto in dt.Rows)
                {
                    CatalogoTipoSolicitud sol = new CatalogoTipoSolicitud();
                    sol.Codigo = Convert.ToInt32(rowProducto["CODIGO"]);
                    sol.Descripcion = rowProducto["DESCRIPCION"].ToString();
                    lista.Add(sol);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
        public List<CatalogoEntidades> GetEntidades()
        {
            List<CatalogoEntidades> lista = new List<CatalogoEntidades>();
            DataTable dt = new DataTable();

            try
            {

                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_SERVICIO.USP_SEL_ENTIDADES";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                foreach (DataRow rowProducto in dt.Rows)
                {
                    CatalogoEntidades ent = new CatalogoEntidades();
                    ent.Codigo = Convert.ToInt32(rowProducto["CODIGOENTIDAD"]);
                    ent.Entidad = rowProducto["ENTIDAD"].ToString();
                    lista.Add(ent);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
        public List<CatalogoAcciones> GetAcciones()
        {
            List<CatalogoAcciones> lista = new List<CatalogoAcciones>();
            DataTable dt = new DataTable();

            try
            {

                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_SERVICIO.USP_SEL_ACCIONES";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                foreach (DataRow rowProducto in dt.Rows)
                {
                    CatalogoAcciones acc = new CatalogoAcciones();
                    acc.Codigo = Convert.ToInt32(rowProducto["CODIGOACCION"]);
                    acc.Descripcion = rowProducto["DESCRIPCION"].ToString();
                    lista.Add(acc);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [HttpPost]
        public int RegistrarServicio(string proceso,
                                     string tempIncident,
                                     string userID,
                                     string taskID, 
                                     string solicitud,
                                     string idEntidad,
                                     string socEntEmp,
                                     string nomPara,
                                     string nomDe,
                                     string tramite,
                                     string detalle,
                                     string direccion,
                                     string piso,
                                     string calle,
                                     string referencia,
                                     string horario,
                                     string telefono,
                                     string asunto,
                                     string idAccion,
                                     string responsable,
                                     string estado,
                                     string comentarios,
                                     string correoIniciador,
                                     string solicitadoPor)
        {
            bool confirm = false;
            int incidente = 0;
            try
            {
                List<NodeVariables> nodeVariables = new List<NodeVariables>();
                nodeVariables.Add(new NodeVariables()
                {
                    NodeName = "TaskData.Global.EmailRecSolicitarServicio",
                    NodeValue = correoIniciador
                });

                if (string.IsNullOrEmpty(taskID))
                {
                    ULA.Quijano.ProcesoLegal.UltimusIntegrationLayer.UltimusIncident ui = UltimusManager.GetTaskByFilters(userID, proceso, "SolicitarServicio", 0);
                    taskID = ui.TaskId;
                }

                incidente = UltimusManager.CompleteTask(userID, taskID, String.Empty, solicitud == "1" ? "Solicitud de Pasante (" + socEntEmp + ")" : (solicitud == "2" ? "Solicitud de Mensajero (" + socEntEmp + ")" : ""), nodeVariables);
                GeneralesController g = new GeneralesController();
                g.ActualizarBitacoraComentarios(proceso, tempIncident, incidente.ToString());

                AttachmentServiceClient client = new AttachmentServiceClient();
                string attachmentFolder = "_tmp_" + userID.Replace("/", "_");
                client.MoveFiles(proceso, attachmentFolder, incidente.ToString());

                //Inserta en la base de datos
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_SERVICIO.USP_INS_SERVICIO";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Int32, ParameterDirection.Input, 255, incidente.ToString());
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Varchar2, ParameterDirection.Input, 20, solicitud.ToString());
                Procedimiento.AddParametros("P_ID_ENTIDAD", OracleDbType.Int32, ParameterDirection.Input, 255, string.IsNullOrEmpty(idEntidad) || idEntidad == "-1" ? "" : idEntidad);
                Procedimiento.AddParametros("P_SOCIEDAD_ENTIDAD_EMPRESA", OracleDbType.Varchar2, ParameterDirection.Input, 100, string.IsNullOrEmpty(socEntEmp) ? "" : socEntEmp);
                Procedimiento.AddParametros("P_NOMBRE_PARA", OracleDbType.Varchar2, ParameterDirection.Input, 250, string.IsNullOrEmpty(nomPara) ? "" : nomPara);
                Procedimiento.AddParametros("P_NOMBRE_DE", OracleDbType.Varchar2, ParameterDirection.Input, 250, string.IsNullOrEmpty(nomDe) ? "" : nomDe);
                Procedimiento.AddParametros("P_TRAMITE", OracleDbType.Varchar2, ParameterDirection.Input, 100, string.IsNullOrEmpty(tramite) ? "" : tramite);
                Procedimiento.AddParametros("P_DETALLE", OracleDbType.Varchar2, ParameterDirection.Input, 4000, string.IsNullOrEmpty(detalle) ? "" : detalle);
                Procedimiento.AddParametros("P_DIRECCION", OracleDbType.Varchar2, ParameterDirection.Input, 500, string.IsNullOrEmpty(direccion) ? "" : direccion);
                Procedimiento.AddParametros("P_PISO", OracleDbType.Varchar2, ParameterDirection.Input, 20, string.IsNullOrEmpty(piso) ? "" : piso);
                Procedimiento.AddParametros("P_CALLE", OracleDbType.Varchar2, ParameterDirection.Input, 20, string.IsNullOrEmpty(calle) ? "" : calle);
                Procedimiento.AddParametros("P_REFERENCIAS", OracleDbType.Varchar2, ParameterDirection.Input, 500, string.IsNullOrEmpty(referencia) ? "" : referencia);
                Procedimiento.AddParametros("P_HORARIO", OracleDbType.Varchar2, ParameterDirection.Input, 100, string.IsNullOrEmpty(horario) ? "" : horario);
                Procedimiento.AddParametros("P_TELEFONO", OracleDbType.Varchar2, ParameterDirection.Input, 50, string.IsNullOrEmpty(telefono) ? "" : telefono);
                Procedimiento.AddParametros("P_ASUNTO", OracleDbType.Varchar2, ParameterDirection.Input, 250, string.IsNullOrEmpty(asunto) ? "" : asunto);
                Procedimiento.AddParametros("P_ID_ACCION", OracleDbType.Int32, ParameterDirection.Input, 255, string.IsNullOrEmpty(idAccion) ? "" : idAccion);
                Procedimiento.AddParametros("P_ID_RESPONSABLE", OracleDbType.Int32, ParameterDirection.Input, 255, string.IsNullOrEmpty(responsable) ? "" : responsable);
                Procedimiento.AddParametros("P_ESTADO", OracleDbType.Varchar2, ParameterDirection.Input, 50, string.IsNullOrEmpty(estado) ? "" : estado);
                Procedimiento.AddParametros("P_COMENTARIOS", OracleDbType.Varchar2, ParameterDirection.Input, 500, string.IsNullOrEmpty(comentarios) ? "" : comentarios);
                Procedimiento.AddParametros("P_SOLICITADO_POR", OracleDbType.Varchar2, ParameterDirection.Input, 100, string.IsNullOrEmpty(solicitadoPor) ? "" : solicitadoPor);

                Procedimientos.Add(Procedimiento);
                confirm = DA.EjecutarProcedimientos(Procedimientos);
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return confirm ? incidente : 0;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [HttpPost]
        public bool RegistrarServicio(string userID,
                                     string taskID,
                                     int incidente,
                                     string solicitud,
                                     string socEntEmp,
                                     string idResponsable,
                                     string usuarioResponsable,
                                     string correoResponsable,
                                     string comentarios)
        {
            bool confirm = false;
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_SERVICIO.USP_UPD_SERVICIO";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Int32, ParameterDirection.Input, 255, incidente.ToString());
                Procedimiento.AddParametros("P_ID_RESPONSABLE", OracleDbType.Int32, ParameterDirection.Input, 255, string.IsNullOrEmpty(idResponsable) ? "" : idResponsable);
                Procedimiento.AddParametros("P_ESTADO", OracleDbType.Varchar2, ParameterDirection.Input, 50, string.Empty);
                Procedimiento.AddParametros("P_COMENTARIOS", OracleDbType.Varchar2, ParameterDirection.Input, 500, string.IsNullOrEmpty(comentarios) ? "" : comentarios);
                Procedimiento.AddParametros("P_COMENTARIOS_RESOLUCION", OracleDbType.Varchar2, ParameterDirection.Input, 500, string.Empty);
                Procedimiento.AddParametros("P_FECHA_SEGUIMIENTO", OracleDbType.Varchar2, ParameterDirection.Input, 10, string.Empty);

                Procedimientos.Add(Procedimiento);
                confirm = DA.EjecutarProcedimientos(Procedimientos);

                if (confirm)
                {
                    List<NodeVariables> nodeList = new List<NodeVariables>();
                    nodeList.Add(new NodeVariables() {
                            NodeName = "TaskData.Global.RecResuelveSolicitud",
                            NodeValue = usuarioResponsable
                        });
                    nodeList.Add(new NodeVariables()
                    {
                        NodeName = "TaskData.Global.EmailResuelveSolicitud",
                        NodeValue = correoResponsable
                    });
                    incidente = UltimusManager.CompleteTask(userID, taskID, String.Empty, solicitud == "1" ? "Solicitud de Pasante (" + socEntEmp + ")" : (solicitud == "2" ? "Solicitud de Mensajero (" + socEntEmp + ")" : ""), nodeList);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return confirm;
        }

        [HttpGet]
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
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
        public List<PersonalServicio> GetPersonalServicio(int tipo)
        {
            List<PersonalServicio> lista = new List<PersonalServicio>();
            DataTable dt = new DataTable();

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_SERVICIO.USP_SEL_PERSONAL_SERVICIO";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_TIPO", OracleDbType.Int32, ParameterDirection.Input, 255, tipo.ToString());
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                foreach (DataRow rowProducto in dt.Rows)
                {
                    PersonalServicio p = new PersonalServicio();
                    p.Codigo = Convert.ToInt32(rowProducto["CODIGO"]);
                    p.Usuario = rowProducto["NOMBRE_USUARIO"].ToString();
                    p.Nombre = rowProducto["NOMBRE_COMPLETO"].ToString();
                    p.Correo = rowProducto["CORREO_ELECTRONICO"].ToString();
                    p.Tipo = rowProducto["TIPO"].ToString();
                    lista.Add(p);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
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
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
        public string ObtieneCorreoAdminMesaOp()
        {
            UltimusUser usuario;
            string err;
            ULA.Quijano.ProcesoLegal.UltimusIntegrationLayer.UltimusIntegrationClient obj = new UltimusIntegrationClient();
            obj.GetUserForJobFunction("Business Organization", "MesaOperaciones", "Administrador Mesa de Operaciones", out usuario, out err);
            return usuario.EmailAddress;
        }
    }
}