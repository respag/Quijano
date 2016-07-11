using NLog;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Web.Configuration;
using System.Web.Http;
using ULA.Quijano.Model;
using ULA.Quijano.ProcesoLegal.Commons;
using ULA.Quijano.ProcesoLegal.Helpers;
using ULA.Quijano.ProcesoLegal.Json;
using ULA.Quijano.ProcesoLegal.UltimusIntegrationLayer;
using Ultimus.DataAccessLayer;
using Ultimus.Utilitarios;
using Ultimus.UtilityLayer;

namespace ULA.Quijano.ProcesoLegal.ControllersWebApi
{
    public class DatosSociedadController : ApiController
    {
        #region LOGGER DEFINITION

        UltimusLogs UltimusLogs = new UltimusLogs("SolicitudAPIMesaOperacion");

        #endregion

        [HttpGet]
        public DatosSolicitud ObtenerDatosSolicitud(string incidente, string etapa, int solicitud)
        {
            try
            {
                SolicitudAPIController s = new SolicitudAPIController();
                DatosFundacionController f = new DatosFundacionController();
                DatosNaveController n = new DatosNaveController();
                DatosMigracionController m = new DatosMigracionController();

                DatosSolicitud d = new DatosSolicitud();
                d.datosSociedad = ObtenerDatosSociedad(incidente, "",solicitud);
                d.datosFundacion = f.ObtenerDatosFundacion(incidente, "",solicitud);
                d.datosNave = n.ObtenerDatosNave(incidente, "",solicitud);
                d.datosMigracion = m.ObtenerDatosMigracion(incidente, "",solicitud);
                d.comunicacion = s.ObtenerComunicacionForServicioLegales(incidente, "");
                d.idiomas = s.GetIdioma();
                d.procedencias = s.GetProcedencia();
                d.areas = s.GetArea();
                d.jurisdicciones = s.GetJurisdiccion();
                d.abogados = d.comunicacion == null ? null : s.GetAbogado(d.comunicacion.CodIdioma);
                d.tramites = d.comunicacion == null ? null : s.GetTramite(d.comunicacion.CodArea);
                d.formasMigracion = m.GetFormasMigracion();
                d.InstruccionesEspecificas = ObtenerInstrucciones(incidente,solicitud);
                d.datosMarcas = ObtenerDatosMarcas(incidente,solicitud);

                return d;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [HttpPost]
        public bool GuardarDatosSolicitud(DatosSolicitud d)
        {
            try
            {
                if (d.datosSociedad != null)
                {
                    d.datosSociedad.Solicitud = d.Solicitud;
                    if (!GuardarDatosSociedad(d.datosSociedad))
                        return false;
                }

                if (d.datosFundacion != null)
                {
                    if (!GuardarDatosFundacion(d.datosFundacion))
                        return false;
                }

                if (d.datosNave != null)
                {
                    d.datosNave.Solicitud = d.Solicitud;
                    if (!GuardarDatosNave(d.datosNave))
                        return false;
                }

                if (d.datosMigracion != null)
                {
                    if (!GuardarDatosMigracion(d.datosMigracion))
                        return false;
                }

                if (d.comunicacion != null)
                {
                    if (!GuardarComunicacion(d.comunicacion))
                        return false;
                }

                if (d.datosMarcas != null)
                {
                    d.datosMarcas.Solicitud = d.Solicitud;
                    if (!GuardarDatosMarcas(d.datosMarcas))
                        return false;
                }

                GuardarInstrucciones(d.comunicacion.Incidente_Relacionado, d.InstruccionesEspecificas,d.Solicitud);

                return true;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return false;
            }
        }

        [HttpGet]
        public DatosSociedad ObtenerDatosSociedad(string incidente, string etapa,int solicitud)
        {
            try
            {
                DatosSociedad d = null;
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_SOCIEDAD.USP_SEL_DATOS_SOCIEDAD";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, solicitud.ToString());

                DataTable dt = DA.GetDataTable(Procedimiento);

                if(dt.Rows.Count>0)
                {
                    if (dt.Rows[0]["SOCIEDAD_EXISTENTE"].ToString() == "1" && !string.IsNullOrEmpty(dt.Rows[0]["COD_SOCIEDAD"].ToString()))
                        d = GetSociedadById(dt.Rows[0]["COD_SOCIEDAD"].ToString());

                    if (d != null)
                    {
                        d.Incidente = incidente;
                        //d.Instrucciones = dt.Rows[0]["INSTRUCCIONES"].ToString();
                        d.Honorarios = dt.Rows[0]["HONORARIOS"].ToString();
                        d.Gastos = dt.Rows[0]["GASTOS"].ToString();
                        d.Total = dt.Rows[0]["TOTAL"].ToString();
                    }
                    else
                    {
                        d = new DatosSociedad()
                        {
                            Incidente = incidente,
                            SociedadExistente = dt.Rows[0]["SOCIEDAD_EXISTENTE"].ToString(),
                            CodSociedad = dt.Rows[0]["COD_SOCIEDAD"].ToString(),
                            NombreSociedad = dt.Rows[0]["NOMBRE_SOCIEDAD"].ToString(),
                            IdTipoCorporacion = dt.Rows[0]["ID_TIPO_CORPORACION"].ToString(),
                            IdClienteAnterior = dt.Rows[0]["ID_CLIENTE_ANTERIOR"].ToString(),
                            NombreClienteAnterior = dt.Rows[0]["NOMBRE_CLIENTE_ANTERIOR"].ToString(),
                            //IdTipoCapital = dt.Rows[0]["ID_TIPO_CAPITAL"].ToString(),
                            MontoCapital = dt.Rows[0]["MONTO_CAPITAL"].ToString(),
                            NAcciones = dt.Rows[0]["N_ACCIONES"].ToString(),
                            ValorAccion = dt.Rows[0]["VALOR_ACCION"].ToString(),
                            IdTipoAcciones = dt.Rows[0]["ID_TIPO_ACCIONES"].ToString(),
                            //IdTipoDirector = dt.Rows[0]["ID_TIPO_DIRECTOR"].ToString(),
                            Cantidad = dt.Rows[0]["CANTIDAD"].ToString(),
                            //Instrucciones = dt.Rows[0]["INSTRUCCIONES"].ToString(),
                            IdRazones = dt.Rows[0]["ID_RAZONES"].ToString(),
                            IdPropositos = dt.Rows[0]["ID_PROPOSITOS"].ToString(),
                            DetalleProposito = dt.Rows[0]["DETALLE_PROPOSITO"].ToString(),
                            ProveeDirectores = dt.Rows[0]["PROVEE_DIRECTORES"].ToString(),
                            Tasa = dt.Rows[0]["TASA"].ToString(),
                            AgenteResid = dt.Rows[0]["AGENTE_RESID"].ToString(),
                            DirectoresQA = dt.Rows[0]["DIRECTORES_QA"].ToString(),
                            Honorarios = dt.Rows[0]["HONORARIOS"].ToString(),
                            Gastos = dt.Rows[0]["GASTOS"].ToString(),
                            Total = dt.Rows[0]["TOTAL"].ToString(),
                            Dignatarios = GetDignatarios(incidente,solicitud)
                        };
                    }
                }
                else
                {
                    d = new DatosSociedad();
                }

                //Consultar informacion ingresada en pestaña Datos Cliente
                SolicitudAPIController s = new SolicitudAPIController();
                DatosCliente c = s.ObtenerDatosCliente(incidente,solicitud);
                if(c != null)
                {
                    d.CodigoCliente = c.Cod == 0 ? "" : c.Cod.ToString();
                    d.NombreCliente = c.Nombre == null ? "" : c.Nombre;
                }

                d.TiposCorporacion = GetTiposCorporacion();
                //TiposCapital = dt.Rows[0][""].ToString(),
                d.TiposAcciones = GetTiposAcciones();
                //TiposDirector = dt.Rows[0][""].ToString(),
                //Razones = dt.Rows[0][""].ToString(),
                //Propositos = dt.Rows[0][""].ToString(),

                return d;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [HttpPost]
        public bool GuardarDatosSociedad(DatosSociedad d)
        {
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_SOCIEDAD.USP_UDP_DATOS_SOCIEDAD";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, d.Incidente);
                Procedimiento.AddParametros("P_SOCIEDAD_EXISTENTE", OracleDbType.Varchar2, ParameterDirection.Input, 1, d.SociedadExistente);
                Procedimiento.AddParametros("P_COD_SOCIEDAD", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.CodSociedad);
                Procedimiento.AddParametros("P_NOMBRE_SOCIEDAD", OracleDbType.Varchar2, ParameterDirection.Input, 200, d.NombreSociedad);
                Procedimiento.AddParametros("P_ID_TIPO_CORPORACION", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.IdTipoCorporacion);
                Procedimiento.AddParametros("P_ID_CLIENTE_ANTERIOR", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.IdClienteAnterior);
                Procedimiento.AddParametros("P_NOMBRE_CLIENTE_ANTERIOR", OracleDbType.Varchar2, ParameterDirection.Input, 200, d.NombreClienteAnterior);
                Procedimiento.AddParametros("P_ID_TIPO_CAPITAL", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.IdTipoCapital);
                Procedimiento.AddParametros("P_MONTO_CAPITAL", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.MontoCapital);
                Procedimiento.AddParametros("P_N_ACCIONES", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.NAcciones);
                Procedimiento.AddParametros("P_VALOR_ACCION", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.ValorAccion);
                Procedimiento.AddParametros("P_ID_TIPO_ACCIONES", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.IdTipoAcciones);
                Procedimiento.AddParametros("P_ID_TIPO_DIRECTOR", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.IdTipoDirector);
                Procedimiento.AddParametros("P_CANTIDAD", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.Cantidad);
                Procedimiento.AddParametros("P_INSTRUCCIONES", OracleDbType.Varchar2, ParameterDirection.Input, 20, "");
                Procedimiento.AddParametros("P_ID_RAZONES", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.IdRazones);
                Procedimiento.AddParametros("P_ID_PROPOSITOS", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.IdPropositos);
                Procedimiento.AddParametros("P_DETALLE_PROPOSITO", OracleDbType.Clob, ParameterDirection.Input, 1000, d.DetalleProposito);
                Procedimiento.AddParametros("P_PROVEE_DIRECTORES", OracleDbType.Varchar2, ParameterDirection.Input, 1, d.ProveeDirectores);
                Procedimiento.AddParametros("P_TASA", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.Tasa);
                Procedimiento.AddParametros("P_AGENTE_RESID", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.AgenteResid);
                Procedimiento.AddParametros("P_DIRECTORES_QA", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.DirectoresQA);
                Procedimiento.AddParametros("P_HONORARIOS", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.Honorarios);
                Procedimiento.AddParametros("P_GASTOS", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.Gastos);
                Procedimiento.AddParametros("P_TOTAL", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.Total);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, d.Solicitud.ToString());
                Procedimientos.Add(Procedimiento);

                foreach (Dignatario t in d.Dignatarios)
                {
                    Procedimiento = new OracleStoredName();
                    Procedimiento.Nombre = "PAQ_DATOS_SOCIEDAD.USP_INS_DIGNATARIO";
                    Procedimiento.IsPadre = Valores.IsNotPadre;
                    Procedimiento.IsHereda = false;
                    Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, d.Incidente);
                    Procedimiento.AddParametros("P_SECUENCIA", OracleDbType.Int32, ParameterDirection.Input, 1, t.Secuencia);
                    Procedimiento.AddParametros("P_NOMBRE", OracleDbType.Varchar2, ParameterDirection.Input, 200, t.Nombre);
                    Procedimiento.AddParametros("P_IDENTIFICACION", OracleDbType.Varchar2, ParameterDirection.Input, 20, t.Identificacion);
                    Procedimiento.AddParametros("P_TITULO", OracleDbType.Varchar2, ParameterDirection.Input, 200, t.Titulo);
                    Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input,255, d.Solicitud.ToString());
                    Procedimientos.Add(Procedimiento);
                }

                return DA.EjecutarProcedimientos(Procedimientos);
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return false;
            }
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [HttpPost]
        public bool GuardarDatosFundacion(DatosFundacion d)
        {
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_FUNDACION.USP_UDP_DATOS_FUNDACION";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, d.Incidente);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, d.Solicitud.ToString());
                Procedimiento.AddParametros("P_FUNDACION_EXISTENTE", OracleDbType.Varchar2, ParameterDirection.Input, 1, d.FundacionExistente);
                Procedimiento.AddParametros("P_COD_FUNDACION", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.CodFundacion);
                Procedimiento.AddParametros("P_NOMBRE_FUNDACION", OracleDbType.Varchar2, ParameterDirection.Input, 200, d.NombreFundacion);
                Procedimiento.AddParametros("P_TIPO_FUNDACION", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.TipoFundacion);
                Procedimiento.AddParametros("P_COD_CIA", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.CodCia);
                Procedimiento.AddParametros("P_DESCRIPCION_CIA", OracleDbType.Varchar2, ParameterDirection.Input, 200, d.DescripcionCia);
                Procedimiento.AddParametros("P_FUNDADOR", OracleDbType.Varchar2, ParameterDirection.Input, 200, d.Fundador);
                Procedimiento.AddParametros("P_COD_BAJO_LEYES", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.CodBajoLeyes);
                Procedimiento.AddParametros("P_DESCRIPCION_BAJO_LEYES", OracleDbType.Varchar2, ParameterDirection.Input, 200, d.DescripcionBajoLeyes);
                Procedimiento.AddParametros("P_IS_PENDIENTE_CAMBIO_CLT", OracleDbType.Varchar2, ParameterDirection.Input, 1, d.IsPendienteCambioClt ? "1" : "0");
                Procedimiento.AddParametros("P_PENDIENTE_CAMBIO_CLT", OracleDbType.Varchar2, ParameterDirection.Input, 200, d.PendienteCambioClt);
                Procedimiento.AddParametros("P_COD_CLIENTE", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.CodigoCliente);
                Procedimiento.AddParametros("P_NOMBRE_CLIENTE", OracleDbType.Varchar2, ParameterDirection.Input, 200, d.NombreCliente);
                Procedimiento.AddParametros("P_DURACION", OracleDbType.Varchar2, ParameterDirection.Input, 200, d.Duracion);
                Procedimiento.AddParametros("P_RESP_ANUALIDAD_COD", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.RespAnualidadCodigo);
                Procedimiento.AddParametros("P_RESP_ANUALIDAD_NOMBRE", OracleDbType.Varchar2, ParameterDirection.Input, 200, d.RespAnualidadCodigo);
                Procedimiento.AddParametros("P_NO_REG", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.NoReg);
                Procedimiento.AddParametros("P_NOMBRE_ANTERIOR", OracleDbType.Varchar2, ParameterDirection.Input, 200, d.NombreAnterior);
                Procedimiento.AddParametros("P_RUC", OracleDbType.Varchar2, ParameterDirection.Input, 200, d.RUC);
                Procedimiento.AddParametros("P_FECHA_INSCRIPCION", OracleDbType.Varchar2, ParameterDirection.Input, 50, d.FechaInscripcion.ToString("dd/MM/yyyy"));
                Procedimiento.AddParametros("P_IDIOMA", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.Idioma);
                Procedimiento.AddParametros("P_OFICINA", OracleDbType.Varchar2, ParameterDirection.Input, 200, d.Oficina);
                Procedimiento.AddParametros("P_PATRIMONIO", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.Patrimonio);
                Procedimiento.AddParametros("P_ESTATUS", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.Estatus);
                Procedimiento.AddParametros("P_PROPOSITO_FUNDACION", OracleDbType.Clob, ParameterDirection.Input, 10000, d.PropositoFundacion);
                Procedimiento.AddParametros("P_PROVEE_DIRECTORES", OracleDbType.Varchar2, ParameterDirection.Input, 1, d.ProveeDirectores);
                Procedimiento.AddParametros("P_TASA", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.Tasa);
                Procedimiento.AddParametros("P_AGENTE_RESID", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.AgenteResid);
                Procedimiento.AddParametros("P_DIRECTORES_QA", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.DirectoresQA);
                Procedimiento.AddParametros("P_HONORARIOS", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.Honorarios);
                Procedimiento.AddParametros("P_GASTOS", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.Gastos);
                Procedimiento.AddParametros("P_TOTAL", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.Total);
                Procedimientos.Add(Procedimiento);

                foreach (ConsejoFundacional c in d.Consejos)
                {
                    Procedimiento = new OracleStoredName();
                    Procedimiento.Nombre = "PAQ_DATOS_FUNDACION.USP_INS_CONSEJO_FUNDACIONAL";
                    Procedimiento.IsPadre = Valores.IsNotPadre;
                    Procedimiento.IsHereda = false;
                    Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, d.Incidente);
                    Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, d.Solicitud.ToString());
                    Procedimiento.AddParametros("P_SECUENCIA", OracleDbType.Int32, ParameterDirection.Input, 1, c.Secuencia);
                    Procedimiento.AddParametros("P_NOMBRE", OracleDbType.Varchar2, ParameterDirection.Input, 200, c.Nombre);
                    Procedimiento.AddParametros("P_IDENTIFICACION", OracleDbType.Varchar2, ParameterDirection.Input, 20, c.Identificacion);
                    Procedimiento.AddParametros("P_TITULO", OracleDbType.Varchar2, ParameterDirection.Input, 200, c.Titulo);
                    Procedimientos.Add(Procedimiento);
                }

