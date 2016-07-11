using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Configuration;
using ULA.Quijano.SendMails.AttachmentService;
using ULA.Quijano.SendMails.UltimusIntegrationLayer;
using Ultimus.DataAccessLayer;
using Ultimus.UtilityLayer;

namespace ULA.Quijano.SendMails
{
    public class WcfMensajeria : IWcfMensajeria
    {

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

        public bool ActualizarEstadoServicio(int incidenteID, string estado, string comentariosResolucion, string fechaSeguimiento)
        {
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_SERVICIO.USP_UPD_SERVICIO";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Int32, ParameterDirection.Input, 255, incidenteID.ToString());
                Procedimiento.AddParametros("P_ID_RESPONSABLE", OracleDbType.Int32, ParameterDirection.Input, 255, string.Empty);
                Procedimiento.AddParametros("P_ESTADO", OracleDbType.Varchar2, ParameterDirection.Input, 255, string.IsNullOrEmpty(estado) ? "" : estado);
                Procedimiento.AddParametros("P_COMENTARIOS", OracleDbType.Varchar2, ParameterDirection.Input, 255, string.Empty);
                Procedimiento.AddParametros("P_COMENTARIOS_RESOLUCION", OracleDbType.Varchar2, ParameterDirection.Input, 500, comentariosResolucion);
                Procedimiento.AddParametros("P_FECHA_SEGUIMIENTO", OracleDbType.Varchar2, ParameterDirection.Input, 10, fechaSeguimiento);

                Procedimientos.Add(Procedimiento);
                return DA.EjecutarProcedimientos(Procedimientos);
            }
            catch (Exception ex)
            {
                new LogFilecs().EscribirArchivo("SendMails.WcfMensajeria", ex);
                return false;
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

    }
}
