using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using ULA.Quijano.Model;
using ULA.Quijano.ProcesoLegal.Commons;
using Ultimus.DataAccessLayer;

namespace ULA.Quijano.ProcesoLegal.ControllersWebApi
{
    public class DatosMigracionController : ApiController
    {
        #region LOGGER DEFINITION

        UltimusLogs UltimusLogs = new UltimusLogs("DatosMigracionController");

        #endregion

        [HttpGet]
        public List<CatalogoFormasMigracion> GetFormasMigracion()
        {
            List<CatalogoFormasMigracion> lista = new List<CatalogoFormasMigracion>();
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                string query = "SELECT COD_FORMAS_MIG, DESCRIPCION FROM CATALOGO_FORMAS_MIGRACION";

                DA.GetDataTable(query, out dt);

                foreach (DataRow rowProducto in dt.Rows)
                {
                    CatalogoFormasMigracion formasMigracion = new CatalogoFormasMigracion();
                    formasMigracion.CodFormasMigracion = Convert.ToInt32(rowProducto["COD_FORMAS_MIG"]);
                    formasMigracion.Descripcion = rowProducto["DESCRIPCION"].ToString();
                    lista.Add(formasMigracion);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
        public Cheque ObtenerChequeByFormasMigracion(int id)
        {
            Cheque cheque = null;
            DataTable dt = new DataTable();            

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_MIGRACION.USP_SEL_CHEQUE_FORMA_MIG";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_COD_FORMA_MIG", OracleDbType.Int32, ParameterDirection.Input, 100, id.ToString());
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                foreach (DataRow row in dt.Rows)
                {
                    cheque = new Cheque();
                    cheque.DepositoRepatriacion = Convert.ToDecimal(row["DEPOSITO_REPATRIACION"]);
                    cheque.CambioCatMigratoria = Convert.ToDecimal(row["CAMBIO_CAT_MIG"]);
                    cheque.CarneTramite = Convert.ToDecimal(row["CARNE_TRAMITE"]);
                    cheque.VisaMultipleEntSal = Convert.ToDecimal(row["VISA_MULTIPLE_ES"]);
                    cheque.Registro = Convert.ToDecimal(row["REGISTRO"]);
                    break;
                }                                
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return cheque;
        }

        [HttpGet]
        public List<TipoRequisito> ObtenerTipoByFormasMigracion(int id)
        {
            List<TipoRequisito> lista = new List<TipoRequisito>();
            DataTable dt = new DataTable();

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_MIGRACION.USP_SEL_TIPOS_FORMA_MIG";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_COD_FORMA_MIG", OracleDbType.Int32, ParameterDirection.Input, 100, id.ToString());
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                foreach (DataRow row in dt.Rows)
                {
                    TipoRequisito tipo = new TipoRequisito();
                    tipo.Id = lista.Count;
                    tipo.Descripcion = row["TIPO"].ToString();
                    lista.Add(tipo);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }

            return lista;
        }

        [HttpGet]
        public List<RequisitoMigracion> ObtenerRequisitosByFormasMigracion(int id)
        {
            List<RequisitoMigracion> lista = new List<RequisitoMigracion>();
            DataTable dt = new DataTable();

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_MIGRACION.USP_SEL_REQUISITOS_FORMA_MIG";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_COD_FORMA_MIG", OracleDbType.Int32, ParameterDirection.Input, 100, id.ToString());
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                foreach (DataRow row in dt.Rows)
                {
                    RequisitoMigracion requisito = new RequisitoMigracion();
                    requisito.IdRequisitoMigracion = 0;
                    requisito.Tipo = row["TIPO"].ToString();
                    requisito.Nombre = row["NOMBRE"].ToString();
                    requisito.Notarizado = row["NOTARIZADO"].ToString();
                    requisito.Traduccion = row["TRADUCCION"].ToString();
                    requisito.Instruccion = "";
                    requisito.Apostillado_Legalizado = row["APOSTILLADO_LEGALIZADO"].ToString();
                    requisito.CodFormasMig = Convert.ToInt32(row["COD_FORMAS_MIG"]);
                    requisito.IdDatosMigracion = 0;
                    lista.Add(requisito);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }

            return lista;
        }