                return DA.EjecutarProcedimientos(Procedimientos);
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return false;
            }
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [HttpPost]
        public bool GuardarDatosNave(DatosNave n)
        {
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_NAVE.USP_UDP_DATOS_NAVE";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, n.Incidente);
                Procedimiento.AddParametros("P_NAVE_EXISTENTE", OracleDbType.Varchar2, ParameterDirection.Input, 1, n.NaveExistente);
                Procedimiento.AddParametros("P_COD_NAVE", OracleDbType.Varchar2, ParameterDirection.Input, 20, n.CodNave);
                Procedimiento.AddParametros("P_NOMBRE_NAVE", OracleDbType.Varchar2, ParameterDirection.Input, 200, n.NombreNave);
                Procedimiento.AddParametros("P_COD_CIA", OracleDbType.Varchar2, ParameterDirection.Input, 20, n.CodCia);
                Procedimiento.AddParametros("P_NOMBRE_CIA", OracleDbType.Varchar2, ParameterDirection.Input, 200, n.NombreCia);
                Procedimiento.AddParametros("P_NOMBRE_ANTERIOR", OracleDbType.Varchar2, ParameterDirection.Input, 200, n.NombreAnterior);
                Procedimiento.AddParametros("P_SOC_PROPIETARIA", OracleDbType.Varchar2, ParameterDirection.Input, 200, n.SocPropietaria);
                Procedimiento.AddParametros("P_CORRESPONSAL", OracleDbType.Varchar2, ParameterDirection.Input, 200, n.Corresponsal);
                Procedimiento.AddParametros("P_CLIENTE_CORRESPONSAL", OracleDbType.Varchar2, ParameterDirection.Input, 200, n.ClienteCorresponsal);                
                Procedimiento.AddParametros("P_ESTADO", OracleDbType.Varchar2, ParameterDirection.Input, 1, n.Estado);
                Procedimiento.AddParametros("P_STS_CONSOLIDADO", OracleDbType.Varchar2, ParameterDirection.Input, 1, n.StsConsolidado);
                Procedimiento.AddParametros("P_PROP_NAVE", OracleDbType.Varchar2, ParameterDirection.Input, 200, n.PropNave);
                Procedimiento.AddParametros("P_SOMOS_RL", OracleDbType.Varchar2, ParameterDirection.Input, 1, n.SomosRL ? "1" : "0");
                Procedimiento.AddParametros("P_COBRAR_RL", OracleDbType.Varchar2, ParameterDirection.Input, 1, n.CobrarRL ? "1" : "0");
                Procedimiento.AddParametros("P_CAMBIO_RL", OracleDbType.Varchar2, ParameterDirection.Input, 1, n.CambioRL ? "1" : "0");
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, n.Solicitud.ToString());
                Procedimientos.Add(Procedimiento);

