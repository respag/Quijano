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

namespace ULA.Quijano.ProcesoLegal.ControllersWebApi
{
    public class GeneralesController : ApiController
    {
        #region LOGGER DEFINITION

        UltimusLogs UltimusLogs = new UltimusLogs("SolicitudAPIMesaOperacion");

        #endregion

        [HttpGet]
        public List<Comentario> ObtenerBitacoraComentarios(string proceso, string incidente, int solicitud)
        {
            List<Comentario> lista = new List<Comentario>();
            DataTable dt = new DataTable();

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_BITACORA_COMENTARIOS.USP_SEL_COMENTARIO";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_PROCESO", OracleDbType.Varchar2, ParameterDirection.Input, 100, proceso);
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, solicitud.ToString());

                dt = DA.GetDataTable(Procedimiento);
                foreach (DataRow rowProducto in dt.Rows)
                {
                    Comentario c = new Comentario();
                    c.Proceso = rowProducto["PROCESO"].ToString();
                    c.Incidente = rowProducto["INCIDENTE"].ToString();
                    c.Usuario = rowProducto["USUARIO"].ToString();
                    c.Fecha = Convert.ToDateTime(rowProducto["FECHA"]);
                    c.strFecha = Convert.ToDateTime(rowProducto["FECHA"]).ToString("dd-MMMM-yyyy HH:mm:ss", new CultureInfo("es-AR"));
                    c.Texto = rowProducto["COMENTARIO"].ToString();
                    lista.Add(c);
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
        public bool GuardarBitacoraComentarios(string proceso, string incidente, string usuario, string comentario, int solicitud)
        {
            if (comentario != null)
            {
                try
                {
                    OracleDataAccess DA = new OracleDataAccess();
                    DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                    ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                    OracleStoredName Procedimiento = new OracleStoredName();
                    Procedimiento.Nombre = "PAQ_BITACORA_COMENTARIOS.USP_INS_COMENTARIO";
                    Procedimiento.IsPadre = Valores.IsNotPadre;
                    Procedimiento.IsHereda = false;
                    Procedimiento.AddParametros("P_PROCESO", OracleDbType.Varchar2, ParameterDirection.Input, 100, proceso);
                    Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                    Procedimiento.AddParametros("P_USUARIO", OracleDbType.Varchar2, ParameterDirection.Input, 200, usuario);
                    Procedimiento.AddParametros("P_COMENTARIO", OracleDbType.Clob, ParameterDirection.Input, 10000, comentario);
                    Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, solicitud.ToString());

                    Procedimientos.Add(Procedimiento);
                    return DA.EjecutarProcedimientos(Procedimientos);
                }
                catch (Exception ex)
                {
                    UltimusLogs.Error(ex.ToString());
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public bool ActualizarBitacoraComentarios(string proceso, string incidenteActual, string incidenteNuevo)
        {
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_BITACORA_COMENTARIOS.USP_UDP_COMENTARIO";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_PROCESO", OracleDbType.Varchar2, ParameterDirection.Input, 100, proceso);
                Procedimiento.AddParametros("P_INCIDENTE_ACTUAL", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidenteActual);
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

    }
}