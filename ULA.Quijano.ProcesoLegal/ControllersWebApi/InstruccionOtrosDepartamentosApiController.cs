using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using ULA.Quijano.Model;
using ULA.Quijano.ProcesoLegal.Commons;
using Ultimus.DataAccessLayer;

namespace ULA.Quijano.ProcesoLegal.ControllersWebApi
{
    public class InstruccionOtrosDepartamentosApiController : ApiController
    {
        #region LOGGER DEFINITION

        UltimusLogs UltimusLogs = new UltimusLogs("InstruccionOtrosDepartamentosApiController");

        #endregion

        [HttpGet]
        public List<Tesoreria> GetTesoreria(string incidente, int solicitud)
        {
            List<Tesoreria> lista = new List<Tesoreria>();
            DataTable dtSL = null;
            DataTable dtTesoreria = null;
            OracleDataAccess DA = null;

            try
            {
                //Obtengo el Nro de Asunto de la Tabla Maestra Servicios Legales
                DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];
                string querySL = "SELECT SERVICIOS_LEGALES.NRO_ASUNTO " +
                                   "FROM SERVICIOS_LEGALES " +
                                  "WHERE ((SERVICIOS_LEGALES.SOLICITUD is not null and SERVICIOS_LEGALES.SOLICITUD = "+solicitud+") or  (SERVICIOS_LEGALES.INCIDENTE = '" + incidente + "')) ";

                DA.GetDataTable(querySL, out dtSL);

                if (dtSL.Rows.Count > 0)
                {
                    DA = new OracleDataAccess();
                    DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                    string query = "SELECT CPV82.CODCIA, " +
                                          "CPV82.ASUNTO, " +
                                          "CPV82.TIPO, " +
                                          "CPV82.CED_RUC, " +
                                          "CPV82.BENEFICIARIO, " +
                                          "CPV82.MONTO_TOTAL, " +
                                          "CPV82.MONTO_DETALLE " +
                                     "FROM CPV82 " +
                                    "WHERE CPV82.ASUNTO = " + dtSL.Rows[0]["NRO_ASUNTO"].ToString() + " " +
                                    "ORDER BY CPV82.BENEFICIARIO";

                    DA.GetDataTable(query, out dtTesoreria);

                    foreach (DataRow rowProducto in dtTesoreria.Rows)
                    {
                        Tesoreria tesoreria = new Tesoreria();
                        tesoreria.Tipo = GetDescriptionByTipo(rowProducto["TIPO"].ToString());
                        tesoreria.CedRuc = rowProducto["CED_RUC"].ToString();
                        tesoreria.Beneficiario = rowProducto["BENEFICIARIO"].ToString();
                        tesoreria.Monto = Convert.ToDecimal(rowProducto["MONTO_TOTAL"]);
                        lista.Add(tesoreria);
                    }
                }                
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpPost]
        public bool RegistrarInstrucOtrosDeptos(InstruccionOtrosDepartamentos instruccionesOtrosDepartamentos)
        {
            DataTable dtSL = null;
            OracleDataAccess DA = null;

            try
            {
                Boolean ret = false;

                //Obtengo el Nro de Asunto de la Tabla Maestra Servicios Legales
                DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];
                string querySL = "SELECT SERVICIOS_LEGALES.NRO_ASUNTO " +
                                   "FROM SERVICIOS_LEGALES " +
                                  "WHERE ((SERVICIOS_LEGALES.SOLICITUD is not null and SERVICIOS_LEGALES.SOLICITUD = " + instruccionesOtrosDepartamentos.Solicitud+ ")  or (SERVICIOS_LEGALES.INCIDENTE = '" + instruccionesOtrosDepartamentos.Incidente + "'))";

                DA.GetDataTable(querySL, out dtSL);

                if (dtSL.Rows.Count > 0)
                {
                    instruccionesOtrosDepartamentos.NroAsunto = Convert.IsDBNull(dtSL.Rows[0]["NRO_ASUNTO"]) ? 0 : Convert.ToInt32(dtSL.Rows[0]["NRO_ASUNTO"]);

                    ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                    OracleStoredName Procedimiento = new OracleStoredName();
                    Procedimiento.Nombre = "PAQ_INSTRUC_OTROS_DEPTOS.USP_UPD_INSTRUC_OTROS_DEPTOS";
                    Procedimiento.IsPadre = Valores.IsNotPadre;
                    Procedimiento.IsHereda = false;
                    Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, instruccionesOtrosDepartamentos.Incidente);
                    Procedimiento.AddParametros("P_INTRUCCIONES", OracleDbType.Varchar2, ParameterDirection.Input, 9, instruccionesOtrosDepartamentos.Instrucciones);
                    Procedimiento.AddParametros("P_URGENTE", OracleDbType.Varchar2, ParameterDirection.Input, 1, instruccionesOtrosDepartamentos.Urgente ? "S" : "N");
                    Procedimiento.AddParametros("P_URGENTE_COMENTARIO", OracleDbType.Varchar2, ParameterDirection.Input, 500, instruccionesOtrosDepartamentos.UrgenteComentario);
                    Procedimiento.AddParametros("P_FECHA_LIMITE_ENTREGA", OracleDbType.Varchar2, ParameterDirection.Input, 100, instruccionesOtrosDepartamentos.FechaLimiteEntrega.ToString("dd/MM/yyyy"));
                    Procedimiento.AddParametros("P_TRADUCCION_DESCRIPCION", OracleDbType.Varchar2, ParameterDirection.Input, 500, instruccionesOtrosDepartamentos.TraduccionDescripcion);
                    Procedimiento.AddParametros("P_NOTARIA_PROTOCOLIZA", OracleDbType.Varchar2, ParameterDirection.Input, 200, instruccionesOtrosDepartamentos.NotariaProtocoliza);
                    Procedimiento.AddParametros("P_NOTARIA_NOTARIA", OracleDbType.Varchar2, ParameterDirection.Input, 50, instruccionesOtrosDepartamentos.NotariaNotaria);
                    Procedimiento.AddParametros("P_NOTARIA_SECCION", OracleDbType.Varchar2, ParameterDirection.Input, 200, instruccionesOtrosDepartamentos.NotariaSeccion);
                    Procedimiento.AddParametros("P_NOTARIA_MODELO", OracleDbType.Varchar2, ParameterDirection.Input, 50, instruccionesOtrosDepartamentos.NotariaModelo);
                    Procedimiento.AddParametros("P_NOTARIA_COMPARECE", OracleDbType.Varchar2, ParameterDirection.Input, 200, instruccionesOtrosDepartamentos.NotariaComparece);
                    Procedimiento.AddParametros("P_NOTARIA_REFRENDA", OracleDbType.Varchar2, ParameterDirection.Input, 200, instruccionesOtrosDepartamentos.NotariaRefrenda);
                    Procedimiento.AddParametros("P_NOTARIA_OTROS", OracleDbType.Varchar2, ParameterDirection.Input, 500, instruccionesOtrosDepartamentos.NotariaOtros);
                    Procedimiento.AddParametros("P_NRO_ASUNTO", OracleDbType.Int32, ParameterDirection.Input, 15, instruccionesOtrosDepartamentos.NroAsunto.ToString());
                    Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, instruccionesOtrosDepartamentos.Solicitud.ToString());