                return DA.EjecutarProcedimientos(Procedimientos);
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return false;
            }
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [HttpPost]
        public bool GuardarDatosMigracion(DatosMigracion m)
        {
            bool ret = false;
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_MIGRACION.USP_UDP_DATOS_MIGRACION";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, m.Incidente);
                Procedimiento.AddParametros("P_DEPOSITO_REPATRIACION", OracleDbType.Varchar2, ParameterDirection.Input, 20, m.DepositoRepatriacion.ToString());
                Procedimiento.AddParametros("P_CAMBIO_CAT_MIGRATORIA", OracleDbType.Varchar2, ParameterDirection.Input, 20, m.CambioCatMigratoria.ToString());
                Procedimiento.AddParametros("P_CARNE_TRAMITE", OracleDbType.Varchar2, ParameterDirection.Input, 20, m.CarneTramite.ToString());
                Procedimiento.AddParametros("P_VISA_MULTIPLE_ENT_SAL", OracleDbType.Varchar2, ParameterDirection.Input, 20, m.VisaMultipleEntSal.ToString());
                Procedimiento.AddParametros("P_REGISTRO", OracleDbType.Varchar2, ParameterDirection.Input, 20, m.Registro.ToString());
                Procedimiento.AddParametros("P_APLICA_DEPENDIENTES", OracleDbType.Varchar2, ParameterDirection.Input, 1, m.AplicaDependientes);
                Procedimiento.AddParametros("P_FECHA_PROG_REGISTRO", OracleDbType.Varchar2, ParameterDirection.Input, 50, Convert.ToDateTime(m.FechaProgRegistro).ToString("dd/MM/yyyy"));
                Procedimiento.AddParametros("P_FECHA_PROG_PRESENTACION", OracleDbType.Varchar2, ParameterDirection.Input, 50, Convert.ToDateTime(m.FechaProgPresentacion).ToString("dd/MM/yyyy"));
                Procedimiento.AddParametros("P_HONORARIOS", OracleDbType.Varchar2, ParameterDirection.Input, 20, m.Honorarios);
                Procedimiento.AddParametros("P_GASTOS", OracleDbType.Varchar2, ParameterDirection.Input, 20, m.Gastos);
                Procedimiento.AddParametros("P_TOTAL", OracleDbType.Varchar2, ParameterDirection.Input, 20, m.Total);
                Procedimientos.Add(Procedimiento);

                ret = DA.EjecutarProcedimientos(Procedimientos);

                if (ret && m.OpcionesCheques != null && m.OpcionesCheques.Count > 0)
                {
                    //Guardo las Opciones de Cheque
                    foreach (var oc in m.OpcionesCheques)
                    {
                        Procedimientos = new List<OracleStoredName>();
                        Procedimiento = new OracleStoredName();
                        Procedimiento.Nombre = "PAQ_DATOS_MIGRACION.USP_UDP_OPCION_CHEQUE";
                        Procedimiento.IsPadre = Valores.IsNotPadre;
                        Procedimiento.IsHereda = false;
                        Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, m.Incidente);
                        Procedimiento.AddParametros("P_ID_OPCION_CHEQUE", OracleDbType.Int32, ParameterDirection.Input, 20, oc.Id.ToString());
                        Procedimiento.AddParametros("P_NOMBRE", OracleDbType.Varchar2, ParameterDirection.Input, 255, oc.Nombre.ToString());
                        Procedimiento.AddParametros("P_VALOR", OracleDbType.Varchar2, ParameterDirection.Input, 20, oc.Valor.ToString());
                        Procedimientos.Add(Procedimiento);

                        ret = DA.EjecutarProcedimientos(Procedimientos);

                        if (!ret) break;
                    }
                }

                if (ret && m.Solicitantes != null && m.Solicitantes.Count > 0)
                {
                    //Elimino los que no existen en la lista
                    string conjuntoIdsDel = "," + string.Join(",", m.Solicitantes.Select(x => x.Id).ToArray()) + ",";
                    Procedimientos = new List<OracleStoredName>();
                    Procedimiento = new OracleStoredName();
                    Procedimiento.Nombre = "PAQ_DATOS_MIGRACION.USP_DEL_SOLICITANTE";
                    Procedimiento.IsPadre = Valores.IsNotPadre;
                    Procedimiento.IsHereda = false;
                    Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, m.Incidente);
                    Procedimiento.AddParametros("P_CONJUNTO_IDS", OracleDbType.Varchar2, ParameterDirection.Input, 255, conjuntoIdsDel);
                    Procedimientos.Add(Procedimiento);
                    ret = DA.EjecutarProcedimientos(Procedimientos);

                    if (ret)
                    {
                        //Guardo las Solicitantes
                        foreach (var s in m.Solicitantes)
                        {
                            Procedimientos = new List<OracleStoredName>();
                            Procedimiento = new OracleStoredName();
                            Procedimiento.Nombre = "PAQ_DATOS_MIGRACION.USP_UDP_SOLICITANTES";
                            Procedimiento.IsPadre = Valores.IsNotPadre;
                            Procedimiento.IsHereda = false;
                            Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, m.Incidente);
                            Procedimiento.AddParametros("P_ID_SOLICITANTE_MIGRACION", OracleDbType.Int32, ParameterDirection.Input, 20, s.Id.ToString());
                            Procedimiento.AddParametros("P_NOMBRE", OracleDbType.Varchar2, ParameterDirection.Input, 255, s.Nombre.ToString());
                            Procedimiento.AddParametros("P_IDENTIFICACION", OracleDbType.Varchar2, ParameterDirection.Input, 255, s.Identificacion.ToString());
                            Procedimiento.AddParametros("P_TIPO", OracleDbType.Varchar2, ParameterDirection.Input, 255, s.Tipo.ToString());
                            Procedimientos.Add(Procedimiento);

                            ret = DA.EjecutarProcedimientos(Procedimientos);

                            if (!ret) break;
                        }
                    }
                }

                if (ret && m.Requisitos != null && m.Requisitos.Count > 0)
                {
                    //Elimino los que no existen en la lista
                    string conjuntoIdsDel = "," + string.Join(",", m.Requisitos.Select(x => x.IdRequisitoMigracion).ToArray()) + ",";
                    Procedimientos = new List<OracleStoredName>();
                    Procedimiento = new OracleStoredName();
                    Procedimiento.Nombre = "PAQ_DATOS_MIGRACION.USP_DEL_REQUISITO";
                    Procedimiento.IsPadre = Valores.IsNotPadre;
                    Procedimiento.IsHereda = false;
                    Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, m.Incidente);
                    Procedimiento.AddParametros("P_CONJUNTO_IDS", OracleDbType.Varchar2, ParameterDirection.Input, 255, conjuntoIdsDel);
                    Procedimientos.Add(Procedimiento);

                    ret = DA.EjecutarProcedimientos(Procedimientos);

                    if (ret)
                    {
                        //Guardo los Requisitos
                        foreach (var r in m.Requisitos)
                        {
                            Procedimientos = new List<OracleStoredName>();
                            Procedimiento = new OracleStoredName();
                            Procedimiento.Nombre = "PAQ_DATOS_MIGRACION.USP_UDP_REQUISITOS";
                            Procedimiento.IsPadre = Valores.IsNotPadre;
                            Procedimiento.IsHereda = false;
                            Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, m.Incidente);
                            Procedimiento.AddParametros("P_ID_REQUISITO_MIGRACION", OracleDbType.Int32, ParameterDirection.Input, 20, r.IdRequisitoMigracion.ToString());
                            Procedimiento.AddParametros("P_TIPO", OracleDbType.Varchar2, ParameterDirection.Input, 200, r.Tipo.ToString());
                            Procedimiento.AddParametros("P_NOMBRE", OracleDbType.Varchar2, ParameterDirection.Input, 200, r.Nombre.ToString());
                            Procedimiento.AddParametros("P_NOTARIZADO", OracleDbType.Varchar2, ParameterDirection.Input, 1, r.Notarizado.ToString());
                            Procedimiento.AddParametros("P_TRADUCCION", OracleDbType.Varchar2, ParameterDirection.Input, 1, r.Traduccion.ToString());
                            Procedimiento.AddParametros("P_APOSTILLADO_LEGALIZADO", OracleDbType.Varchar2, ParameterDirection.Input, 1, r.Apostillado_Legalizado.ToString());
                            Procedimiento.AddParametros("P_INSTRUCCION", OracleDbType.Varchar2, ParameterDirection.Input, 200, r.Instruccion.ToString());
                            Procedimientos.Add(Procedimiento);
                            ret = DA.EjecutarProcedimientos(Procedimientos);

                            if (!ret) break;
                        }
                    }
                }                        
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                ret = false;
            }

            return ret;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [HttpPost]
        public bool GuardarComunicacion(Comunicacion d)
        {
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_SOCIEDAD.USP_UDP_COMUNICACION";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE_RELACIONADO", OracleDbType.Varchar2, ParameterDirection.Input, 100, d.Incidente_Relacionado.ToString());
                Procedimiento.AddParametros("P_CODIDIOMA", OracleDbType.Int32, ParameterDirection.Input, 255, d.CodIdioma.ToString());
                Procedimiento.AddParametros("P_CODPROCEDENCIA", OracleDbType.Int32, ParameterDirection.Input, 255, d.CodProcedencia.ToString());
                Procedimiento.AddParametros("P_CODAREA", OracleDbType.Int32, ParameterDirection.Input, 255, d.CodArea.ToString());
                Procedimiento.AddParametros("P_CODJURISDICCION", OracleDbType.Int32, ParameterDirection.Input, 255, d.CodJurisdiccion.ToString());
                Procedimiento.AddParametros("P_CODTRAMITE", OracleDbType.Int32, ParameterDirection.Input, 255, d.CodTramite.ToString());
                Procedimiento.AddParametros("P_COD_FORMAS_MIG", OracleDbType.Int32, ParameterDirection.Input, 255, d.CodFormasMig.ToString());
                Procedimiento.AddParametros("P_ABOGADO_ASIGNADO", OracleDbType.Varchar2, ParameterDirection.Input, 200, d.Abogado_Asignado.ToString());
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, d.Solicitud.ToString());
                Procedimientos.Add(Procedimiento);
                return DA.EjecutarProcedimientos(Procedimientos);
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.StackTrace);
                return false;
            }
        }

        [HttpGet]
        public DatosSociedad GetSociedadById(string id)
        {
            DatosSociedad d = null;
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query = "SELECT SOV06.*, CCV61.NOMBRE AS NOMBRE_RESPONSABLE FROM SOV06 LEFT OUTER JOIN CCV61 ON SOV06.RESPONSABLE = CCV61.CLIENTE WHERE CODSOCIEDAD='" + id + "'";

                DA.GetDataTable(query, out dt);

                if (dt.Rows.Count > 0)
                {
                    string CodCia = dt.Rows[0]["CODCIA"].ToString();
                    string IdCliente = dt.Rows[0]["CLIENTE"].ToString();

                    d = new DatosSociedad()
                    {
                        SociedadExistente = "1",
                        CodSociedad = dt.Rows[0]["CODSOCIEDAD"].ToString(),
                        NombreSociedad = dt.Rows[0]["NOMBRE"].ToString(),
                        IdTipoCorporacion = dt.Rows[0]["TIPOCORP"].ToString(),
                        IdClienteAnterior = dt.Rows[0]["RESPONSABLE"].ToString(),
                        NombreClienteAnterior = dt.Rows[0]["NOMBRE_RESPONSABLE"].ToString(),
                        //IdTipoCapital = dt.Rows[0]["ID_TIPO_CAPITAL"].ToString(),//NO ESTA
                        MontoCapital = dt.Rows[0]["CAPITALAUTORIZADO"].ToString(),
                        NAcciones = dt.Rows[0]["CANTACCIONES"].ToString(),
                        ValorAccion = dt.Rows[0]["VALORACCIONES"].ToString(),
                        IdTipoAcciones = dt.Rows[0]["TIPOACCIONES"].ToString(),
                        //IdTipoDirector = dt.Rows[0]["ID_TIPO_DIRECTOR"].ToString(),
                        Cantidad = dt.Rows[0]["CANTDIRECTORES"].ToString(),
                        //Instrucciones = dt.Rows[0]["INSTRUCCIONES"].ToString(),//NO ESTA
                        IdRazones = dt.Rows[0]["DESCRIPCION"].ToString(),
                        IdPropositos = dt.Rows[0]["OTROS_PROPOSITO"].ToString(),
                        DetalleProposito = dt.Rows[0]["PROPOSITO"].ToString(),
                        ProveeDirectores = dt.Rows[0]["JDIRECTIVA"].ToString() == "S" ? "1" : "0",
                    };


                    //DIGNATARIOS
                    query = "SELECT * FROM SOV04 WHERE CODSOCIEDAD=" + id + " AND CODCIA=" + CodCia + " ORDER BY SECUENCIA";
                    DA.GetDataTable(query, out dt);
                    d.Dignatarios = dt.Rows.Cast<DataRow>().Select(r => new Dignatario()
                    {
                        Secuencia = r["SECUENCIA"].ToString(),
                        Nombre = r["NOMBRE"].ToString(),
                        Identificacion = r["CEDULA"].ToString(),
                        Titulo = r["TITULO"].ToString()
                    }).ToList();


                    //TARIFAS
                    query = "SELECT * FROM SOV14 WHERE CODSOCIEDAD=" + id + " AND CODCIA=" + CodCia + " AND CLIENTE=" + IdCliente;
                    DA.GetDataTable(query, out dt);

                    if (dt.Rows.Cast<DataRow>().Count(r => r["DESCRIPCION"].ToString() == "TASA UNICA ANUAL") > 0)
                        d.Tasa = dt.Rows.Cast<DataRow>().First(r => r["DESCRIPCION"].ToString() == "TASA UNICA ANUAL")["MONTO_GASTO"].ToString();
                    else
                        d.Tasa = "0";

                    if (dt.Rows.Cast<DataRow>().Count(r => r["DESCRIPCION"].ToString() == "AGENTE RESIDENTE") > 0)
                        d.AgenteResid = dt.Rows.Cast<DataRow>().First(r => r["DESCRIPCION"].ToString() == "AGENTE RESIDENTE")["MONTO_GASTO"].ToString();
                    else
                        d.AgenteResid = "0";

                    if (dt.Rows.Cast<DataRow>().Count(r => r["DESCRIPCION"].ToString() == "DIRECTORES Y DIGNATARIOS") > 0)
                        d.DirectoresQA = dt.Rows.Cast<DataRow>().First(r => r["DESCRIPCION"].ToString() == "DIRECTORES Y DIGNATARIOS")["MONTO_GASTO"].ToString();
                    else
                        d.DirectoresQA = "0";


                    //ANALISIS ANTIGUEDAD
                    query = "SELECT * FROM CCV166SOC WHERE CODSOCIEDAD='" + id + "' AND CODCIA=" + CodCia;
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
                        tramites = new AnalisisAntiguedad() { Secuencia = "1", Descripcion = "Trámites" };

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
                        anualidades = new AnalisisAntiguedad() { Secuencia = "2", Descripcion = "Anualidades" };

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

                    d.AnalisisAntiguedad = new List<AnalisisAntiguedad>();
                    d.AnalisisAntiguedad.Add(tramites);
                    d.AnalisisAntiguedad.Add(anualidades);
                    d.AnalisisAntiguedad.Add(totales);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return d;
        }

        [HttpGet]
        public DatosSociedad GetSociedadByNombre(string nombre)
        {
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query = "SELECT CODSOCIEDAD FROM SOV06 WHERE NOMBRE LIKE '%" + nombre.ToUpper() + "%'";
                DA.GetDataTable(query, out dt);

                if (dt.Rows.Count > 0)
                    return GetSociedadById(dt.Rows[0]["CODSOCIEDAD"].ToString());
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
        public List<KeyValuePair<string, string>> GetTiposCorporacion()
        {
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query = "SELECT * FROM SOV237 WHERE PARTICULARIDAD='SO'";
                DA.GetDataTable(query, out dt);

                return dt.Rows.Cast<DataRow>().Select(r => new KeyValuePair<string, string>(r["CODIGO"].ToString(), r["DESCRIPCION"].ToString())).ToList();
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }
        }

        [HttpGet]
        public List<KeyValuePair<string, string>> GetTiposAcciones()
        {
            DataTable dt = new DataTable();

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_SOCIEDAD.USP_SEL_TIPO_ACCIONES";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                return dt.Rows.Cast<DataRow>().Select(r => new KeyValuePair<string, string>(r["CODIGO"].ToString(), r["DESCRIPCION"].ToString())).ToList();
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }
        }

        public List<Dignatario> GetDignatarios(string incidente, int solicitud)
        {
            DataTable dt = new DataTable();

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_SOCIEDAD.USP_SEL_DIGNATARIO";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255,solicitud.ToString());

                dt = DA.GetDataTable(Procedimiento);
                return dt.Rows.Cast<DataRow>().Select(r => new Dignatario()
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
        public DatosCorporateIncorporate ObtenerCorporateIncorporate(string incidente, int solicitud)
        {
            try
            {
                DatosCorporateIncorporate datosCorporateIncorporate = null;
                DatosSociedad d = null;
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_SOCIEDAD.USP_SEL_DATOS_SOCIEDAD";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, solicitud.ToString());

                DataTable dt = DA.GetDataTable(Procedimiento);

                if (dt.Rows.Count > 0)
                {
                    if (!String.IsNullOrEmpty(dt.Rows[0]["COD_SOCIEDAD"].ToString()))
                    {
                        datosCorporateIncorporate = ObtenerCorporateIncorporateByCodigoSociedad(dt.Rows[0]["COD_SOCIEDAD"].ToString());
                        datosCorporateIncorporate.CodigoSociedad = dt.Rows[0]["COD_SOCIEDAD"].ToString();
                        datosCorporateIncorporate.Incidente = incidente;
                    }
                }

                return datosCorporateIncorporate;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }

        }

        [HttpGet]
        public DatosCorporateIncorporate ObtenerCorporateIncorporateByCodigoSociedad(string CodigoSociedad)
        {
            DatosCorporateIncorporate datosCorporateIncorporate = new DatosCorporateIncorporate();
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query =  "SELECT  SOV06.MAA, " +
                                        "SOV06.APP_DIRECTORS, " +
                                        "SOV06.COI, " +
                                        "SOV06.CORPORATE_SEAL, " +
                                        "SOV06.LETTER_CONSENT, " +
                                        "SOV06.RESOLUTION_SHARES, " +
                                        "SOV06.CERTIFICATE, " +
                                        "SOV06.REGISTER_DIRECTOR, " +
                                        "SOV06.REGISTER_M, " +
                                        "SOV06.ACCOUNTING_RECORD, " +
                                        "SOV06.NEW_DIRECTORS, " +
                                        "SOV06.NEW_OFFICER, " +
                                        "SOV06.NEW_MEMBERS, " +
                                        "SOV06.LETTER_RESIGNATION, " +
                                        "SOV06.NEW_SHAR_CERT, " +
                                        "SOV06.NEW_DIRECTOR_CONSENT, " +
                                        "SOV06.POWER_ATTORNEY, " +
                                        "SOV06.INSTRUMENT, " +
                                        "SOV06.TRUST, " +
                                        "SOV06.COMPANY_APLICATION_FORM, " +
                                        "SOV06.RECORDS_LOCATION " +
                                "FROM SOV06 " +
                                "WHERE SOV06.CODSOCIEDAD = '" + CodigoSociedad + "'";

                DA.GetDataTable(query, out dt);

                foreach (DataRow rowProducto in dt.Rows)
                {
                    datosCorporateIncorporate.MandAA = rowProducto["MAA"].ToString().Equals("S");
                    datosCorporateIncorporate.AppointmentFirstDirectors = rowProducto["APP_DIRECTORS"].ToString().Equals("S");
                    datosCorporateIncorporate.COI = rowProducto["COI"].ToString().Equals("S");
                    datosCorporateIncorporate.SealImpression = rowProducto["CORPORATE_SEAL"].ToString().Equals("S");
                    datosCorporateIncorporate.LetterImpression = rowProducto["LETTER_CONSENT"].ToString().Equals("S");
                    datosCorporateIncorporate.ResolutionShares = rowProducto["RESOLUTION_SHARES"].ToString().Equals("S");
                    datosCorporateIncorporate.ShareCertificate = rowProducto["CERTIFICATE"].ToString().Equals("S");
                    datosCorporateIncorporate.RegisterDirectors = rowProducto["REGISTER_DIRECTOR"].ToString().Equals("S");
                    datosCorporateIncorporate.RegisterMembers = rowProducto["REGISTER_M"].ToString().Equals("S");
                    datosCorporateIncorporate.AccountingRecordSolution = rowProducto["ACCOUNTING_RECORD"].ToString().Equals("S");
                    datosCorporateIncorporate.CompanyApplicationForm = rowProducto["COMPANY_APLICATION_FORM"].ToString().Equals("S");
                    datosCorporateIncorporate.ResolutionNewDirectors = rowProducto["NEW_DIRECTORS"].ToString().Equals("S");
                    datosCorporateIncorporate.ResolutionNewOfficer = rowProducto["NEW_OFFICER"].ToString().Equals("S");
                    datosCorporateIncorporate.ResolutionNewMembers = rowProducto["NEW_MEMBERS"].ToString().Equals("S");
                    datosCorporateIncorporate.LetterResignation = rowProducto["LETTER_RESIGNATION"].ToString().Equals("S");
                    datosCorporateIncorporate.NewShareCertificate = rowProducto["NEW_SHAR_CERT"].ToString().Equals("S");
                    datosCorporateIncorporate.NewDirectorConsent = rowProducto["NEW_DIRECTOR_CONSENT"].ToString().Equals("S");
                    datosCorporateIncorporate.PowerAttorney = rowProducto["POWER_ATTORNEY"].ToString().Equals("S");
                    datosCorporateIncorporate.InstrumentTransfer = rowProducto["INSTRUMENT"].ToString().Equals("S");
                    datosCorporateIncorporate.PTrustAgreement = rowProducto["TRUST"].ToString().Equals("S");
                    datosCorporateIncorporate.AccountingRecordLocation = rowProducto["RECORDS_LOCATION"].ToString();
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return datosCorporateIncorporate;
        }

        [HttpGet]
        public List<RazonEstado> GetRazones()
        {
            List<RazonEstado> lista = new List<RazonEstado>();
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query = "SELECT SOV66.CODIGO, SOV66.DESCRIPCION, SOV66.STATUS " +
                                "FROM SOV66 " +
                                "ORDER BY SOV66.DESCRIPCION";

                DA.GetDataTable(query, out dt);

                foreach (DataRow rowProducto in dt.Rows)
                {
                    RazonEstado razon = new RazonEstado();
                    razon.Codigo = Convert.ToInt32(rowProducto["CODIGO"]);
                    razon.Descripcion = rowProducto["DESCRIPCION"].ToString();
                    lista.Add(razon);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
        public List<Proposito> GetPropositos()
        {
            List<Proposito> lista = new List<Proposito>();
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                string query = "SELECT CATALOGO_PROPOSITO.CODPROPOSITO, CATALOGO_PROPOSITO.DESCRIPCION " +
                                "FROM CATALOGO_PROPOSITO " +
                                "ORDER BY CATALOGO_PROPOSITO.DESCRIPCION";

                DA.GetDataTable(query, out dt);

                foreach (DataRow rowProducto in dt.Rows)
                {
                    Proposito proposito = new Proposito();
                    proposito.Codigo = Convert.ToInt32(rowProducto["CODPROPOSITO"]);
                    proposito.Descripcion = rowProducto["DESCRIPCION"].ToString();
                    lista.Add(proposito);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
        public List<DatosSociedad> BuscarSociedadesById(string id)
        {
            List<DatosSociedad> lista = new List<DatosSociedad>();
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query = "SELECT SOV06.CODSOCIEDAD, SOV06.NOMBRE " +
                                 "FROM SOV06 " +
                                "WHERE CODSOCIEDAD LIKE '%" + id + "%' " +
                                " AND FUNDACION = 'N' " +
                                "ORDER BY SOV06.NOMBRE";

                DA.GetDataTable(query, out dt);

                foreach (DataRow rowProducto in dt.Rows)
                {
                    DatosSociedad sociedad = new DatosSociedad();
                    sociedad.CodSociedad = rowProducto["CODSOCIEDAD"].ToString();
                    sociedad.NombreSociedad = rowProducto["NOMBRE"].ToString();
                    lista.Add(sociedad);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
        public List<DatosSociedad> BuscarSociedadesByName(string nombre)
        {
            List<DatosSociedad> lista = new List<DatosSociedad>();
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query = "SELECT SOV06.CODSOCIEDAD, SOV06.NOMBRE " +
                                 "FROM SOV06 " +
                                "WHERE NOMBRE LIKE '%" + nombre.ToUpper() + "%' " +
                                " AND FUNDACION = 'N' " +
                                "ORDER BY SOV06.NOMBRE";

                DA.GetDataTable(query, out dt);

                foreach (DataRow rowProducto in dt.Rows)
                {
                    DatosSociedad sociedad = new DatosSociedad();
                    sociedad.CodSociedad = rowProducto["CODSOCIEDAD"].ToString();
                    sociedad.NombreSociedad = rowProducto["NOMBRE"].ToString();
                    lista.Add(sociedad);
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
        public bool GuardarInstrucciones(string incidente, string instrucciones, int solicitud)
        {
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_SOCIEDAD.USP_UDP_INSTRUCCIONES";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                Procedimiento.AddParametros("P_INSTRUCCIONES_ESPECIFICAS", OracleDbType.Varchar2, ParameterDirection.Input, 20, instrucciones);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, solicitud.ToString());

                Procedimientos.Add(Procedimiento);
                return DA.EjecutarProcedimientos(Procedimientos);
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.StackTrace);
                return false;
            }
        }

        [HttpGet]
        public string ObtenerInstrucciones(string incidente, int solicitud)
        {
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_SOCIEDAD.USP_SEL_INSTRUCCIONES";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, solicitud.ToString());

                DataTable dt = DA.GetDataTable(Procedimiento);
                if (dt.Rows.Count > 0)
                    return dt.Rows[0]["INSTRUCCIONES_ESPECIFICAS"].ToString();
                else
                    return "";
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }
        }

        public bool GuardarDatosMarcas(DatosMarcas d)
        {
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_MARCA.USP_UDP_DATOS_MARCAS";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, d.Incidente);
                Procedimiento.AddParametros("P_TIPO_SOLICITUD_MARCA", OracleDbType.Int32, ParameterDirection.Input, 255, d.TIPO_SOLICITUD_MARCA.ToString());
                Procedimiento.AddParametros("P_TIPO_REGISTRO", OracleDbType.Int32, ParameterDirection.Input, 255, d.TIPO_REGISTRO.ToString());
                Procedimiento.AddParametros("P_PODER", OracleDbType.Varchar2, ParameterDirection.Input, 1, d.PODER);
                Procedimiento.AddParametros("P_CHEQUE", OracleDbType.Varchar2, ParameterDirection.Input, 1, d.CHEQUE);
                Procedimiento.AddParametros("P_ETIQUETAS", OracleDbType.Varchar2, ParameterDirection.Input, 1, d.ETIQUETAS);
                Procedimiento.AddParametros("P_DECLARACION", OracleDbType.Varchar2, ParameterDirection.Input, 1, d.DECLARACION);
                Procedimiento.AddParametros("P_FORMULARIO", OracleDbType.Varchar2, ParameterDirection.Input, 1, d.FORMULARIO);
                Procedimiento.AddParametros("P_OTROS", OracleDbType.Varchar2, ParameterDirection.Input, 1, d.OTROS);
                Procedimiento.AddParametros("P_ANEXOS", OracleDbType.Varchar2, ParameterDirection.Input, 1, d.ANEXOS);
                Procedimiento.AddParametros("P_PETICION", OracleDbType.Varchar2, ParameterDirection.Input, 1, d.PETICION);
                Procedimiento.AddParametros("P_NUMERO_REGISTRO", OracleDbType.Varchar2, ParameterDirection.Input, 20, d.NUMERO_REGISTRO);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, d.Solicitud.ToString());

                Procedimientos.Add(Procedimiento);
                return DA.EjecutarProcedimientos(Procedimientos);
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.StackTrace);
                return false;
            }
        }

        public DatosMarcas ObtenerDatosMarcas(string incidente, int solicitud)
        {
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_MARCA.USP_SEL_DATOS_MARCAS";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, solicitud.ToString());

                DataTable dt = DA.GetDataTable(Procedimiento);
                if (dt.Rows.Count > 0)
                    return new DatosMarcas() {
                        TIPO_SOLICITUD_MARCA = Convert.IsDBNull(dt.Rows[0]["TIPO_SOLICITUD_MARCA"]) ? 0 : (int)((decimal) dt.Rows[0]["TIPO_SOLICITUD_MARCA"]),
                        TIPO_REGISTRO = Convert.IsDBNull(dt.Rows[0]["TIPO_REGISTRO"]) ? 0 : (int)((decimal) dt.Rows[0]["TIPO_REGISTRO"]),
                        PODER = dt.Rows[0]["PODER"].ToString(),
                        CHEQUE = dt.Rows[0]["CHEQUE"].ToString(),
                        ETIQUETAS = dt.Rows[0]["ETIQUETAS"].ToString(),
                        DECLARACION = dt.Rows[0]["DECLARACION"].ToString(),
                        FORMULARIO = dt.Rows[0]["FORMULARIO"].ToString(),
                        OTROS = dt.Rows[0]["OTROS"].ToString(),
                        ANEXOS = dt.Rows[0]["ANEXOS"].ToString(),
                        PETICION = dt.Rows[0]["PETICION"].ToString(),
                        NUMERO_REGISTRO = dt.Rows[0]["NUMERO_REGISTRO"].ToString()
                    };
                else
                    return null;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [HttpPost]
        public bool GuardarDatosEtapaControl(string incidente, int solicitud, string comentario)
        {
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_SOCIEDAD.USP_UPD_COMENTARIO_CONTROL";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, solicitud.ToString());
                Procedimiento.AddParametros("P_COMENTARIO", OracleDbType.Varchar2, ParameterDirection.Input, 1000, comentario);

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