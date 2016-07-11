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
    public class DatosFundacionController : ApiController
    {
        #region LOGGER DEFINITION

        UltimusLogs UltimusLogs = new UltimusLogs("DatosFundacionController");

        #endregion

        [HttpGet]
        public List<DatosFundacion> BuscarFundacionesById(string id)
        {
            List<DatosFundacion> lista = new List<DatosFundacion>();
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query = "SELECT SOV06.CODSOCIEDAD, SOV06.NOMBRE " +
                                 "FROM SOV06 " +
                                "WHERE CODSOCIEDAD LIKE '%" + id + "%' " +
                                " AND FUNDACION = 'S' " +
                                "ORDER BY SOV06.NOMBRE";

                DA.GetDataTable(query, out dt);

                foreach (DataRow rowProducto in dt.Rows)
                {
                    DatosFundacion fundacion = new DatosFundacion();
                    fundacion.CodFundacion = rowProducto["CODSOCIEDAD"].ToString();
                    fundacion.NombreFundacion = rowProducto["NOMBRE"].ToString();
                    lista.Add(fundacion);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
        public List<DatosFundacion> BuscarFundacionesByName(string nombre)
        {
            List<DatosFundacion> lista = new List<DatosFundacion>();
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query = "SELECT SOV06.CODSOCIEDAD, SOV06.NOMBRE " +
                                 "FROM SOV06 " +
                                "WHERE NOMBRE LIKE '%" + nombre.ToUpper() + "%' " +
                                " AND FUNDACION = 'S' " +
                                "ORDER BY SOV06.NOMBRE";

                DA.GetDataTable(query, out dt);

                foreach (DataRow rowProducto in dt.Rows)
                {
                    DatosFundacion fundacion = new DatosFundacion();
                    fundacion.CodFundacion = rowProducto["CODSOCIEDAD"].ToString();
                    fundacion.NombreFundacion = rowProducto["NOMBRE"].ToString();
                    lista.Add(fundacion);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
        public DatosFundacion GetFundacionById(string id)
        {
            DatosFundacion datosFundacion = null;
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query = "SELECT SOV06.*, " +
                                      "SOV06.CODCIA AS COD_CIA, " +
                                      "(SELECT GSVCIAS.NOMBRECIA FROM GSVCIAS WHERE GSVCIAS.CODCIA = SOV06.CODCIA) AS DESCRIPCION_CIA, " +
                                      "(SELECT TRV02.NOMBRE FROM TRV02 WHERE TRV02.CODJURISDICCION = SOV06.CODJURISDICCION) AS DESC_BAJO_LEYES, " +
                                      "(SELECT CCV61.CLIENTE FROM CCV61 WHERE CLIENTE = SOV06.CLIENTE) AS COD_CLIENTE, " +
                                      "(SELECT CCV61.NOMBRE FROM CCV61 WHERE CLIENTE = SOV06.CLIENTE) AS NOMBRE_CLIENTE, " +
                                      "(SELECT CCV61.CLIENTE FROM CCV61 WHERE CLIENTE = SOV06.RESPONSABLE) AS COD_RESP_ANUALIDAD, " +
                                      "(SELECT CCV61.NOMBRE FROM CCV61 WHERE CLIENTE = SOV06.RESPONSABLE) AS NOMBRE_RESP_ANUALIDAD, " +
                                      "CASE " +
                                        "WHEN SOV06.IDIOMA = 'ESP' THEN '2' " +
                                        "WHEN SOV06.IDIOMA = 'ES/IN/AL' THEN '1' " +
                                        "WHEN SOV06.IDIOMA = 'ING' THEN '1' " +
                                        "WHEN SOV06.IDIOMA = 'ESP/ING' THEN '2' " +
                                        "ELSE '1' " +
                                      "END AS CODIDIOMA " +
                                "FROM SOV06 " +
                                "WHERE CODSOCIEDAD = '" + id  + "'";

                DA.GetDataTable(query, out dt);

                if (dt.Rows.Count > 0)
                {
                    datosFundacion = new DatosFundacion()
                    {
                        FundacionExistente = "1",
                        CodFundacion = dt.Rows[0]["CODSOCIEDAD"].ToString(),
                        NombreFundacion = dt.Rows[0]["NOMBRE"].ToString(),
                        TipoFundacion = dt.Rows[0]["TIPOCORP"].ToString(),
                        CodCia = dt.Rows[0]["COD_CIA"].ToString(),
                        DescripcionCia = dt.Rows[0]["DESCRIPCION_CIA"].ToString(), 
                        Fundador = dt.Rows[0]["FUNDADOR"].ToString(),
                        CodBajoLeyes = dt.Rows[0]["CODJURISDICCION"].ToString(),
                        DescripcionBajoLeyes = dt.Rows[0]["DESC_BAJO_LEYES"].ToString(),                        
                        IsPendienteCambioClt = Convert.IsDBNull(dt.Rows[0]["PENDIENTE_CAMBIO_CLT"]) ? false : true,
                        PendienteCambioClt = Convert.IsDBNull(dt.Rows[0]["PENDIENTE_CAMBIO_CLT"]) ? "" : dt.Rows[0]["PENDIENTE_CAMBIO_CLT"].ToString(),
                        CodigoCliente = dt.Rows[0]["COD_CLIENTE"].ToString(),
                        NombreCliente = dt.Rows[0]["NOMBRE_CLIENTE"].ToString(),
                        Duracion = dt.Rows[0]["DURACION"].ToString(),
                        RespAnualidadCodigo = dt.Rows[0]["COD_RESP_ANUALIDAD"].ToString(),
                        RespAnualidadNombre = dt.Rows[0]["NOMBRE_RESP_ANUALIDAD"].ToString(),
                        NoReg = dt.Rows[0]["NOREGISTROTEXT"].ToString().Trim(),
                        NombreAnterior = dt.Rows[0]["NOMBREANTERIOR"].ToString(),
                        RUC = dt.Rows[0]["TOMOROLLO"].ToString() + " - " + dt.Rows[0]["FOLIOIMAGEN"].ToString() + " - " + dt.Rows[0]["ASIENTOFICHA"].ToString(),
                        FechaInscripcion = Convert.IsDBNull(dt.Rows[0]["FECHAINSCRIP"]) ? new DateTime() : Convert.ToDateTime(dt.Rows[0]["FECHAINSCRIP"].ToString()),
                        Idioma = dt.Rows[0]["CODIDIOMA"].ToString(),
                        Oficina = dt.Rows[0]["OFICINA"].ToString(),
                        Patrimonio = dt.Rows[0]["CAPITALAUTORIZADO"].ToString(),
                        Estatus = dt.Rows[0]["STATUS"].ToString(),
                        ProveeDirectores = dt.Rows[0]["JDIRECTIVA"].ToString() == "S" ? "1" : "0",
                        Consejos = new List<ConsejoFundacional>(),
                        AnalisisAntiguedad = new List<AnalisisAntiguedad>()
                    };

                    //Consejo Fundacional
                    query = "SELECT * FROM SOV04 WHERE CODSOCIEDAD = " + id + " AND CODCIA = " + datosFundacion.CodCia + " ORDER BY SECUENCIA";
                    DA.GetDataTable(query, out dt);
                    datosFundacion.Consejos = dt.Rows.Cast<DataRow>().Select(r => new ConsejoFundacional()
                    {
                        Secuencia = r["SECUENCIA"].ToString(),
                        Nombre = r["NOMBRE"].ToString(),
                        Identificacion = r["CEDULA"].ToString(),
                        Titulo = r["TITULO"].ToString()
                    }).ToList();


                    //Tarifas
                    query = "SELECT * FROM SOV14 WHERE CODSOCIEDAD = " + id + " AND CODCIA = " + datosFundacion.CodCia + " AND CLIENTE = " + datosFundacion.CodigoCliente;
                    DA.GetDataTable(query, out dt);

                    if (dt.Rows.Cast<DataRow>().Count(r => r["DESCRIPCION"].ToString() == "TASA UNICA ANUAL") > 0)
                        datosFundacion.Tasa = dt.Rows.Cast<DataRow>().First(r => r["DESCRIPCION"].ToString() == "TASA UNICA ANUAL")["MONTO_GASTO"].ToString();
                    else
                        datosFundacion.Tasa = "0";

                    if (dt.Rows.Cast<DataRow>().Count(r => r["DESCRIPCION"].ToString() == "AGENTE RESIDENTE") > 0)
                        datosFundacion.AgenteResid = dt.Rows.Cast<DataRow>().First(r => r["DESCRIPCION"].ToString() == "AGENTE RESIDENTE")["MONTO_GASTO"].ToString();
                    else
                        datosFundacion.AgenteResid = "0";

                    if (dt.Rows.Cast<DataRow>().Count(r => r["DESCRIPCION"].ToString() == "DIRECTORES Y DIGNATARIOS") > 0)
                        datosFundacion.DirectoresQA = dt.Rows.Cast<DataRow>().First(r => r["DESCRIPCION"].ToString() == "DIRECTORES Y DIGNATARIOS")["MONTO_GASTO"].ToString();
                    else
                        datosFundacion.DirectoresQA = "0";


                    //Analisis Antiguedad
                    query = "SELECT * FROM CCV166SOC WHERE CODSOCIEDAD = '" + id + "' AND CODCIA = " + datosFundacion.CodCia;
                    DA.GetDataTable(query, out dt);

                    AnalisisAntiguedad tramites = null;
                    if (dt.Rows.Cast<DataRow>().Count(r => r["ANUAL_ASUN"].ToString() == "AS") > 0)
                    {
                        DataRow dr = dt.Rows.Cast<DataRow>().First(r => r["ANUAL_ASUN"].ToString() == "AS");
                        tramites = new AnalisisAntiguedad()
                        {
                            Secuencia = "1",
                            Descripcion = "Trámites",
                            SaldoCorriente = dr["SALDO_CORRIENTE"].ToString(),
                            Saldo30 = dr["SALDO_30"].ToString(),
                            Saldo60 = dr["SALDO_60"].ToString(),
                            Saldo90 = dr["SALDO_90"].ToString(),
                            Saldo120 = dr["SALDO_120"].ToString(),
                            Saldo360 = dr["SALDO_360"].ToString(),
                            SaldoMas360 = dr["SALDO_MAS360"].ToString(),
                            SaldoTotal = dr["SALDO_TOTAL"].ToString()
                        };
                    }
                    else
                    {
                        tramites = new AnalisisAntiguedad() { Secuencia = "1", Descripcion = "Trámites" };
                    }                        

                    AnalisisAntiguedad anualidades = null;
                    if (dt.Rows.Cast<DataRow>().Count(r => r["ANUAL_ASUN"].ToString() == "AN") > 0)
                    {
                        DataRow dr = dt.Rows.Cast<DataRow>().First(r => r["ANUAL_ASUN"].ToString() == "AN");
                        anualidades = new AnalisisAntiguedad()
                        {
                            Secuencia = "2",
                            Descripcion = "Anualidades",
                            SaldoCorriente = dr["SALDO_CORRIENTE"].ToString(),
                            Saldo30 = dr["SALDO_30"].ToString(),
                            Saldo60 = dr["SALDO_60"].ToString(),
                            Saldo90 = dr["SALDO_90"].ToString(),
                            Saldo120 = dr["SALDO_120"].ToString(),
                            Saldo360 = dr["SALDO_360"].ToString(),
                            SaldoMas360 = dr["SALDO_MAS360"].ToString(),
                            SaldoTotal = dr["SALDO_TOTAL"].ToString()
                        };
                    }
                    else
                    {
                        anualidades = new AnalisisAntiguedad() { Secuencia = "2", Descripcion = "Anualidades" };
                    }                        

                    double temp1, temp2;
                    AnalisisAntiguedad totales = new AnalisisAntiguedad()
                    {
                        Secuencia = "3",
                        Descripcion = "Totales",
                        SaldoCorriente = ((double.TryParse(tramites.SaldoCorriente, out temp1) ? temp1 : 0) + (double.TryParse(anualidades.SaldoCorriente, out temp2) ? temp2 : 0)).ToString(),
                        Saldo30 = ((double.TryParse(tramites.Saldo30, out temp1) ? temp1 : 0) + (double.TryParse(anualidades.Saldo30, out temp2) ? temp2 : 0)).ToString(),
                        Saldo60 = ((double.TryParse(tramites.Saldo60, out temp1) ? temp1 : 0) + (double.TryParse(anualidades.Saldo60, out temp2) ? temp2 : 0)).ToString(),
                        Saldo90 = ((double.TryParse(tramites.Saldo90, out temp1) ? temp1 : 0) + (double.TryParse(anualidades.Saldo90, out temp2) ? temp2 : 0)).ToString(),
                        Saldo120 = ((double.TryParse(tramites.Saldo120, out temp1) ? temp1 : 0) + (double.TryParse(anualidades.Saldo120, out temp2) ? temp2 : 0)).ToString(),
                        Saldo360 = ((double.TryParse(tramites.Saldo360, out temp1) ? temp1 : 0) + (double.TryParse(anualidades.Saldo360, out temp2) ? temp2 : 0)).ToString(),
                        SaldoMas360 = ((double.TryParse(tramites.SaldoMas360, out temp1) ? temp1 : 0) + (double.TryParse(anualidades.SaldoMas360, out temp2) ? temp2 : 0)).ToString(),
                        SaldoTotal = ((double.TryParse(tramites.SaldoTotal, out temp1) ? temp1 : 0) + (double.TryParse(anualidades.SaldoTotal, out temp2) ? temp2 : 0)).ToString()
                    };

                    datosFundacion.AnalisisAntiguedad = new List<AnalisisAntiguedad>();
                    datosFundacion.AnalisisAntiguedad.Add(tramites);
                    datosFundacion.AnalisisAntiguedad.Add(anualidades);
                    datosFundacion.AnalisisAntiguedad.Add(totales);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return datosFundacion;
        }

        [HttpGet]
        public DatosFundacion GetFundacionByNombre(string nombre)
        {
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query = "SELECT CODSOCIEDAD FROM SOV06 WHERE NOMBRE LIKE '%" + nombre.ToUpper() + "%'";
                DA.GetDataTable(query, out dt);

                if (dt.Rows.Count > 0)
                    return GetFundacionById(dt.Rows[0]["CODFUNDACION"].ToString());
                else
                    return null;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }
        }

        [HttpGet]
        public List<TipoFundacion> GetTipoFundacion()
        {
            List<TipoFundacion> lista = new List<TipoFundacion>();
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query = "SELECT SOV237.CODIGO, SOV237.DESCRIPCION " + 
                                "FROM SOV237 " +
                               "WHERE SOV237.PARTICULARIDAD = 'FU' " +
                               "ORDER BY SOV237.DESCRIPCION";

                DA.GetDataTable(query, out dt);

                foreach (DataRow rowProducto in dt.Rows)
                {
                    TipoFundacion tipoFundacion = new TipoFundacion();
                    tipoFundacion.CodTipoFundacion = rowProducto["CODIGO"].ToString();
                    tipoFundacion.Descripcion = rowProducto["DESCRIPCION"].ToString();
                    lista.Add(tipoFundacion);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        public List<ConsejoFundacional> GetConsejoFundacional(string incidente, string solicitud)
        {
            DataTable dt = new DataTable();

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_FUNDACION.USP_SEL_CONSEJO_FUNDACIONAL";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, solicitud.ToString());
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                return dt.Rows.Cast<DataRow>().Select(r => new ConsejoFundacional()
                {
                    Secuencia = r["SECUENCIA"].ToString(),
                    Nombre = r["NOMBRE"].ToString(),
                    Identificacion = r["IDENTIFICACION"].ToString(),
                    Titulo = r["TITULO"].ToString()
                }).ToList();
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }
        }

        [HttpGet]
        public DatosFundacion ObtenerDatosFundacion(string incidente, string etapa,int solicitud)
        {
            try
            {
                DatosFundacion d = null;
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_FUNDACION.USP_SEL_DATOS_FUNDACION";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, solicitud.ToString());
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                DataTable dt = DA.GetDataTable(Procedimiento);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["FUNDACION_EXISTENTE"].ToString() == "1" && !string.IsNullOrEmpty(dt.Rows[0]["COD_FUNDACION"].ToString()))
                        d = GetFundacionById(dt.Rows[0]["COD_FUNDACION"].ToString());

                    if (d != null)
                    {
                        d.Incidente = incidente;
                        d.Honorarios = dt.Rows[0]["HONORARIOS"].ToString();
                        d.Gastos = dt.Rows[0]["GASTOS"].ToString();
                        d.Total = dt.Rows[0]["TOTAL"].ToString();
                    }
                    else
                    {
                        d = new DatosFundacion()
                        {
                            Incidente = incidente,
                            FundacionExistente = dt.Rows[0]["FUNDACION_EXISTENTE"].ToString(),
                            CodFundacion = dt.Rows[0]["COD_FUNDACION"].ToString(),
                            NombreFundacion = dt.Rows[0]["NOMBRE_FUNDACION"].ToString(),
                            TipoFundacion = dt.Rows[0]["TIPO_FUNDACION"].ToString(),
                            CodCia = dt.Rows[0]["COD_CIA"].ToString(),
                            DescripcionCia = dt.Rows[0]["DESCRIPCION_CIA"].ToString(),
                            Fundador = dt.Rows[0]["FUNDADOR"].ToString(),
                            CodBajoLeyes = dt.Rows[0]["COD_BAJO_LEYES"].ToString(),
                            DescripcionBajoLeyes = dt.Rows[0]["DESCRIPCION_BAJO_LEYES"].ToString(),
                            IsPendienteCambioClt = dt.Rows[0]["IS_PENDIENTE_CAMBIO_CLT"].ToString() == "1",
                            PendienteCambioClt = dt.Rows[0]["PENDIENTE_CAMBIO_CLT"].ToString(),
                            CodigoCliente = dt.Rows[0]["COD_CLIENTE"].ToString(),
                            NombreCliente = dt.Rows[0]["NOMBRE_CLIENTE"].ToString(),
                            Duracion = dt.Rows[0]["DURACION"].ToString(),
                            RespAnualidadCodigo = dt.Rows[0]["RESP_ANUALIDAD_COD"].ToString(),
                            RespAnualidadNombre = dt.Rows[0]["RESP_ANUALIDAD_NOMBRE"].ToString(),
                            NoReg = dt.Rows[0]["NO_REG"].ToString(),
                            NombreAnterior = dt.Rows[0]["NOMBRE_ANTERIOR"].ToString(),
                            RUC = dt.Rows[0]["RUC"].ToString(),
                            FechaInscripcion = Convert.IsDBNull(dt.Rows[0]["FECHA_INSCRIPCION"]) ? new DateTime() : Convert.ToDateTime(dt.Rows[0]["FECHA_INSCRIPCION"]),
                            Idioma = dt.Rows[0]["IDIOMA"].ToString(),
                            Oficina = dt.Rows[0]["OFICINA"].ToString(),
                            PropositoFundacion = dt.Rows[0]["PROPOSITO_FUNDACION"].ToString(),
                            Patrimonio = dt.Rows[0]["PATRIMONIO"].ToString(),
                            Estatus = dt.Rows[0]["ESTATUS"].ToString(),
                            ProveeDirectores = dt.Rows[0]["PROVEE_DIRECTORES"].ToString(),
                            Tasa = dt.Rows[0]["TASA"].ToString(),
                            AgenteResid = dt.Rows[0]["AGENTE_RESID"].ToString(),
                            DirectoresQA = dt.Rows[0]["DIRECTORES_QA"].ToString(),
                            Honorarios = dt.Rows[0]["HONORARIOS"].ToString(),
                            Gastos = dt.Rows[0]["GASTOS"].ToString(),
                            Total = dt.Rows[0]["TOTAL"].ToString(),
                            Consejos = GetConsejoFundacional(incidente, solicitud.ToString())
                        };
                    }
                }
                else
                {
                    d = new DatosFundacion();
                }

                //Consultar informacion ingresada en pestaña Datos Cliente
                SolicitudAPIController s = new SolicitudAPIController();
                DatosCliente c = s.ObtenerDatosCliente(incidente,solicitud);
                if (c != null)
                {
                    d.CodigoCliente = c.Cod == 0 ? "" : c.Cod.ToString();
                    d.NombreCliente = c.Nombre == null ? "" : c.Nombre;
                }

                return d;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }
        }

        [HttpPost]
        public bool GuardarCheckListControl(CheckListControlFundacion checkListControlFundacion)
        {
            try
            {
                Boolean ret = false;
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_FUNDACION.USP_INS_CHECK_LIST_CONTROL";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 50, checkListControlFundacion.Incidente);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, checkListControlFundacion.Solicitud.ToString());
                Procedimiento.AddParametros("P_CODIGO_FUNDACION", OracleDbType.Varchar2, ParameterDirection.Input, 20, checkListControlFundacion.CodigoFundacion);
                Procedimiento.AddParametros("P_ACTA_FUNDACIONAL", OracleDbType.Varchar2, ParameterDirection.Input, 1, checkListControlFundacion.ActaFundacional);
                Procedimiento.AddParametros("P_REGLAMENTOS", OracleDbType.Varchar2, ParameterDirection.Input, 1, checkListControlFundacion.Reglamentos);
                Procedimiento.AddParametros("P_DESIGNACION_PROTECTOR", OracleDbType.Varchar2, ParameterDirection.Input, 1, checkListControlFundacion.DesignacionProtector);
                Procedimiento.AddParametros("P_PODER_GEN_ESP", OracleDbType.Varchar2, ParameterDirection.Input, 1, checkListControlFundacion.PoderGenEsp);
                Procedimiento.AddParametros("P_PI_NOMBRE_PRIMER_BEN", OracleDbType.Varchar2, ParameterDirection.Input, 1, checkListControlFundacion.PINombrePrimerBen);
                Procedimiento.AddParametros("P_PRUEBA_DOM_PRIMER_BEN", OracleDbType.Varchar2, ParameterDirection.Input, 1, checkListControlFundacion.PruebaDomPrimerBen);
                Procedimiento.AddParametros("P_COPIA_PAS_ID_PRIMER_BEN", OracleDbType.Varchar2, ParameterDirection.Input, 1, checkListControlFundacion.CopiaPasIdPrimerBen);
                Procedimiento.AddParametros("P_CARTA_REF_BANCARIA", OracleDbType.Varchar2, ParameterDirection.Input, 1, checkListControlFundacion.CartaRefBancaria);
                Procedimiento.AddParametros("P_CARTA_REF_PROFESIONAL", OracleDbType.Varchar2, ParameterDirection.Input, 1, checkListControlFundacion.CartaRefProfesional);
                Procedimiento.AddParametros("P_ACCOUNTING_RECORD_LOCATION", OracleDbType.Varchar2, ParameterDirection.Input, 255, checkListControlFundacion.AccountingRecordLocation);
                Procedimiento.AddParametros("P_EPI_NOMBRE_PRIMER_BEN", OracleDbType.Varchar2, ParameterDirection.Input, 1, checkListControlFundacion.EPINombrePrimerBen);
                Procedimiento.AddParametros("P_DIRECCION_PRIMER_BEN", OracleDbType.Varchar2, ParameterDirection.Input, 1, checkListControlFundacion.DireccionPrimerBen);

                Procedimientos.Add(Procedimiento);
                ret = DA.EjecutarProcedimientos(Procedimientos);

                return ret;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return false;
            }
        }

        [HttpGet]
        public CheckListControlFundacion ObtenerCheckListControl(string incidente, int solicitud)
        {
            try
            {
                CheckListControlFundacion checkListControl = null;
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_FUNDACION.USP_SEL_CHECK_LIST_CONTROL";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, solicitud.ToString());
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                DataTable dt = DA.GetDataTable(Procedimiento);

                if (dt.Rows.Count > 0)
                {
                    checkListControl = new CheckListControlFundacion()
                    {
                        Incidente = incidente,
                        IdDatosFundacion = Convert.ToInt32(dt.Rows[0]["ID_DATOS_FUNDACION"]),
                        CodigoFundacion = dt.Rows[0]["CODIGO_FUNDACION"].ToString(),
                        ActaFundacional = dt.Rows[0]["ACTA_FUNDACIONAL"].ToString(),
                        Reglamentos = dt.Rows[0]["REGLAMENTOS"].ToString(),
                        DesignacionProtector = dt.Rows[0]["DESIGNACION_PROTECTOR"].ToString(),
                        PoderGenEsp = dt.Rows[0]["PODER_GEN_ESP"].ToString(),
                        PINombrePrimerBen = dt.Rows[0]["PI_NOMBRE_PRIMER_BEN"].ToString(),
                        PruebaDomPrimerBen = dt.Rows[0]["PRUEBA_DOM_PRIMER_BEN"].ToString(),
                        CopiaPasIdPrimerBen = dt.Rows[0]["COPIA_PAS_ID_PRIMER_BEN"].ToString(),
                        CartaRefBancaria = dt.Rows[0]["CARTA_REF_BANCARIA"].ToString(),
                        CartaRefProfesional = dt.Rows[0]["CARTA_REF_PROFESIONAL"].ToString(),
                        AccountingRecordLocation = dt.Rows[0]["ACCOUNTING_RECORD_LOCATION"].ToString(),
                        EPINombrePrimerBen = dt.Rows[0]["EPI_NOMBRE_PRIMER_BEN"].ToString(),
                        DireccionPrimerBen = dt.Rows[0]["DIRECCION_PRIMER_BEN"].ToString(),
                        Solicitud = solicitud
                    };
                }

                return checkListControl;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }
        }
    }
}