                    Procedimientos.Add(Procedimiento);
                    ret = DA.EjecutarProcedimientos(Procedimientos);
                }

                return ret;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return false;
            }
        }

        [HttpGet]
        public InstruccionOtrosDepartamentos ObtenerInstrucOtrosDeptos(string incidente, int solicitud)
        {
            try
            {
                InstruccionOtrosDepartamentos instruccionOtrosDepartamentos = null;
                String instrucciones = String.Empty;
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                //Obtengo las Instrucciones de DATOS_SOCIEDAD
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_INSTRUC_OTROS_DEPTOS.USP_SEL_INSTRUC_DATOS_SOCIEDAD";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, solicitud.ToString());

                DataTable dt = DA.GetDataTable(Procedimiento);

                if (dt.Rows.Count > 0)
                {
                    instrucciones = dt.Rows[0]["INSTRUCCIONES"].ToString();
                }

                Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_INSTRUC_OTROS_DEPTOS.USP_SEL_INSTRUC_OTROS_DEPTOS";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, solicitud.ToString());

                DataTable dtIOD = DA.GetDataTable(Procedimiento);

                if (dtIOD.Rows.Count > 0)
                {
                    instruccionOtrosDepartamentos = new InstruccionOtrosDepartamentos();
                    instruccionOtrosDepartamentos.Urgente = dtIOD.Rows[0]["URGENTE"].ToString().Equals("S");
                    instruccionOtrosDepartamentos.UrgenteComentario = dtIOD.Rows[0]["URGENTE_COMENTARIO"].ToString();
                    instruccionOtrosDepartamentos.FechaLimiteEntrega = Convert.IsDBNull(dtIOD.Rows[0]["FECHA_LIMITE_ENTREGA"]) ? new DateTime() : Convert.ToDateTime(dtIOD.Rows[0]["FECHA_LIMITE_ENTREGA"]);
                    instruccionOtrosDepartamentos.TraduccionDescripcion = dtIOD.Rows[0]["TRADUCCION_DESCRIPCION"].ToString();
                    instruccionOtrosDepartamentos.NotariaProtocoliza = dtIOD.Rows[0]["NOTARIA_PROTOCOLIZA"].ToString();
                    instruccionOtrosDepartamentos.NotariaNotaria = dtIOD.Rows[0]["NOTARIA_NOTARIA"].ToString();
                    instruccionOtrosDepartamentos.NotariaSeccion = dtIOD.Rows[0]["NOTARIA_SECCION"].ToString();
                    instruccionOtrosDepartamentos.NotariaModelo = dtIOD.Rows[0]["NOTARIA_MODELO"].ToString();
                    instruccionOtrosDepartamentos.NotariaComparece = dtIOD.Rows[0]["NOTARIA_COMPARECE"].ToString();
                    instruccionOtrosDepartamentos.NotariaRefrenda = dtIOD.Rows[0]["NOTARIA_REFRENDA"].ToString();
                    instruccionOtrosDepartamentos.NotariaOtros = dtIOD.Rows[0]["NOTARIA_OTROS"].ToString();
                    instruccionOtrosDepartamentos.NroAsunto = Convert.IsDBNull(dtIOD.Rows[0]["NRO_ASUNTO"]) ? 0 : Convert.ToInt32(dtIOD.Rows[0]["NRO_ASUNTO"]);

                    if (String.IsNullOrEmpty(instrucciones))
                        instrucciones = dtIOD.Rows[0]["INSTRUCCIONES"].ToString();

                    SetInstruccionesSeccion(instrucciones, instruccionOtrosDepartamentos);
                }               

                return instruccionOtrosDepartamentos;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }
        }

        #region PRIVATE

        private void SetInstruccionesSeccion(String instrucciones, InstruccionOtrosDepartamentos instruccionOtrosDepartamentos)
        {
            char[] array = instrucciones.ToCharArray();

            instruccionOtrosDepartamentos.Traduccion = array[0].ToString().Equals("1");
            instruccionOtrosDepartamentos.Autenticacion = array[1].ToString().Equals("1");
            instruccionOtrosDepartamentos.Notaria = array[2].ToString().Equals("1");
            instruccionOtrosDepartamentos.Apostilla = array[3].ToString().Equals("1");
            instruccionOtrosDepartamentos.MinrexGobJustici = array[4].ToString().Equals("1");
            instruccionOtrosDepartamentos.Consulado = array[5].ToString().Equals("1");
            instruccionOtrosDepartamentos.SeInscribe = array[6].ToString().Equals("1");
            instruccionOtrosDepartamentos.AlteracionTurno = array[7].ToString().Equals("1");
            instruccionOtrosDepartamentos.AperturaCuenta = array[8].ToString().Equals("1");
        }

        private string GetDescriptionByTipo(string tipo)
        {
            switch (tipo)
            {
                case "A": return "Cheque";
                case "B": return "Cheque de Gerencia/Management Check";
                case "C": return "Cheque Certificado";
                case "E": return "Transferencias o Giros por Cheques//Transfer or Money Order by Checks";
                case "F": return "Caja Menuda/Petty Cash";
                case "G": return "Timbres Fiscales/Fiscal Stamps";
                case "H": return "Tarjeta DE Crédito/Credit Card";
                default: return "";
            }
        }

        #endregion
    }
}