        [HttpGet]
        public DatosMigracion ObtenerDatosMigracion(string incidente, string etapa,int solicitud)
        {
            try
            {
                DatosMigracion m = null;
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_MIGRACION.USP_SEL_DATOS_MIGRACION";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                DataTable dt = DA.GetDataTable(Procedimiento);

                if (dt.Rows.Count > 0)
                {
                    m = new DatosMigracion();
                    m.DepositoRepatriacion = Convert.ToDecimal(dt.Rows[0]["DEPOSITO_REPATRIACION"]);
                    m.CambioCatMigratoria = Convert.ToDecimal(dt.Rows[0]["CAMBIO_CAT_MIGRATORIA"]);
                    m.CarneTramite = Convert.ToDecimal(dt.Rows[0]["CARNE_TRAMITE"]);
                    m.VisaMultipleEntSal = Convert.ToDecimal(dt.Rows[0]["VISA_MULTIPLE_ENT_SAL"]);
                    m.Registro = Convert.ToDecimal(dt.Rows[0]["REGISTRO"]);
                    m.AplicaDependientes = dt.Rows[0]["APLICA_DEPENDIENTES"].ToString();
                    m.FechaProgRegistro = String.Format("{0:MM/dd/yyyy}", DateTime.ParseExact(dt.Rows[0]["FECHA_PROG_REGISTRO"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    m.FechaProgPresentacion = String.Format("{0:MM/dd/yyyy}", DateTime.ParseExact(dt.Rows[0]["FECHA_PROG_PRESENTACION"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    m.Honorarios = dt.Rows[0]["HONORARIOS"].ToString();
                    m.Gastos = dt.Rows[0]["GASTOS"].ToString();
                    m.Total = dt.Rows[0]["TOTAL"].ToString();
                    m.OpcionesCheques = GetOpcionesChequesByIncidente(incidente);
                    m.Solicitantes = GetSolicitantesByIncidente(incidente);
                    m.Requisitos = GetRequisitosByIncidente(incidente);
                }
                else
                {
                    m = new DatosMigracion();
                }

                //Consultar informacion ingresada en pestaña Datos Cliente
                SolicitudAPIController s = new SolicitudAPIController();
                DatosCliente c = s.ObtenerDatosCliente(incidente,solicitud);
                if (c != null)
                {
                    m.CodigoCliente = c.Cod == 0 ? "" : c.Cod.ToString();
                    m.NombreCliente = c.Nombre == null ? "" : c.Nombre;
                }

                return m;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }
        }

        public List<OpcionCheque> GetOpcionesChequesByIncidente(string incidente)
        {
            try
            {
                List<OpcionCheque> list = null;
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_MIGRACION.USP_SEL_OPCION_CHEQUE";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                DataTable dt = DA.GetDataTable(Procedimiento);

                if (dt.Rows.Count > 0)
                {
                    list = new List<OpcionCheque>();
                    foreach (DataRow row in dt.Rows)
                    {
                        OpcionCheque oc = new OpcionCheque();
                        oc.Id = Convert.ToInt32(row["ID_OPCION_CHEQUE"]);
                        oc.Nombre = row["NOMBRE"].ToString();
                        oc.Valor = Convert.ToDecimal(row["VALOR"]);
                        list.Add(oc);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }            
        }

        public List<Solicitante> GetSolicitantesByIncidente(string incidente)
        {
            try
            {
                List<Solicitante> list = null;
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_MIGRACION.USP_SEL_SOLICITANTE";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                DataTable dt = DA.GetDataTable(Procedimiento);

                if (dt.Rows.Count > 0)
                {
                    list = new List<Solicitante>();
                    foreach (DataRow row in dt.Rows)
                    {
                        Solicitante sol = new Solicitante();
                        sol.Id = Convert.ToInt32(row["ID_SOLICITANTE_MIGRACION"]);
                        sol.Nombre = row["NOMBRE"].ToString();
                        sol.Identificacion = row["IDENTIFICACION"].ToString();
                        sol.Tipo = row["TIPO"].ToString();
                        sol.IdDatosMigracion = Convert.ToInt32(row["ID_DATOS_MIGRACION"]);
                        list.Add(sol);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }
        }

        public List<RequisitoMigracion> GetRequisitosByIncidente(string incidente)
        {
            try
            {
                List<RequisitoMigracion> list = null;
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_MIGRACION.USP_SEL_REQUISITO";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                DataTable dt = DA.GetDataTable(Procedimiento);

                if (dt.Rows.Count > 0)
                {
                    list = new List<RequisitoMigracion>();
                    foreach (DataRow row in dt.Rows)
                    {
                        RequisitoMigracion req = new RequisitoMigracion();
                        req.IdRequisitoMigracion = Convert.ToInt32(row["ID_REQUISITO_MIGRACION"]);
                        req.Tipo = row["TIPO"].ToString();
                        req.Nombre = row["NOMBRE"].ToString();
                        req.Instruccion = row["INSTRUCCION"].ToString();
                        req.Notarizado = row["NOTARIZADO"].ToString();
                        req.Traduccion = row["TRADUCCION"].ToString();
                        req.Apostillado_Legalizado = row["APOSTILLADO_LEGALIZADO"].ToString();
                        req.IdDatosMigracion = Convert.ToInt32(row["ID_DATOS_MIGRACION"]);
                        list.Add(req);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }
        }
    }
}