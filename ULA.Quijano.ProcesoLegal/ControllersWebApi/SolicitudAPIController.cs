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
using ULA.Quijano.Model.ModeloDB;
using Ultimus.DataAccessLayer;
using System.Web.Configuration;
using Oracle.DataAccess.Client;
using System.Data;
using Ultimus.UtilityLayer;
using Ultimus.Utilitarios;
using ULA.Quijano.ProcesoLegal.UltimusIntegrationLayer;
using System.Web;
using ULA.Quijano.ProcesoLegal.AttachmentService;


namespace ULA.Quijano.ProcesoLegal.ControllersWebApi
{
    public class SolicitudAPIController : ApiController
    {

        #region LOGGER DEFINITION

        UltimusLogs UltimusLogs = new UltimusLogs("SolicitudAPIController");
        MesaOperacionController mesaOperacionController = new MesaOperacionController();
        
        #endregion

        #region PUBLIC

        [HttpGet]
        public List<CatalogoIdioma> GetIdioma()
        {
            List<CatalogoIdioma> lista = new List<CatalogoIdioma>();
            DataTable dt = new DataTable();

            try
            {

                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_COMUNICACION.USP_SEL_IDIOMA";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                foreach (DataRow rowProducto in dt.Rows)
                {
                    CatalogoIdioma idi = new CatalogoIdioma();
                    idi.CodIdioma = Convert.ToInt32(rowProducto["CODIDIOMA"]);
                    idi.Descripcion = rowProducto["DESCRIPCION"].ToString();
                    lista.Add(idi);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
        public List<Idioma> GetIdiomaByCiente(int cli)
        {
            List<Idioma> lista = new List<Idioma>();
            DataTable dt = null;
            string nombreIdioma = "";
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query = "SELECT IDIOMA_FACTURACION FROM CCV90 WHERE CLIENTE=" + cli;

                DA.GetDataTable(query, out dt);

                foreach (DataRow rowProducto in dt.Rows)
                {
                    Idioma idioma = new Idioma();
                    idioma.CodIdioma = Convert.ToInt32(rowProducto["IDIOMA_FACTURACION"]);
                    if (idioma.CodIdioma == 1)
                        nombreIdioma = "ESPAÑOL";
                    else if (idioma.CodIdioma == 2)
                        nombreIdioma = "INGLES";
                    else if (idioma.CodIdioma == 3)
                        nombreIdioma = "PORTUGUES";
                    else if (idioma.CodIdioma == 4)
                        nombreIdioma = "FRANCES";
                    else if (idioma.CodIdioma == 5)
                        nombreIdioma = "HOLANDES";
                    else if (idioma.CodIdioma == 6)
                        nombreIdioma = "DANES";
                    else if (idioma.CodIdioma == 7)
                        nombreIdioma = "ALEMAN";
                    else if (idioma.CodIdioma == 8)
                        nombreIdioma = "ITALIANO";

                    idioma.NombreIdioma = nombreIdioma;
                    lista.Add(idioma);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
        public List<CatalogoProcedencia> GetProcedencia()
        {            
            List<CatalogoProcedencia> lista = new List<CatalogoProcedencia>();
            DataTable dt = new DataTable();

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_COMUNICACION.USP_SEL_PROCEDENCIA";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                foreach (DataRow rowProducto in dt.Rows)
                {
                    CatalogoProcedencia pro = new CatalogoProcedencia();
                    pro.CodProcedencia = Convert.ToInt32(rowProducto["CODPROCEDENCIA"]);
                    pro.Descripcion = rowProducto["DESCRIPCION"].ToString();
                    lista.Add(pro);
                }
            }
            catch(Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
        public List<CatalogoArea> GetArea()
        {           
            List<CatalogoArea> lista = new List<CatalogoArea>();
            DataTable dt = new DataTable();

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_COMUNICACION.USP_SEL_AREA";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                foreach (DataRow rowProducto in dt.Rows)
                {
                    CatalogoArea are = new CatalogoArea();
                    are.CodArea = Convert.ToInt32(rowProducto["CODAREA"]);
                    are.Descripcion = rowProducto["DESCRIPCION"].ToString();
                    lista.Add(are);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
        public List<CatalogoTramite> GetTramite(int idArea)
        {           
            List<CatalogoTramite> lista = new List<CatalogoTramite>();
            DataTable dt = new DataTable();

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_COMUNICACION.USP_SEL_TRAMITE";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_CODAREA", OracleDbType.Int32, ParameterDirection.Input, 255, idArea.ToString());
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                foreach (DataRow rowProducto in dt.Rows)
                {
                    CatalogoTramite tra = new CatalogoTramite();
                    tra.CodTramite = Convert.ToInt32(rowProducto["CODTRAMITE"]);
                    tra.Descripcion = rowProducto["DESCRIPCION"].ToString();
                    lista.Add(tra);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista.OrderBy(d => d.Descripcion).ToList();
        }

        [HttpGet]
        public List<CatalogoJurisdiccion> GetJurisdiccion()
        {
            List<CatalogoJurisdiccion> lista = new List<CatalogoJurisdiccion>();
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query = "SELECT TRV02.CODJURISDICCION,TRV02.NOMBRE FROM TRV02";

                DA.GetDataTable(query, out dt);

                foreach (DataRow rowProducto in dt.Rows)
                {
                    CatalogoJurisdiccion jur = new CatalogoJurisdiccion();
                    jur.CodJurisdiccion = Convert.ToInt32(rowProducto["CODJURISDICCION"]);
                    jur.Nombre = rowProducto["NOMBRE"].ToString();
                    lista.Add(jur);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
        public List<CatalogoJurisdiccion> GetJurisdiccionPanama()
        {
            List<CatalogoJurisdiccion> lista = new List<CatalogoJurisdiccion>();
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query = "SELECT TRV02.CODJURISDICCION,TRV02.NOMBRE FROM TRV02 WHERE TRV02.NOMBRE LIKE '%PANAMA'";
                query = query + " UNION SELECT TRV02.CODJURISDICCION,TRV02.NOMBRE FROM TRV02 WHERE TRV02.NOMBRE LIKE '%PANAMA%'";
                query = query + " UNION SELECT TRV02.CODJURISDICCION,TRV02.NOMBRE FROM TRV02 WHERE TRV02.NOMBRE LIKE 'PANAMA%'";

                DA.GetDataTable(query, out dt);

                foreach (DataRow rowProducto in dt.Rows)
                {
                    CatalogoJurisdiccion jur = new CatalogoJurisdiccion();
                    jur.CodJurisdiccion = Convert.ToInt32(rowProducto["CODJURISDICCION"]);
                    jur.Nombre = rowProducto["NOMBRE"].ToString();
                    lista.Add(jur);
                }
            }
            catch (Exception ex)
            {

            }

            return lista;
        }

        [HttpGet]
        public List<Cliente> GetClientesPorPalabras(string palabra)
        {
            List<Cliente> lista = new List<Cliente>();
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query1 = "SELECT CCV61.CLIENTE,CCV61.NIT,CCV61.NOMBRE,CCV61.CATEGORIA_CLIENTE,CCV61.EMAIL,CCV61.TELEFONO,CCV61.CODPAIS,CCV61.OFICINA,CCV61.CODCOBRA,CCV61.DILIGENCE,CCV61.CLASIFICACION_CLT,CCV61.SECTOR_ECONOMICO,CCV61.PEP,CCV61.RIESGO_PUNTOS,CCV61.CELULAR,CCV61.PASAPORTE,CCV61.OCUPACION FROM CCV61 WHERE CCV61.NOMBRE LIKE '" + palabra + "%'";
                string query2 = "SELECT CCV61.CLIENTE,CCV61.NIT,CCV61.NOMBRE,CCV61.CATEGORIA_CLIENTE,CCV61.EMAIL,CCV61.TELEFONO,CCV61.CODPAIS,CCV61.OFICINA,CCV61.CODCOBRA,CCV61.DILIGENCE,CCV61.CLASIFICACION_CLT,CCV61.SECTOR_ECONOMICO,CCV61.PEP,CCV61.RIESGO_PUNTOS,CCV61.CELULAR,CCV61.PASAPORTE,CCV61.OCUPACION FROM CCV61 WHERE CCV61.NOMBRE LIKE '% " + palabra + " %'";
                string query3 = "SELECT CCV61.CLIENTE,CCV61.NIT,CCV61.NOMBRE,CCV61.CATEGORIA_CLIENTE,CCV61.EMAIL,CCV61.TELEFONO,CCV61.CODPAIS,CCV61.OFICINA,CCV61.CODCOBRA,CCV61.DILIGENCE,CCV61.CLASIFICACION_CLT,CCV61.SECTOR_ECONOMICO,CCV61.PEP,CCV61.RIESGO_PUNTOS,CCV61.CELULAR,CCV61.PASAPORTE,CCV61.OCUPACION FROM CCV61 WHERE CCV61.NOMBRE LIKE '%" + palabra + "'";
                string queryFinal = query1 + " UNION " + query2 + " UNION " + query3;

                DA.GetDataTable(queryFinal, out dt);

                foreach (DataRow rowProducto in dt.Rows)
                {
                    Cliente clie = new Cliente();
                    clie.ID = Convert.ToInt32(rowProducto["CLIENTE"]);
                    clie.Nombre = rowProducto["NOMBRE"].ToString();
                    clie.Email = rowProducto["EMAIL"].ToString();
                    lista.Add(clie);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
        public DatosCliente GetClienteById(int id)
        {
            DatosCliente datosCliente = null;
            DataTable dtCliente = null;
            DataTable dtMoneda = null;
            DataTable dtDirecciones = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query = "SELECT CCV61.CLIENTE, " +
                                      "CCV61.NOMBRE, " +
                                      "CCV61.DUE_COMPLETE, " +
                                      "CCV61.RIESGO, " +
                                      "CCV61.RIESGO_PUNTOS, " +
                                      "CCV61.PASAPORTE, " +
                                      "CCV61.TELEFONO, " +
                                      "CCV61.CODPAIS, " +
                                      "CCV61.OFICINA, " +
                                      "CCV61.OCUPACION, " +
                                      "CCV61.CELULAR, " +
                                      "CCV61.EMAIL, " +
                                      "CCV61.SECTOR_ECONOMICO, " +
                                      "CCV61.PEP, " +
                                      "CCV61.CATEGORIA_CLIENTE, " +
                                      "CCV61.CLASIFICACION_CLT, " +
                                      "CCV61.DD_FORMULARIO, " +
                                      "CCV61.DD_PASSAPORTE, " +
                                      "CCV61.DD_REFERENCIA, " +
                                      "CCV61.DD_PROFESIONAL, " +
                                      "CCV61.DD_DOMICILIO, " +
                                      "CCV61.DD_INCORP, " +
                                      "CCV61.DD_PROMOSIONALES, " +
                                      "CCV61.DD_MEMBRESIA, " +
                                      "(SELECT  IDIOMA_FACTURACION FROM CCV90 WHERE CLIENTE = CCV61.CLIENTE AND rownum<=1) AS Idioma" +
                                 " FROM CCV61 " +
                                "WHERE CCV61.CLIENTE = " + id;

                DA.GetDataTable(query, out dtCliente);

                foreach (DataRow rowProducto in dtCliente.Rows)
                {
                    datosCliente = new DatosCliente();
                    datosCliente.Cod = Convert.IsDBNull(rowProducto["CLIENTE"]) ? 0 : Convert.ToInt32(rowProducto["CLIENTE"]);
                    datosCliente.Nombre = rowProducto["NOMBRE"].ToString();
                    datosCliente.EstadoCumplimiento = rowProducto["DUE_COMPLETE"].ToString().Equals("S") ? "S" : "N";
                    datosCliente.NivelRiesgo = rowProducto["RIESGO"].ToString();
                    datosCliente.PuntajeRiesgo = Convert.IsDBNull(rowProducto["RIESGO_PUNTOS"]) ? 0 : Convert.ToDecimal(rowProducto["RIESGO_PUNTOS"]);
                    datosCliente.Pasaporte = rowProducto["PASAPORTE"].ToString();
                    datosCliente.Idioma = rowProducto["IDIOMA"].ToString();
                    datosCliente.TelefonoCasa = rowProducto["TELEFONO"].ToString();
                    datosCliente.Pais = rowProducto["CODPAIS"].ToString();
                    datosCliente.Moneda = String.Empty; //Se carga del listado de monedas configuradas para el cliente
                    datosCliente.Ocupacion = rowProducto["OCUPACION"].ToString();
                    datosCliente.Celular = rowProducto["CELULAR"].ToString();
                    datosCliente.Email = rowProducto["EMAIL"].ToString();
                    datosCliente.SectorEconomico = Convert.IsDBNull(rowProducto["SECTOR_ECONOMICO"]) ? 0 : Convert.ToDecimal(rowProducto["SECTOR_ECONOMICO"]);
                    datosCliente.Pep = rowProducto["PEP"].ToString();
                    datosCliente.TipoPersona = rowProducto["CATEGORIA_CLIENTE"].ToString();
                    datosCliente.ClasificacionCliente = rowProducto["CLASIFICACION_CLT"].ToString();
                    datosCliente.ReqFormulario = rowProducto["DD_FORMULARIO"].ToString().Equals("S");
                    datosCliente.ReqPasaporte = rowProducto["DD_PASSAPORTE"].ToString().Equals("S");
                    datosCliente.ReqReferencia = rowProducto["DD_REFERENCIA"].ToString().Equals("S");
                    datosCliente.ReqProfesional = rowProducto["DD_PROFESIONAL"].ToString().Equals("S");
                    datosCliente.ReqDomicilio = rowProducto["DD_DOMICILIO"].ToString().Equals("S");
                    datosCliente.ReqIncorp = rowProducto["DD_INCORP"].ToString().Equals("S");
                    datosCliente.ReqPromocionales = rowProducto["DD_PROMOSIONALES"].ToString().Equals("S");
                    datosCliente.ReqMembresia = rowProducto["DD_MEMBRESIA"].ToString().Equals("S");
                    datosCliente.Monedas = new List<Moneda>();
                    datosCliente.LibretaDirecciones = new List<LibretaDireccion>();

                    //Cargo la lista de Monedas del Cliente
                    string QueryMonedas = "SELECT GSVMON.CODMONEDA, GSVMON.DESMONEDA, GSVMON.SIMBOLO " +
                                            "FROM GSVMON " +
                                           "WHERE GSVMON.CODMONEDA IN " +
                                           "(SELECT CCV85.CODMONEDA " +
                                              "FROM CCV85 " +
                                             "WHERE CCV85.CLIENTE = " + id + ") " +
                                           "ORDER BY GSVMON.CODMONEDA";

                    DA.GetDataTable(QueryMonedas, out dtMoneda);
                    foreach (DataRow rowMoneda in dtMoneda.Rows)
                    {
                        datosCliente.Monedas.Add(
                            new Moneda()
                            {
                                CodMoneda = Convert.IsDBNull(rowMoneda["CODMONEDA"]) ? 0 : Convert.ToInt32(rowMoneda["CODMONEDA"]),
                                DesMoneda = rowMoneda["DESMONEDA"].ToString(),
                                Simbolo = rowMoneda["SIMBOLO"].ToString()
                            });
                    }

                    //Cargo la lista de Direcciones del Cliente
                    string QueryDirecciones = "SELECT CCV90.CLIENTE, CCV90.COD_DIRECCION, CCV90.DIRECCION, CCV90.NOMBRE_FACT " +
                                                "FROM CCV90 " +
                                               "WHERE CLIENTE = " + id + " " +
                                              "ORDER BY CCV90.COD_DIRECCION";

                    DA.GetDataTable(QueryDirecciones, out dtDirecciones);
                    foreach (DataRow rowDireccion in dtDirecciones.Rows)
                    {
                        datosCliente.LibretaDirecciones.Add(
                            new LibretaDireccion()
                            {
                                CodCliente = Convert.ToInt32(rowDireccion["CLIENTE"]),
                                CodDireccion = rowDireccion["COD_DIRECCION"].ToString(),
                                Direccion = rowDireccion["DIRECCION"].ToString(),
                                NombreFactura = rowDireccion["NOMBRE_FACT"].ToString()
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return datosCliente;
        }

        [HttpGet]
        public List<LibretaDireccion> GetDireccionClienteById(int id)
        {
            List<LibretaDireccion> LibretaDirecciones = new List<LibretaDireccion>();
            DataTable dtDirecciones = null;
            try
            {

                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                //Cargo la lista de Direcciones del Cliente                    
                string QueryDirecciones = "SELECT CCV90.CLIENTE, CCV90.COD_DIRECCION, CCV90.DIRECCION, CCV90.NOMBRE_FACT " +
                                            "FROM CCV90 " +
                                           "WHERE CLIENTE = " + id + " " +
                                           "ORDER BY CCV90.COD_DIRECCION";

                DA.GetDataTable(QueryDirecciones, out dtDirecciones);
                foreach (DataRow rowDireccion in dtDirecciones.Rows)
                {
                    LibretaDirecciones.Add(new LibretaDireccion()
                        {
                            CodCliente = Convert.ToInt32(rowDireccion["CLIENTE"]),
                            CodDireccion = rowDireccion["COD_DIRECCION"].ToString(),
                            Direccion = rowDireccion["DIRECCION"].ToString(),
                            NombreFactura = rowDireccion["NOMBRE_FACT"].ToString()
                        });
                }
          
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return LibretaDirecciones;
        }

        [HttpGet]
        public Asunto GetAsuntoById(int id)
        {                     
            Asunto datosAsunto = null;
            DataTable dtAsuntos = null;
            DataTable dtEstado = null;
                                    
            try
            {

                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                /*
                string query = "SELECT TRVAS.ASUNTO, " +
                                        "TRVAS.CODMATERIA, " +
                                        "TRVAS.CODAPLICACION, " +
                                        "TRVAS.CODCIA, " +
                                        "TRVAS.CLIENTE, " +
                                        "TRVAS.CODIGO, " +
                                        "TRVAS.FECHAAPERTURA, " +
                                        "TRVAS.SECRETARIA, " +
                                        "TRVAS.TIPOFACTURA_AS, " +
                                        "TRVAS.OFICINA, " +
                                        "TRVAS.ESTATUS, " +
                                        "GSVCIAS.NOMBRECIA, " +
                                        "PLV61AST.NOMBREEMPLEADO, " +
                                        "NVL(CCV61.NOMBRE, '') AS NOMBRE_CLIENTE, " +
                                        "NVL(SOV06.NOMBRE, '') AS NOMBRE_SOCIEDAD " +
                                        "FROM TRVAS " +
                                        "LEFT JOIN GSVCIAS ON TRVAS.CODCIA = GSVCIAS.CODCIA " +
                                        "LEFT JOIN PLV61AST ON TRVAS.SECRETARIA = PLV61AST.CODEMPLEADO " +
                                        "LEFT OUTER JOIN CCV61 ON TRVAS.CLIENTE = CCV61.CLIENTE " +
                                        "LEFT OUTER JOIN SOV06 ON TRVAS.CODIGO = SOV06.CODSOCIEDAD " +
                                        "WHERE ASUNTO = " + id;
                 */
                string query = @"
SELECT
  NVL(A.ASUNTO, '') AS ASUNTO,
  NVL(CO.CODCIA, '') AS CODIGO_COMPANIA,
  NVL(CO.NOMBRECIA, '') AS NOMBRE_COMPANIA,
  NVL(A.SECCION, '') AS SECCION,
  CASE
    WHEN A.TIPO_AL='M' THEN 'MARCAS PATENTES / PATENTEDTRADEMARKS'
    WHEN A.TIPO_AL='R' THEN 'REGISTROS SANITARIOS / SANITARY REGISTRIES'
    WHEN A.TIPO_AL='L' THEN 'LICENCIAS COMERCIALES / COMMERCIAL LICENSES'
    WHEN A.TIPO_AL='D' THEN 'DERECHO AUTOR / AUTHOR RIGHT'
    WHEN A.TIPO_AL='V' THEN 'MIGRACION / MIGRATION'
    WHEN A.TIPO_AL='O' THEN 'OTROS'
    ELSE A.TIPO_AL
  END AS PROPIEDAD_INTELECTUAL,
  NVL(A.NOMBRE, '') AS NOMBRE_FACT,
  NVL(J.CODJURISDICCION, '') AS CODIGO_JURISDICCION,
  NVL(J.NOMBRE, '') AS NOMBRE_JURISDICCION,
  NVL(A.CODMATERIA, '') AS CODIGO_MATERIA,
  NVL(M.DESCRIPCION, '') AS NOMBRE_MATERIA,
  NVL(C.NOMBRE, '') AS CORRESPONSAL,
  NVL(D.DIRECCION, '') AS DIRECCION,
  NVL(A.REFERENCIA, '') AS REFERENCIA,
  NVL(A.FECHAAPERTURA, '') AS FECHA_APERTURA,
  NVL(AB.NOMBREEMPLEADO, '') AS ABOGADO,
  NVL(S.NOMBREEMPLEADO, '') AS SECRETARIA,
  NVL(A.OFICINA, '') AS OFICINA,
  NVL(A.OBSERVACIONES, '') AS OBSERVACIONES,
  NVL(A.CODAPLICACION, '') AS CODIGO_APLICACION,
  NVL(A.CLIENTE, '') AS CODIGO_CLIENTE,
  NVL(A.CODIGO, '') AS CODIGO,
  NVL(A.TIPOFACTURA_AS, '') AS TIPO_FACTURA,
  NVL(C.NOMBRE, '') AS NOMBRE_CLIENTE,
  NVL(SO.NOMBRE, '') AS NOMBRE_SOCIEDAD,
  NVL(NA.NOMBRE, '') AS NOMBRE_NAVE,
  NVL(A.SECRETARIA, '') AS CODIGO_SECRETARIA,
  NVL(A.REFERIDO_POR, '') AS REFERIDO_POR,
  NVL(A.ESTATUS, '') AS CODIGO_ESTADO,
  CASE
    WHEN A.ESTATUS='A' THEN 'ABIERTO'
    WHEN A.ESTATUS='P' THEN 'PENDIENTE'
    WHEN A.ESTATUS='T' THEN 'TERMINADO'
    WHEN A.ESTATUS='X' THEN 'ELIMINADO'
    ELSE A.ESTATUS
  END AS NOMBRE_ESTADO
FROM
  ULTIMUS.TRVAS A
  LEFT OUTER JOIN ULTIMUS.GSVCIAS CO ON A.CODCIA=CO.CODCIA
  LEFT OUTER JOIN ULTIMUS.TRV02 J ON A.CODJURISDICCION=J.CODJURISDICCION
  LEFT OUTER JOIN ULTIMUS.CCV90 D ON A.CLIENTE=D.CLIENTE AND A.CODDIRECCION=D.COD_DIRECCION
  LEFT OUTER JOIN ULTIMUS.PLV61AST S ON A.SECRETARIA=S.CODEMPLEADO
  LEFT OUTER JOIN CCV61 C ON A.CLIENTE = C.CLIENTE
  LEFT OUTER JOIN ULTIMUS.SOV06 SO ON A.CODAPLICACION='SOC' AND A.CODIGO=SO.CODSOCIEDAD AND A.CODCIA=SO.CODCIA
  LEFT OUTER JOIN ULTIMUS.NVV09 NA ON A.CODAPLICACION='NAV' AND A.CODIGO=NA.CODSOCIEDAD AND A.CODCIA=NA.CODCIA
  LEFT OUTER JOIN ULTIMUS.TRV04 M ON A.CODCIA=M.CODCIA AND A.CODAPLICACION=M.CODAPLICACION AND A.CODMATERIA=M.CODMATERIA
  LEFT OUTER JOIN ULTIMUS.PLV61ABOG AB ON A.CODEMPLEADO=AB.CODEMPLEADO
WHERE
  A.ASUNTO=" + id;

                DA.GetDataTable(query, out dtAsuntos);

                foreach (DataRow rowProducto in dtAsuntos.Rows)
                {
                    datosAsunto = new Asunto();
                    datosAsunto.NroAsunto = Convert.ToInt32(rowProducto["ASUNTO"]);
                    datosAsunto.Aplicacion = rowProducto["CODIGO_APLICACION"].ToString();
                    datosAsunto.CodCompany = rowProducto["CODIGO_COMPANIA"].ToString();
                    datosAsunto.Company = rowProducto["NOMBRE_COMPANIA"].ToString();
                    datosAsunto.Oficina = rowProducto["OFICINA"].ToString();
                    datosAsunto.Estado = rowProducto["CODIGO_ESTADO"].ToString();
                    datosAsunto.NombreEstado = rowProducto["NOMBRE_ESTADO"].ToString();
                    datosAsunto.FechaApertura = rowProducto["FECHA_APERTURA"].ToString();
                    datosAsunto.CodSociedad = rowProducto["CODIGO"].ToString();
                    datosAsunto.SociedadFundac = rowProducto["NOMBRE_SOCIEDAD"].ToString();
                    datosAsunto.CodCliente = rowProducto["CODIGO_CLIENTE"].ToString();
                    datosAsunto.NombreCliente = rowProducto["NOMBRE_CLIENTE"].ToString();
                    datosAsunto.Asistente = rowProducto["SECRETARIA"].ToString();
                    datosAsunto.Secretaria = rowProducto["SECRETARIA"].ToString();
                    datosAsunto.CodAsistente = rowProducto["CODIGO_SECRETARIA"].ToString();
                    datosAsunto.TipoFactura = rowProducto["TIPO_FACTURA"].ToString();

                    datosAsunto.Seccion = rowProducto["SECCION"].ToString();
                    datosAsunto.PropiedadIntelectual = rowProducto["PROPIEDAD_INTELECTUAL"].ToString();
                    datosAsunto.NombreFactura = rowProducto["NOMBRE_FACT"].ToString();
                    datosAsunto.CodJurisdiccion = rowProducto["CODIGO_JURISDICCION"].ToString();
                    datosAsunto.DescJurisdiccion = rowProducto["NOMBRE_JURISDICCION"].ToString();
                    datosAsunto.CodMateria = rowProducto["CODIGO_MATERIA"].ToString();
                    datosAsunto.DescMateria = rowProducto["NOMBRE_MATERIA"].ToString();
                    datosAsunto.Corresponsal = rowProducto["CORRESPONSAL"].ToString();
                    datosAsunto.Direccion = rowProducto["DIRECCION"].ToString();
                    datosAsunto.Referencia = rowProducto["REFERENCIA"].ToString();
                    datosAsunto.Abogado = rowProducto["ABOGADO"].ToString();
                    datosAsunto.Observaciones = rowProducto["OBSERVACIONES"].ToString();
                    datosAsunto.CodNave = rowProducto["CODIGO"].ToString();
                    datosAsunto.DescNave = rowProducto["NOMBRE_NAVE"].ToString();
                    datosAsunto.ReferidoPor = rowProducto["REFERIDO_POR"].ToString();
                    datosAsunto.Responsable = rowProducto["NOMBRE_CLIENTE"].ToString();

                    datosAsunto.EstadoSolicitudes = new List<AsuntoEstadoSolicitudes>();
                }
                
                //Estado Solicitudes
                string querySolicitudes = "SELECT TRV151.ASUNTO, " +
                                                "TRV151.CODMATERIA, " +
                                                "TRV151.CODAPLICACION, " +
                                                "TRV151.CODCIA, " +
                                                "TRV151.NO_SOLICITUD, " +
                                                "TRV151.TIPO, " +
                                                "TRV151.FECHA_SOL, " +
                                                "TRV151.ASIGNADO_A, " +
                                                "TRV151.ESTADO  " +  
                                            "FROM TRV151 " +
                                            "WHERE TRV151.ASUNTO = " + id +
                                            " ORDER BY TRV151.FECHA_SOL";

                DA.GetDataTable(querySolicitudes, out dtEstado);

                foreach (DataRow rowEstado in dtEstado.Rows)
                {
                    datosAsunto.EstadoSolicitudes.Add(new AsuntoEstadoSolicitudes()
                        {
                            Tipo = GetDescriptionByTipoES(rowEstado["TIPO"].ToString()),
                            Asignado = rowEstado["ASIGNADO_A"].ToString(),
                            Estado = GetDescriptionByEstadoES(rowEstado["ESTADO"].ToString()),
                            FechaSolicitud = rowEstado["FECHA_SOL"].ToString()                      
                        });
                }                   
                
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return datosAsunto;
        }

        [HttpGet]
        public Asunto GetAsuntoNavesById(int id)
        {
            Asunto datosAsunto = null;
            DataTable dtAsuntos = null;
            DataTable dtEstado = null;

            try
            {

                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query = "SELECT TRVAS.ASUNTO, " +
                                      "TRVAS.CODMATERIA, " +
                                      "TRVAS.CODAPLICACION, " +
                                      "TRVAS.CODCIA, " +
                                      "TRVAS.CLIENTE, " +
                                      "TRVAS.CODIGO, " +
                                      "TRVAS.FECHAAPERTURA, " +
                                      "TRVAS.SECRETARIA, " +
                                      "TRVAS.TIPOFACTURA_AS, " + 
                                      "TRVAS.OFICINA, " +
                                      "TRVAS.ESTATUS, " +
                                      "TRVAS.CODDIRECCION, " +
                                      "TRVAS.REFERENCIA, " +
                                      "TRVAS.REFERIDO_POR, " +
                                      "TRVAS.NOMBRE, " +
                                      "TRVAS.CODJURISDICCION, " +
                                      "GSVCIAS.NOMBRECIA, " + 
                                      "PLV61AST.NOMBREEMPLEADO, " +
                                      "(SELECT TRV02.NOMBRE FROM TRV02 WHERE TRV02.CODJURISDICCION = TRVAS.CODJURISDICCION) AS NOMBRE_JURISDICCION, " +
                                      "(SELECT NVL(CCV61.NOMBRE, '') FROM CCV61 WHERE CCV61.CLIENTE = TRVAS.CLIENTE) AS NOMBRE_CLIENTE, " +
                                      "(SELECT NVL(NVV09.NOMBRE, '') FROM NVV09 WHERE NVV09.CODNAVE = TRVAS.CODIGO) AS NOMBRE_NAVE " +
                                 "FROM TRVAS " +
                                      "LEFT JOIN GSVCIAS ON TRVAS.CODCIA = GSVCIAS.CODCIA " +
                                      "LEFT JOIN PLV61AST ON TRVAS.SECRETARIA = PLV61AST.CODEMPLEADO " +
                                "WHERE ASUNTO = " + id;

                DA.GetDataTable(query, out dtAsuntos);

                foreach (DataRow rowProducto in dtAsuntos.Rows)
                {
                    datosAsunto = new Asunto();
                    datosAsunto.NroAsunto = Convert.ToInt32(rowProducto["ASUNTO"]);
                    datosAsunto.CodCompany = rowProducto["CODCIA"].ToString();
                    datosAsunto.Company = rowProducto["NOMBRECIA"].ToString();
                    datosAsunto.CodJurisdiccion = rowProducto["CODJURISDICCION"].ToString();
                    datosAsunto.DescJurisdiccion = rowProducto["NOMBRE_JURISDICCION"].ToString();
                    datosAsunto.CodNave = rowProducto["CODIGO"].ToString();
                    datosAsunto.DescNave = rowProducto["NOMBRE_NAVE"].ToString();
                    datosAsunto.Oficina = rowProducto["OFICINA"].ToString();
                    datosAsunto.CodMateria = rowProducto["CODMATERIA"].ToString();
                    datosAsunto.Responsable = rowProducto["NOMBRE_CLIENTE"].ToString();
                    datosAsunto.Direccion = rowProducto["CODDIRECCION"].ToString();
                    datosAsunto.Referencia = rowProducto["REFERENCIA"].ToString();
                    datosAsunto.ReferidoPor = rowProducto["REFERIDO_POR"].ToString();
                    datosAsunto.NombreFactura = rowProducto["NOMBRE"].ToString();
                    datosAsunto.Secretaria = rowProducto["NOMBREEMPLEADO"].ToString();
                    if (rowProducto["FECHAAPERTURA"].ToString() != "")
                        datosAsunto.FechaApertura = Convert.ToDateTime(rowProducto["FECHAAPERTURA"]).ToString("MM/dd/yyyy");
                    else
                        datosAsunto.FechaApertura = "";
                    datosAsunto.Estado = GetDescriptionByEstadoAsunto(rowProducto["ESTATUS"].ToString());
                    datosAsunto.EstadoSolicitudes = new List<AsuntoEstadoSolicitudes>();
                }

                //Saldos

                //Estado Solicitudes
                string querySolicitudes = "SELECT TRV151.ASUNTO, " +
                                                "TRV151.CODMATERIA, " +
                                                "TRV151.CODAPLICACION, " +
                                                "TRV151.CODCIA, " +
                                                "TRV151.NO_SOLICITUD, " +
                                                "TRV151.TIPO, " +
                                                "TRV151.FECHA_SOL, " +
                                                "TRV151.ASIGNADO_A, " +
                                                "TRV151.ESTADO  " +
                                            "FROM TRV151 " +
                                            "WHERE TRV151.ASUNTO = " + id;

                DA.GetDataTable(querySolicitudes, out dtEstado);

                foreach (DataRow rowEstado in dtEstado.Rows)
                {
                    datosAsunto.EstadoSolicitudes.Add(new AsuntoEstadoSolicitudes()
                    {
                        Tipo = GetDescriptionByTipoES(rowEstado["TIPO"].ToString()),
                        Asignado = rowEstado["ASIGNADO_A"].ToString(),
                        Estado = GetDescriptionByEstadoES(rowEstado["ESTADO"].ToString()),
                        FechaSolicitud = rowEstado["FECHA_SOL"].ToString()
                    });
                }

            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return datosAsunto;
        }

        [HttpGet]
        public List<Pais> GetPaises()
        {
            List<Pais> lista = new List<Pais>();
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query = "SELECT GSVPA.CODPAIS, GSVPA.NOMBRE FROM GSVPA ORDER BY GSVPA.NOMBRE";

                DA.GetDataTable(query, out dt);

                foreach (DataRow rowProducto in dt.Rows)
                {
                    Pais pais = new Pais();
                    pais.CodPais = Convert.ToInt32(rowProducto["CODPAIS"]);
                    pais.Nombre = rowProducto["NOMBRE"].ToString();
                    lista.Add(pais);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
        public List<SectorEconomico> GetSectoresEconomicos()
        {
            List<SectorEconomico> lista = new List<SectorEconomico>();
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query = "SELECT CCV18.CODSECTOR, CCV18.DESCRIPCION FROM CCV18 ORDER BY CCV18.DESCRIPCION";

                DA.GetDataTable(query, out dt);

                foreach (DataRow rowProducto in dt.Rows)
                {
                    SectorEconomico se = new SectorEconomico();
                    se.CodSector = Convert.ToInt32(rowProducto["CODSECTOR"]);
                    se.Descripcion = rowProducto["DESCRIPCION"].ToString();
                    lista.Add(se);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
        public List<Moneda> GetMonedas()
        {
            List<Moneda> lista = new List<Moneda>();
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query = "SELECT GSVMON.CODMONEDA, GSVMON.DESMONEDA, GSVMON.SIMBOLO " +
                                 "FROM GSVMON " +
                               "ORDER BY GSVMON.DESMONEDA";

                DA.GetDataTable(query, out dt);

                foreach (DataRow rowProducto in dt.Rows)
                {
                    Moneda moneda = new Moneda();
                    moneda.CodMoneda = Convert.ToInt32(rowProducto["CODMONEDA"]);
                    moneda.DesMoneda = rowProducto["DESMONEDA"].ToString();
                    moneda.Simbolo = rowProducto["SIMBOLO"].ToString();
                    lista.Add(moneda);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
        public List<Abogado> GetAbogado(int idIdioma, int idProcedencia, int idArea, string jurisdiccion)
        {
            List<Abogado> lista = new List<Abogado>();
            DataTable dt = new DataTable();

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_COMUNICACION.USP_SEL_ABOGADOS";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_CODIDIOMA", OracleDbType.Int32, ParameterDirection.Input, 255, idIdioma.ToString());
                Procedimiento.AddParametros("P_CODPROCEDENCIA", OracleDbType.Int32, ParameterDirection.Input, 255, idProcedencia.ToString());
                Procedimiento.AddParametros("P_CODAREA", OracleDbType.Int32, ParameterDirection.Input, 255, idArea.ToString());
                Procedimiento.AddParametros("P_JURISDICCION", OracleDbType.Varchar2, ParameterDirection.Input, 255, jurisdiccion.ToString());
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                foreach (DataRow rowProducto in dt.Rows)
                {
                    Abogado abo = new Abogado();
                    abo.Nombre = rowProducto["NOMBRE_COMPLETO"].ToString();
                    lista.Add(abo);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista.OrderBy(a => a.Nombre).ToList();
        }

        [HttpGet]
        public List<Abogado> GetAbogado(int idIdioma)
        {
            List<Abogado> lista = new List<Abogado>();
            DataTable dt = new DataTable();

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_COMUNICACION.USP_SEL_ABOGADOS_X_IDIOMA";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_CODIDIOMA", OracleDbType.Int32, ParameterDirection.Input, 255, idIdioma.ToString());
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                foreach (DataRow rowProducto in dt.Rows)
                {
                    Abogado abo = new Abogado();
                    abo.Usuario = rowProducto["NOMBRE_USUARIO"].ToString();
                    abo.Nombre = rowProducto["NOMBRE_COMPLETO"].ToString();
                    lista.Add(abo);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista.OrderBy(a => a.Nombre).ToList();
        }

        [HttpPost]
        public int RegistrarCorreo(RegistrarCorreo registrarCorreo)
        {
            bool confirm = false;
            int incidente = 0;
            try
            {
                //Completar Etapa                

                //Asigna valores a las variables del esquema
                List<NodeVariables> nodeList = new List<NodeVariables>();
                NodeVariables node;

                node = new NodeVariables();
                node.NodeName = "TaskData.Global.NombreProceso";
                node.NodeValue = registrarCorreo.Proceso;
                nodeList.Add(node);

                node = new NodeVariables();
                node.NodeName = "TaskData.Global.EsAbogado";
                node.NodeValue = string.IsNullOrEmpty(registrarCorreo.Respuesta) ? "0" : "1";
                nodeList.Add(node);

                node = new NodeVariables();
                node.NodeName = "TaskData.Global.EsDevuelto";
                node.NodeValue = string.IsNullOrEmpty(registrarCorreo.AsuntoOrigen) ? "0" : (registrarCorreo.AsuntoOrigen.Split('-').Length < 2 ? "0" : registrarCorreo.AsuntoOrigen.Split('-')[1]);
                nodeList.Add(node);

                node = new NodeVariables();
                node.NodeName = "TaskData.Global.RecRespondeCorreo";
                node.NodeValue = registrarCorreo.UsuarioAbogadoAsignado;
                nodeList.Add(node);

                string summary = registrarCorreo.Nombre;
                //recibe numero de incidente
                incidente = UltimusManager.CompleteTask(registrarCorreo.UserId, registrarCorreo.TaskId, String.Empty, summary, nodeList);
                AttachmentServiceClient client = new AttachmentServiceClient();
                client.MoveFiles("Comunicacion", "_tmp_" + registrarCorreo.UserId.Replace("/", "_"), incidente.ToString());
                GeneralesController g = new GeneralesController();
                g.ActualizarBitacoraComentarios(registrarCorreo.Proceso, registrarCorreo.TempIncident, incidente.ToString());

                //Inserta en la base de datos
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_COMUNICACION.USP_INS_COMUNICACION";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Int32, ParameterDirection.Input, 255, incidente.ToString());
                Procedimiento.AddParametros("P_IDCLIENTE", OracleDbType.Int32, ParameterDirection.Input, 255, registrarCorreo.IdCliente.ToString());
                Procedimiento.AddParametros("P_NOMBRE", OracleDbType.Varchar2, ParameterDirection.Input, 100, string.IsNullOrEmpty(registrarCorreo.Nombre) ? "" : registrarCorreo.Nombre);
                Procedimiento.AddParametros("P_INCIDENTE_RELACIONADO", OracleDbType.Varchar2, ParameterDirection.Input, 100, registrarCorreo.IncidenteRelacionado.ToString());
                Procedimiento.AddParametros("P_CODIDIOMA", OracleDbType.Int32, ParameterDirection.Input, 255, registrarCorreo.CodIdioma.ToString());
                Procedimiento.AddParametros("P_CODPROCEDENCIA", OracleDbType.Int32, ParameterDirection.Input, 255, registrarCorreo.CodProcedencia.ToString());
                Procedimiento.AddParametros("P_CODAREA", OracleDbType.Int32, ParameterDirection.Input, 255, registrarCorreo.CodArea.ToString());
                Procedimiento.AddParametros("P_CODJURISDICCION", OracleDbType.Int32, ParameterDirection.Input, 255, registrarCorreo.CodJurisdiccion.ToString());
                Procedimiento.AddParametros("P_CODTRAMITE", OracleDbType.Int32, ParameterDirection.Input, 255, registrarCorreo.CodTramite.ToString());
                Procedimiento.AddParametros("P_ABOGADO_ASIGNADO", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.IsNullOrEmpty(registrarCorreo.AbogadoAsignado) ? "" : registrarCorreo.AbogadoAsignado);
                Procedimiento.AddParametros("P_CORREO_ORIGEN", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.IsNullOrEmpty(registrarCorreo.CorreoOrigen) ? "" : registrarCorreo.CorreoOrigen);
                Procedimiento.AddParametros("P_CORREO_PARA", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.IsNullOrEmpty(registrarCorreo.CorreoPara) ? "" : registrarCorreo.CorreoPara);
                Procedimiento.AddParametros("P_ASUNTO", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.IsNullOrEmpty(registrarCorreo.Asunto) ? "" : registrarCorreo.Asunto);
                Procedimiento.AddParametros("P_CONTENIDO", OracleDbType.Clob, ParameterDirection.Input, 1000000, string.IsNullOrEmpty(registrarCorreo.Contenido) ? "" : registrarCorreo.Contenido);
                Procedimiento.AddParametros("P_ASUNTO_ORIGEN", OracleDbType.Varchar2, ParameterDirection.Input, 5, string.IsNullOrEmpty(registrarCorreo.AsuntoOrigen) ? "" : registrarCorreo.AsuntoOrigen);
                Procedimiento.AddParametros("P_CORREO_INFRACTOR", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.IsNullOrEmpty(registrarCorreo.CorreoInfractor) ? "" : registrarCorreo.CorreoInfractor);
                Procedimiento.AddParametros("P_RESPUESTA", OracleDbType.Clob, ParameterDirection.Input, 1000000, string.IsNullOrEmpty(registrarCorreo.Respuesta) ? "" : registrarCorreo.Respuesta);
                Procedimiento.AddParametros("P_ABOGADO_RESPONDE", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.IsNullOrEmpty(registrarCorreo.AbogadoResponde) ? "" : registrarCorreo.AbogadoResponde);
                Procedimiento.AddParametros("P_GENERA_INSTRUCCION", OracleDbType.Int32, ParameterDirection.Input, 255, "");
                Procedimiento.AddParametros("P_ETAPA", OracleDbType.Varchar2, ParameterDirection.Input, 255, registrarCorreo.Etapa);
                Procedimiento.AddParametros("P_LISTA_ADJUNTOS", OracleDbType.Clob, ParameterDirection.Input, 1000000, "");
                Procedimiento.AddParametros("P_CORREO_CC", OracleDbType.Varchar2, ParameterDirection.Input, 1000, string.IsNullOrEmpty(registrarCorreo.CorreoCC) ? "" : registrarCorreo.CorreoCC);

                Procedimientos.Add(Procedimiento);
                confirm = DA.EjecutarProcedimientos(Procedimientos);
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.StackTrace);
            }

            return confirm ? incidente : 0;
        }

        [HttpPost]
        public bool RegistrarCorreoServiciosLegales(Comunicacion comunicacion)
        {
            bool ret = false;
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                bool notificacionServiciosLegales = (comunicacion.EtapaActual == "NotificarCliente" || comunicacion.EtapaActual == "PendDocCliente");

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_COMUNICACION.USP_INS_UPD_COMUNICACION";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Int32, ParameterDirection.Input, 255, notificacionServiciosLegales ? "0" : comunicacion.Incidente.ToString());
                Procedimiento.AddParametros("P_IDCLIENTE", OracleDbType.Int32, ParameterDirection.Input, 255, comunicacion.IdCliente.ToString());
                Procedimiento.AddParametros("P_NOMBRE", OracleDbType.Varchar2, ParameterDirection.Input, 100, string.IsNullOrEmpty(comunicacion.Nombre) ? "" : comunicacion.Nombre);
                Procedimiento.AddParametros("P_INCIDENTE_RELACIONADO", OracleDbType.Varchar2, ParameterDirection.Input, 100, comunicacion.Incidente_Relacionado.ToString());
                Procedimiento.AddParametros("P_CODIDIOMA", OracleDbType.Int32, ParameterDirection.Input, 255, comunicacion.CodIdioma.ToString());
                Procedimiento.AddParametros("P_CODPROCEDENCIA", OracleDbType.Int32, ParameterDirection.Input, 255, comunicacion.CodProcedencia.ToString());
                Procedimiento.AddParametros("P_CODAREA", OracleDbType.Int32, ParameterDirection.Input, 255, comunicacion.CodArea.ToString());
                Procedimiento.AddParametros("P_CODJURISDICCION", OracleDbType.Int32, ParameterDirection.Input, 255, comunicacion.CodJurisdiccion.ToString());
                Procedimiento.AddParametros("P_CODTRAMITE", OracleDbType.Int32, ParameterDirection.Input, 255, comunicacion.CodTramite.ToString());
                Procedimiento.AddParametros("P_ABOGADO_ASIGNADO", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.IsNullOrEmpty(comunicacion.Abogado_Asignado) ? "" : comunicacion.Abogado_Asignado);
                Procedimiento.AddParametros("P_CORREO_ORIGEN", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.IsNullOrEmpty(comunicacion.Correo_Origen) ? "" : comunicacion.Correo_Origen);
                Procedimiento.AddParametros("P_CORREO_PARA", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.IsNullOrEmpty(comunicacion.Correo_Para) ? "" : comunicacion.Correo_Para);
                Procedimiento.AddParametros("P_ASUNTO", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.IsNullOrEmpty(comunicacion.Asunto) ? "" : comunicacion.Asunto);
                Procedimiento.AddParametros("P_CONTENIDO", OracleDbType.Clob, ParameterDirection.Input, 1000000, string.IsNullOrEmpty(comunicacion.Contenido) ? "" : comunicacion.Contenido);
                Procedimiento.AddParametros("P_ASUNTO_ORIGEN", OracleDbType.Varchar2, ParameterDirection.Input, 5, "");
                Procedimiento.AddParametros("P_CORREO_INFRACTOR", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.IsNullOrEmpty(comunicacion.Correo_Infractor) ? "" : comunicacion.Correo_Infractor);
                Procedimiento.AddParametros("P_RESPUESTA", OracleDbType.Clob, ParameterDirection.Input, 1000000, string.IsNullOrEmpty(comunicacion.Respuesta) ? "" : comunicacion.Respuesta);
                Procedimiento.AddParametros("P_ABOGADO_RESPONDE", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.IsNullOrEmpty(comunicacion.Abogado_Responde) ? "" : comunicacion.Abogado_Responde);
                Procedimiento.AddParametros("P_GENERA_INSTRUCCION", OracleDbType.Int32, ParameterDirection.Input, 255, "");
                Procedimiento.AddParametros("P_ETAPA_ACTUAL", OracleDbType.Varchar2, ParameterDirection.Input, 255, comunicacion.EtapaActual.ToString());
                Procedimiento.AddParametros("P_ETAPA_ANTERIOR", OracleDbType.Varchar2, ParameterDirection.Input, 255, comunicacion.EtapaAnterior.ToString());
                Procedimiento.AddParametros("P_CORREO_CC", OracleDbType.Varchar2, ParameterDirection.Input, 1000, string.IsNullOrEmpty(comunicacion.CorreoCC) ? "" : comunicacion.CorreoCC);

                Procedimientos.Add(Procedimiento);
                ret = DA.EjecutarProcedimientos(Procedimientos);

                if (ret && notificacionServiciosLegales)
                {
                    using(WsComunicacion.WsComunicacion client = new WsComunicacion.WsComunicacion())
                    {
                        client.EnviarNotificacionServiciosLegales("ServiciosLegales", int.Parse(comunicacion.Incidente_Relacionado));
                    }
                }

            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.StackTrace);
            }

            return ret;
        }

        [HttpPost]
        public bool ActualizarCorreo(ActualizarCorreo actualizarCorreo)
        {
            bool confirm = false;
            try
            {
                //Actualiza en la base de datos
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_COMUNICACION.USP_UDP_COMUNICACION";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Int32, ParameterDirection.Input, 255, actualizarCorreo.Incidente.ToString());
                Procedimiento.AddParametros("P_IDCLIENTE", OracleDbType.Int32, ParameterDirection.Input, 255, actualizarCorreo.IdCliente.ToString());
                Procedimiento.AddParametros("P_NOMBRE", OracleDbType.Varchar2, ParameterDirection.Input, 100, string.IsNullOrEmpty(actualizarCorreo.Nombre) ? "" : actualizarCorreo.Nombre);
                Procedimiento.AddParametros("P_INCIDENTE_RELACIONADO", OracleDbType.Varchar2, ParameterDirection.Input, 100, actualizarCorreo.IncidenteRelacionado.ToString());
                Procedimiento.AddParametros("P_CODIDIOMA", OracleDbType.Int32, ParameterDirection.Input, 255, actualizarCorreo.CodIdioma.ToString());
                Procedimiento.AddParametros("P_CODPROCEDENCIA", OracleDbType.Int32, ParameterDirection.Input, 255, actualizarCorreo.CodProcedencia.ToString());
                Procedimiento.AddParametros("P_CODAREA", OracleDbType.Int32, ParameterDirection.Input, 255, actualizarCorreo.CodArea.ToString());
                Procedimiento.AddParametros("P_CODJURISDICCION", OracleDbType.Int32, ParameterDirection.Input, 255, actualizarCorreo.CodJurisdiccion.ToString());
                Procedimiento.AddParametros("P_CODTRAMITE", OracleDbType.Int32, ParameterDirection.Input, 255, actualizarCorreo.CodTramite.ToString());
                Procedimiento.AddParametros("P_ABOGADO_ASIGNADO", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.IsNullOrEmpty(actualizarCorreo.AbogadoAsignado) ? "" : actualizarCorreo.AbogadoAsignado);
                Procedimiento.AddParametros("P_CORREO_ORIGEN", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.IsNullOrEmpty(actualizarCorreo.CorreoOrigen) ? "" : actualizarCorreo.CorreoOrigen);
                Procedimiento.AddParametros("P_CORREO_PARA", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.IsNullOrEmpty(actualizarCorreo.CorreoPara) ? "" : actualizarCorreo.CorreoPara);
                Procedimiento.AddParametros("P_ASUNTO", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.IsNullOrEmpty(actualizarCorreo.Asunto) ? "" : actualizarCorreo.Asunto);
                Procedimiento.AddParametros("P_CONTENIDO", OracleDbType.Clob, ParameterDirection.Input, 1000000, string.IsNullOrEmpty(actualizarCorreo.Contenido) ? "" : actualizarCorreo.Contenido);
                Procedimiento.AddParametros("P_ASUNTO_ORIGEN", OracleDbType.Varchar2, ParameterDirection.Input, 5, string.IsNullOrEmpty(actualizarCorreo.AsuntoOrigen) ? "" : actualizarCorreo.AsuntoOrigen);
                Procedimiento.AddParametros("P_CORREO_INFRACTOR", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.IsNullOrEmpty(actualizarCorreo.CorreoInfractor) ? "" : actualizarCorreo.CorreoInfractor);
                Procedimiento.AddParametros("P_RESPUESTA", OracleDbType.Clob, ParameterDirection.Input, 1000000, string.IsNullOrEmpty(actualizarCorreo.Respuesta) ? "" : actualizarCorreo.Respuesta);
                Procedimiento.AddParametros("P_ABOGADO_RESPONDE", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.IsNullOrEmpty(actualizarCorreo.AbogadoResponde) ? "" : actualizarCorreo.AbogadoResponde);
                Procedimiento.AddParametros("P_GENERA_INSTRUCCION", OracleDbType.Int32, ParameterDirection.Input, 255, "");
                Procedimiento.AddParametros("P_CORREO_CC", OracleDbType.Varchar2, ParameterDirection.Input, 1000, string.IsNullOrEmpty(actualizarCorreo.CorreoCC) ? "" : actualizarCorreo.CorreoCC);

                Procedimientos.Add(Procedimiento);
                confirm = DA.EjecutarProcedimientos(Procedimientos);

                if(confirm)
                {
                    List<NodeVariables> nodeList = new List<NodeVariables>();
                    NodeVariables node;

                    node = new NodeVariables();
                    node.NodeName = "TaskData.Global.EsDevuelto";
                    node.NodeValue = "0";
                    nodeList.Add(node);

                    node = new NodeVariables();
                    node.NodeName = "TaskData.Global.RecRespondeCorreo";
                    node.NodeValue = actualizarCorreo.UsuarioAbogadoAsignado;
                    nodeList.Add(node);

                    string summary = actualizarCorreo.Nombre;
                    var incidente = UltimusManager.CompleteTask(actualizarCorreo.UserId, actualizarCorreo.TaskId, String.Empty, summary, nodeList);
                    confirm = incidente > 0;
                }

            }
            catch (Exception ex)
            {
                confirm = false;
                UltimusLogs.Error(ex.StackTrace);
            }

            return confirm;
        }

        [HttpPost]
        public int RespondeCorreo(RespondeCorreo respondeCorreo)
        {
            int confirm = 0;
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_COMUNICACION.USP_UPD_COMUNICACION_RESPONDE";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_RESPUESTA", OracleDbType.Clob, ParameterDirection.Input, 1000000, respondeCorreo.Respuesta);
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Int32, ParameterDirection.Input, 255, respondeCorreo.Incidente.ToString());
                Procedimiento.AddParametros("P_ABOGADO_RESPONDE ", OracleDbType.Varchar2, ParameterDirection.Input, 200, string.IsNullOrEmpty(respondeCorreo.AbogadoResponde) ? "" : respondeCorreo.AbogadoResponde);
                Procedimiento.AddParametros("P_CORREO_CC", OracleDbType.Varchar2, ParameterDirection.Input, 1000, string.IsNullOrEmpty(respondeCorreo.CorreoCC) ? "" : respondeCorreo.CorreoCC);

                Procedimientos.Add(Procedimiento);
                bool ret = DA.EjecutarProcedimientos(Procedimientos);

                if (ret)
                {
                    List<NodeVariables> nodeList = new List<NodeVariables>();
                    NodeVariables node = new NodeVariables();
                    node.NodeName = "TaskData.Global.EsDevuelto";
                    node.NodeValue = respondeCorreo.Devuelta.ToString();
                    nodeList.Add(node);

                    ProcesosLegalesApiController obj = new ProcesosLegalesApiController();
                    respondeCorreo.Solicitud = obj.DevuelveCodSolicitud("0");

                    var incidente = UltimusManager.CompleteTask(respondeCorreo.UserId, respondeCorreo.TaskId, String.Empty, respondeCorreo.Nombre, nodeList);
                    confirm = (incidente > 0) ? 0 : -1;

                    if (respondeCorreo.ServicioLegal == 1)
                    {
                        confirm = GenerarIncidenteServiciosLegales(respondeCorreo);
                    }
                }
            }
            catch (Exception ex)
            {
                confirm = -1;
                UltimusLogs.Error(ex.StackTrace);
            }

            return confirm;
            
        }

        [HttpGet]
        public int AbortarComunicacion(string userID, string taskID)
        {
            try
            {
                List<NodeVariables> nodeList = new List<NodeVariables>();
                NodeVariables node = new NodeVariables();
                node.NodeName = "TaskData.Global.EsCorreoBasura";
                node.NodeValue = "1";
                nodeList.Add(node);

                return UltimusManager.CompleteTask(userID, taskID, String.Empty, String.Empty, nodeList) > 0 ? 1 : 0;
            }
            catch(Exception ex)
            {
                UltimusLogs.Error(ex.Message);
                return 0;
            }
        }

        [HttpGet]
        public List<Comunicacion> GetComunicacion() //trae los correos registrados
        {
            List<Comunicacion> lista = new List<Comunicacion>();
            DataTable dt = new DataTable();

            try
            {

                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_COMUNICACION.USP_SEL_COMUNICACION";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                foreach (DataRow rowProducto in dt.Rows)
                {
                    Comunicacion com = new Comunicacion();
                    com.Incidente = Convert.ToInt32(rowProducto["INCIDENTE"]);
                    com.IdCliente = Convert.IsDBNull(rowProducto["IDCLIENTE"]) ? 0 : Convert.ToInt32(rowProducto["IDCLIENTE"]);
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

                    com.Fecha_Resp = Convert.IsDBNull(rowProducto["FECHA_RESP"]) ? "" : Convert.ToDateTime(rowProducto["FECHA_RESP"]).ToString("dd-MMMM-yyyy hh:mm tt");
                    com.Correo_Infractor = rowProducto["CORREO_INFRACTOR"].ToString();
                    com.Respuesta = rowProducto["RESPUESTA"].ToString();
                    com.Abogado_Responde = rowProducto["ABOGADO_RESPONDE"].ToString();
                    com.Genera_Instruccion = Convert.IsDBNull(rowProducto["GENERA_INSTRUCCION"]) ? 0 : Convert.ToInt32(rowProducto["GENERA_INSTRUCCION"]);
                    lista.Add(com);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.StackTrace);
            }

            return lista;
        }

        [HttpGet]
        public List<Comunicacion> GetComunicacionByID(int incidenteID, string etapa) //busca un correo por Id de incidente
        {
            List<Comunicacion> lista = new List<Comunicacion>();
            DataTable dt = new DataTable();

            try
            {
                ULA.Quijano.ProcesoLegal.Commons.Crypt crypt = new ULA.Quijano.ProcesoLegal.Commons.Crypt();
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
                    com.Abogado_Asignado = rowProducto["ABOGADO_ASIGNADO"].ToString();
                    com.Abogado_Asignado_User = rowProducto["ABOGADO_ASIGNADO_USER"].ToString();
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
                    com.ParametroAdjuntos = HttpContext.Current.Server.UrlEncode(crypt.EncryptString("process=Comunicacion&incident=" + rowProducto["INCIDENTE"].ToString() + "&upload=0&delete=0&title=0&style=1&etapa=" + etapa));
                    com.ListaAdjuntos = rowProducto["LISTA_ADJUNTOS"].ToString();
                    com.CorreoCC = rowProducto["CORREO_CC"].ToString();
                    com.JustificacionDevolucion = rowProducto["JUSTIFICACION_DEVOLUCION"].ToString();
                    lista.Add(com);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.StackTrace);
            }

            return lista;
        }

        [HttpGet]
        public List<Comunicacion> GetComunicacionByIncidenteRelacionado(string incidenteRelacionado, string etapa) //busca un correo por Id de incidente relacionado
        {
            DataTable dt = new DataTable();
            List<Comunicacion> lista = new List<Comunicacion>();

            try
            {
                ULA.Quijano.ProcesoLegal.Commons.Crypt crypt = new ULA.Quijano.ProcesoLegal.Commons.Crypt();
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_COMUNICACION.USP_SEL_COMUNICACION_BY_INC_R";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE_RELACIONADO", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidenteRelacionado);
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                if (dt.Rows.Count > 0)
                {
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
                        com.ParametroAdjuntos = HttpContext.Current.Server.UrlEncode(crypt.EncryptString("process=Comunicacion&incident=" + rowProducto["INCIDENTE"].ToString() + "&upload=0&delete=0&title=0&style=1&etapa=" + etapa));
                        com.Etapa = Convert.IsDBNull(rowProducto["ETAPA"]) ? "" : rowProducto["ETAPA"].ToString();
                        lista.Add(com);
                    }                   

                    return lista;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.StackTrace);
                return null;
            }
        }

        [HttpGet]
        public Comunicacion GetComunicacionByIncidenteRelacionadoAndEtapa(string incidenteRelacionado, string etapa, int solicitud) //busca un correo por Id de incidente relacionado
        {
            DataTable dt = new DataTable();

            try
            {
                ULA.Quijano.ProcesoLegal.Commons.Crypt crypt = new ULA.Quijano.ProcesoLegal.Commons.Crypt();
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_COMUNICACION.USP_SEL_COMUNICACION_BY_ETAPA";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE_RELACIONADO", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidenteRelacionado);
                Procedimiento.AddParametros("P_ETAPA", OracleDbType.Varchar2, ParameterDirection.Input, 100, etapa);
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);              

                dt = DA.GetDataTable(Procedimiento);
                if (dt.Rows.Count > 0)
                {
                    DataRow rowProducto = dt.Rows[0];
                    Comunicacion com = new Comunicacion();
                    com.Incidente = Convert.IsDBNull(rowProducto["INCIDENTE"]) ? 0 : Convert.ToInt32(rowProducto["INCIDENTE"]);
                    com.IdCliente = Convert.IsDBNull(rowProducto["IDCLIENTE"]) ? 0 : Convert.ToInt32(rowProducto["IDCLIENTE"]);
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
                    com.Contenido = rowProducto["CONTENIDO"].ToString();
                    com.Fecha_Resp = Convert.IsDBNull(rowProducto["FECHA_RESP"]) ? "" : Convert.ToDateTime(rowProducto["FECHA_RESP"]).ToString("dd-MMMM-yyyy hh:mm tt");
                    com.Respuesta = rowProducto["RESPUESTA"].ToString();
                    com.Abogado_Responde = rowProducto["ABOGADO_RESPONDE"].ToString();
                    com.Genera_Instruccion = Convert.IsDBNull(rowProducto["GENERA_INSTRUCCION"]) ? 0 : Convert.ToInt32(rowProducto["GENERA_INSTRUCCION"]);
                    com.Etapa = rowProducto["ETAPA"].ToString();

                    return com;
                }
                else
                {
                    DatosCliente datosCliente = GetDatosCliente(incidenteRelacionado,solicitud);

                    if (datosCliente != null)
                    {
                        Comunicacion com = new Comunicacion();
                        com.IdCliente = datosCliente.Cod;
                        com.Nombre = datosCliente.Nombre;
                        com.Etapa = etapa;
                        com.Incidente_Relacionado = incidenteRelacionado;
                        return com;
                    }
                    else
                    {
                        return null;
                    }

                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.StackTrace);
                return null;
            }
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [HttpPost]
        public List<string> EnviarCorreo(string Attach, string Body, string Domain, string FromEmail, bool IsBodyHtml, bool IsSend, string MailTo, string Password, int SmtpPort,
                                    string SmtpServer, string Subject, string UserName)
        {
            List<string> result = new List<string>();
            try
            {
                /*
                WCFSendMail.SendMail sm = new WCFSendMail.SendMail();
                sm.EnviarCorreo(Attach, Body, Domain, FromEmail, IsBodyHtml, IsSend, MailTo, Password, SmtpPort,
                                    SmtpServer, Subject, UserName);
                */
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.StackTrace);
            }

            return result;
        }


        [HttpGet]
        public List<TipoFacturacion> GetTiposFacturacion()
        {

            List<TipoFacturacion> lista = new List<TipoFacturacion>();
            DataTable dt = new DataTable();

            try
            {

                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOSFACTURACION.USP_SEL_TIPO_FACTURACION";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                foreach (DataRow rowProducto in dt.Rows)
                {
                    TipoFacturacion tFac = new TipoFacturacion();
                    tFac.COD_FACTURACION = rowProducto["COD_FACTURACION"].ToString();
                    tFac.DESCRIPCION = rowProducto["DESCRIPCION"].ToString();
                    lista.Add(tFac);
                }             

            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
        public DATOS_FACTURACION GetDatosFacturacion(string incidente, int solicitud)
        {
            try
            {
                DATOS_FACTURACION d = null;
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOSFACTURACION.USP_SEL_DATOS_FACTURACION";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, solicitud.ToString());

                DataTable dt = DA.GetDataTable(Procedimiento);

                if (dt.Rows.Count > 0)
                {
                        d = new DATOS_FACTURACION();
                        d.INCIDENTE = incidente;
                        d.COD_CLIENTE = dt.Rows[0]["COD_CLIENTE"].ToString();
                        d.DIRECCION_FACTURA = dt.Rows[0]["DIRECCION_FACTURA"].ToString();
                        //d.FORMA_ENVIO = Convert.ToInt16(dt.Rows[0]["FORMA_ENVIO"].ToString());
                        d.FORMA_ENVIO_CURRIER = dt.Rows[0]["FORMA_ENVIO_CURRIER"].ToString();
                        d.FORMA_ENVIO_EMAIL = dt.Rows[0]["FORMA_ENVIO_EMAIL"].ToString();
                        d.FORMA_ENVIO_PERSONAL = dt.Rows[0]["FORMA_ENVIO_PERSONAL"].ToString();
                        d.FORMA_ENVIO_MENSAJERIA = dt.Rows[0]["FORMA_ENVIO_MENSAJERIA"].ToString();
                        d.DIRECCION_ENVIO = dt.Rows[0]["DIRECCION_ENVIO"].ToString();
                        d.FORMA_PAGO = dt.Rows[0]["FORMA_PAGO"].ToString();
                        d.LEYENDA_FACTURA = dt.Rows[0]["LEYENDA_FACTURA"].ToString();
                        d.NOMBRE_FACTURA = dt.Rows[0]["NOMBRE_FACTURA"].ToString();
                        d.NUMERO_GUIA = Convert.ToDecimal(dt.Rows[0]["NUMERO_GUIA"].ToString());
                        d.NUMERO_SOLICITUD = Convert.ToDecimal(dt.Rows[0]["NUMERO_SOLICITUD"].ToString());
                        d.PROFORMA = Convert.ToInt16(dt.Rows[0]["PROFORMA"].ToString());
                        d.REFERENCIA = dt.Rows[0]["REFERENCIA"].ToString();
                        d.RESPONSABLE_NOTIF = dt.Rows[0]["RESPONSABLE_NOTIF"].ToString();
                        d.TIPO_FACTURA = dt.Rows[0]["TIPO_FACTURA"].ToString();
                        d.TIPO_FACTURACION = dt.Rows[0]["TIPO_FACTURACION"].ToString();
                        d.MONEDA = dt.Rows[0]["MONEDA"].ToString();
                        d.HONORARIOS = dt.Rows[0]["HONORARIOS"].ToString();
                        d.GASTOS = dt.Rows[0]["GASTOS"].ToString();
                        d.TOTAL = dt.Rows[0]["TOTAL"].ToString();
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
        public bool RegistrarDatosFacturacion(DATOS_FACTURACION parametros)
        {
            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOSFACTURACION.USP_INS_DATOSFACTURACION";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 50, parametros.INCIDENTE);
                Procedimiento.AddParametros("P_COD_CLIENTE", OracleDbType.Varchar2, ParameterDirection.Input, 20, parametros.COD_CLIENTE);
                Procedimiento.AddParametros("P_TIPO_FACTURACION", OracleDbType.Varchar2, ParameterDirection.Input, 255, parametros.TIPO_FACTURACION);
                Procedimiento.AddParametros("P_FORMA_PAGO", OracleDbType.Varchar2, ParameterDirection.Input, 10, parametros.FORMA_PAGO);
                Procedimiento.AddParametros("P_PROFORMA", OracleDbType.Int32, ParameterDirection.Input, 255, parametros.PROFORMA.ToString());
                Procedimiento.AddParametros("P_TIPO_FACTURA", OracleDbType.Varchar2, ParameterDirection.Input, 10, parametros.TIPO_FACTURA);
                Procedimiento.AddParametros("P_REFERENCIA", OracleDbType.Varchar2, ParameterDirection.Input, 100, parametros.REFERENCIA);
                Procedimiento.AddParametros("P_NOMBRE_FACTURA", OracleDbType.Varchar2, ParameterDirection.Input, 100, parametros.NOMBRE_FACTURA);
                Procedimiento.AddParametros("P_DIRECCION_FACTURA", OracleDbType.Varchar2, ParameterDirection.Input, 200, parametros.DIRECCION_FACTURA);
                Procedimiento.AddParametros("P_LEYENDA_FACTURA", OracleDbType.Varchar2, ParameterDirection.Input, 1250, parametros.LEYENDA_FACTURA);
                Procedimiento.AddParametros("P_RESPONSABLE_NOTIF", OracleDbType.Varchar2, ParameterDirection.Input, 10, parametros.RESPONSABLE_NOTIF);
                Procedimiento.AddParametros("P_FORMA_ENVIO_CURRIER", OracleDbType.Varchar2, ParameterDirection.Input, 1, parametros.FORMA_ENVIO_CURRIER);
                Procedimiento.AddParametros("P_FORMA_ENVIO_EMAIL", OracleDbType.Varchar2, ParameterDirection.Input, 1, parametros.FORMA_ENVIO_EMAIL);
                Procedimiento.AddParametros("P_FORMA_ENVIO_PERSONAL", OracleDbType.Varchar2, ParameterDirection.Input, 1, parametros.FORMA_ENVIO_PERSONAL);
                Procedimiento.AddParametros("P_FORMA_ENVIO_MENSAJERIA", OracleDbType.Varchar2, ParameterDirection.Input, 1, parametros.FORMA_ENVIO_MENSAJERIA);
                Procedimiento.AddParametros("P_DIRECCION_ENVIO", OracleDbType.Varchar2, ParameterDirection.Input, 255, parametros.DIRECCION_ENVIO);
                Procedimiento.AddParametros("P_NUMERO_SOLICITUD", OracleDbType.Decimal, ParameterDirection.Input, 255, parametros.NUMERO_SOLICITUD.ToString());
                Procedimiento.AddParametros("P_NUMERO_GUIA", OracleDbType.Decimal, ParameterDirection.Input, 255, parametros.NUMERO_GUIA.ToString());
                Procedimiento.AddParametros("P_MONEDA", OracleDbType.Varchar2, ParameterDirection.Input, 20, string.IsNullOrEmpty(parametros.MONEDA) ? "" : parametros.MONEDA);
                Procedimiento.AddParametros("P_HONORARIOS", OracleDbType.Varchar2, ParameterDirection.Input, 20, string.IsNullOrEmpty(parametros.HONORARIOS) ? "" : parametros.HONORARIOS);
                Procedimiento.AddParametros("P_GASTOS", OracleDbType.Varchar2, ParameterDirection.Input, 20, string.IsNullOrEmpty(parametros.GASTOS) ? "" : parametros.GASTOS);
                Procedimiento.AddParametros("P_TOTAL", OracleDbType.Varchar2, ParameterDirection.Input, 20, string.IsNullOrEmpty(parametros.TOTAL) ? "" : parametros.TOTAL);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, parametros.Solicitud.ToString());

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
        public bool RegistrarDatosCliente(DatosCliente datosCliente)
        {
            try
            {
                Boolean ret = false;
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_CLIENTE.USP_UPD_DATOS_CLIENTE";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, datosCliente.Incidente);
                Procedimiento.AddParametros("P_CLIENTE_EXISTENTE", OracleDbType.Varchar2, ParameterDirection.Input, 1, datosCliente.ClienteExistente);
                Procedimiento.AddParametros("P_COD", OracleDbType.Decimal, ParameterDirection.Input, 255, datosCliente.Cod.ToString());
                Procedimiento.AddParametros("P_NOMBRE", OracleDbType.Varchar2, ParameterDirection.Input, 60, datosCliente.Nombre);
                Procedimiento.AddParametros("P_ESTADO_CUMPLIMIENTO", OracleDbType.Varchar2, ParameterDirection.Input, 1, datosCliente.EstadoCumplimiento);
                Procedimiento.AddParametros("P_NIVEL_RIESGO", OracleDbType.Varchar2, ParameterDirection.Input, 2, datosCliente.NivelRiesgo);
                Procedimiento.AddParametros("P_PUNTAJE_RIESGO", OracleDbType.Decimal, ParameterDirection.Input, 255, datosCliente.PuntajeRiesgo.ToString());
                Procedimiento.AddParametros("P_PASAPORTE", OracleDbType.Varchar2, ParameterDirection.Input, 25, datosCliente.Pasaporte);
                Procedimiento.AddParametros("P_IDIOMA", OracleDbType.Varchar2, ParameterDirection.Input, 25, datosCliente.Idioma);
                Procedimiento.AddParametros("P_TELEFONO_CASA", OracleDbType.Varchar2, ParameterDirection.Input, 60, datosCliente.TelefonoCasa);
                Procedimiento.AddParametros("P_PAIS", OracleDbType.Varchar2, ParameterDirection.Input, 4, datosCliente.Pais);
                Procedimiento.AddParametros("P_MONEDA", OracleDbType.Varchar2, ParameterDirection.Input, 50, datosCliente.Moneda);
                Procedimiento.AddParametros("P_OCUPACION", OracleDbType.Varchar2, ParameterDirection.Input, 100, datosCliente.Ocupacion);
                Procedimiento.AddParametros("P_CELULAR", OracleDbType.Varchar2, ParameterDirection.Input, 21, datosCliente.Celular);
                Procedimiento.AddParametros("P_EMAIL", OracleDbType.Varchar2, ParameterDirection.Input, 200, datosCliente.Email);
                Procedimiento.AddParametros("P_SECTOR_ECONOMICO", OracleDbType.Decimal, ParameterDirection.Input, 255, datosCliente.SectorEconomico.ToString());
                Procedimiento.AddParametros("P_PEP", OracleDbType.Varchar2, ParameterDirection.Input, 1, datosCliente.Pep);
                Procedimiento.AddParametros("P_TIPO_PERSONA", OracleDbType.Varchar2, ParameterDirection.Input, 2, datosCliente.TipoPersona);
                Procedimiento.AddParametros("P_CLASIFICACION_CLIENTE", OracleDbType.Varchar2, ParameterDirection.Input, 5, datosCliente.ClasificacionCliente);
                Procedimiento.AddParametros("P_REQ_FORMULARIO", OracleDbType.Varchar2, ParameterDirection.Input, 1, datosCliente.ReqFormulario ? "S" : "N");
                Procedimiento.AddParametros("P_REQ_PASAPORTE", OracleDbType.Varchar2, ParameterDirection.Input, 1, datosCliente.ReqPasaporte ? "S" : "N");
                Procedimiento.AddParametros("P_REQ_REFERENCIA", OracleDbType.Varchar2, ParameterDirection.Input, 1, datosCliente.ReqReferencia ? "S" : "N");
                Procedimiento.AddParametros("P_REQ_PROFESIONAL", OracleDbType.Varchar2, ParameterDirection.Input, 1, datosCliente.ReqProfesional ? "S" : "N");
                Procedimiento.AddParametros("P_REQ_DOMICILIO", OracleDbType.Varchar2, ParameterDirection.Input, 1, datosCliente.ReqDomicilio ? "S" : "N");
                Procedimiento.AddParametros("P_REQ_INCORP", OracleDbType.Varchar2, ParameterDirection.Input, 1, datosCliente.ReqIncorp ? "S" : "N");
                Procedimiento.AddParametros("P_REQ_PROMOCIONALES", OracleDbType.Varchar2, ParameterDirection.Input, 1, datosCliente.ReqPromocionales ? "S" : "N");
                Procedimiento.AddParametros("P_REQ_MEMBRESIA", OracleDbType.Varchar2, ParameterDirection.Input, 1, datosCliente.ReqMembresia ? "S" : "N");
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, datosCliente.Solicitud.ToString());
                Procedimientos.Add(Procedimiento);
                ret = DA.EjecutarProcedimientos(Procedimientos);

                if (ret)
                {
                    if (datosCliente.LibretaDirecciones != null)
                        foreach (var ld in datosCliente.LibretaDirecciones)
                        {
                            Procedimiento = new OracleStoredName();
                            Procedimiento.Nombre = "PAQ_LIBRETA_DIRECCIONES.USP_INS_LIBRETADIRECCIONES";
                            Procedimiento.IsPadre = Valores.IsNotPadre;
                            Procedimiento.IsHereda = false;
                            Procedimiento.AddParametros("P_NOMBRE_FACTURA", OracleDbType.Varchar2, ParameterDirection.Input, 100, ld.NombreFactura);
                            Procedimiento.AddParametros("P_CODIGO_DIRECCION", OracleDbType.Varchar2, ParameterDirection.Input, 100, ld.CodDireccion);
                            Procedimiento.AddParametros("P_DIRECCION", OracleDbType.Varchar2, ParameterDirection.Input, 50, ld.Direccion);
                            Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, datosCliente.Incidente);
                            Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Varchar2, ParameterDirection.Input, 255, datosCliente.Solicitud.ToString());
                            Procedimientos.Clear();
                            Procedimientos.Add(Procedimiento);
                            ret = DA.EjecutarProcedimientos(Procedimientos);

                            if (!ret)
                                break;
                        }
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
        public DatosCliente ObtenerDatosCliente(string incidente, int solicitud)
        {
            DatosCliente datosCliente = ObtenerDatosClienteByComunicacion(incidente, "GestionaCorreo",solicitud);
            if (datosCliente == null)
            {
                datosCliente = GetDatosCliente(incidente,solicitud);
            }

            return datosCliente;
        }

        [HttpGet]
        public DatosCliente ObtenerDatosClienteByComunicacion(string incidente, string etapa, int solicitud)
        {
            try
            {
                DatosCliente datosCliente = null;
                Comunicacion comunicacion = ObtenerComunicacionForServicioLegales(incidente, etapa);

                if (comunicacion != null)
                {
                    if (comunicacion.IdCliente > 0)
                    {
                        datosCliente = GetDatosCliente(incidente,solicitud);

                        if (!datosCliente.ExistBD)
                        {
                            datosCliente = GetClienteById((int)comunicacion.IdCliente);
                            datosCliente.Incidente = incidente;
                        }

                        datosCliente.ClienteExistente = "S";
                    }
                }

                return datosCliente;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }
        }

        public Comunicacion ObtenerComunicacionForServicioLegales(string incidente, string etapa)
        {
            try
            {
                Comunicacion comunicacion = null;
                List<Comunicacion> lista = GetComunicacionByIncidenteRelacionado(incidente, etapa);

                if (lista != null)
                {
                    if (lista.Count == 1)
                    {
                        comunicacion = lista[0];
                    }
                    else if (lista.Count > 1)
                    {
                        foreach (Comunicacion com in lista)
                        {
                            if (com.Etapa == "" || com.Etapa == "GestionaCorreo")
                            {
                                comunicacion = com;
                                break;
                            }
                        }
                    }
                }

                return comunicacion;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }
        }

        [HttpGet]
        public DatosCliente GetDatosCliente(string incidente, int solicitud)
        {
            try
            {
                DatosCliente datosCliente = null;
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_CLIENTE.USP_SEL_DATOS_CLIENTE";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, solicitud.ToString());

                DataTable dt = DA.GetDataTable(Procedimiento);

                if (dt.Rows.Count > 0)
                {
                    //Si el Cliente existe lo obtengo de GENSYS
                    if (dt.Rows[0]["CLIENTE_EXISTENTE"].ToString().Equals("S"))
                    {
                        datosCliente = GetClienteById(Convert.ToInt32(dt.Rows[0]["COD"]));
                        datosCliente.ClienteExistente = dt.Rows[0]["CLIENTE_EXISTENTE"].ToString();
                        datosCliente.Incidente = incidente;
                        datosCliente.Moneda = dt.Rows[0]["MONEDA"].ToString();
                    }
                    //Sino lo busco en DATOSLEGALES
                    else
                    {
                        datosCliente = new DatosCliente()
                        {
                            Incidente = incidente,
                            ClienteExistente = dt.Rows[0]["CLIENTE_EXISTENTE"].ToString(),
                            Cod = Convert.IsDBNull(dt.Rows[0]["COD"]) ? 0 : Convert.ToInt32(dt.Rows[0]["COD"]),
                            Nombre = dt.Rows[0]["NOMBRE"].ToString(),
                            EstadoCumplimiento = dt.Rows[0]["ESTADO_CUMPLIMIENTO"].ToString(),
                            NivelRiesgo = dt.Rows[0]["NIVEL_RIESGO"].ToString(),
                            PuntajeRiesgo = Convert.IsDBNull(dt.Rows[0]["PUNTAJE_RIESGO"]) ? 0 : Convert.ToDecimal(dt.Rows[0]["PUNTAJE_RIESGO"]),
                            Pasaporte = dt.Rows[0]["PASAPORTE"].ToString(),
                            Idioma = dt.Rows[0]["IDIOMA"].ToString(),
                            TelefonoCasa = dt.Rows[0]["TELEFONO_CASA"].ToString(),
                            Pais = dt.Rows[0]["PAIS"].ToString(),
                            Moneda = dt.Rows[0]["MONEDA"].ToString(),
                            Ocupacion = dt.Rows[0]["OCUPACION"].ToString(),
                            Celular = dt.Rows[0]["CELULAR"].ToString(),
                            Email = dt.Rows[0]["EMAIL"].ToString(),
                            SectorEconomico = Convert.IsDBNull(dt.Rows[0]["SECTOR_ECONOMICO"]) ? 0 : Convert.ToDecimal(dt.Rows[0]["SECTOR_ECONOMICO"]),
                            Pep = dt.Rows[0]["PEP"].ToString(),
                            TipoPersona = dt.Rows[0]["TIPO_PERSONA"].ToString(),
                            ClasificacionCliente = dt.Rows[0]["CLASIFICACION_CLIENTE"].ToString(),
                            ReqFormulario = dt.Rows[0]["REQ_FORMULARIO"].ToString() == "S",
                            ReqPasaporte = dt.Rows[0]["REQ_PASAPORTE"].ToString() == "S",
                            ReqReferencia = dt.Rows[0]["REQ_REFERENCIA"].ToString() == "S",
                            ReqProfesional = dt.Rows[0]["REQ_PROFESIONAL"].ToString() == "S",
                            ReqDomicilio = dt.Rows[0]["REQ_DOMICILIO"].ToString() == "S",
                            ReqIncorp = dt.Rows[0]["REQ_INCORP"].ToString() == "S",
                            ReqPromocionales = dt.Rows[0]["REQ_PROMOCIONALES"].ToString() == "S",
                            ReqMembresia = dt.Rows[0]["REQ_MEMBRESIA"].ToString() == "S"
                        };

                        datosCliente.Monedas = GetMonedas();
                        datosCliente.LibretaDirecciones = new List<LibretaDireccion>();

                        Procedimiento = new OracleStoredName();
                        Procedimiento.Nombre = "PAQ_LIBRETA_DIRECCIONES.USP_SEL_LIBRETADIRECCIONES";
                        Procedimiento.IsPadre = Valores.IsNotPadre;
                        Procedimiento.IsHereda = false;
                        Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                        Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);
                        Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, solicitud.ToString());

                        DataTable dtld = DA.GetDataTable(Procedimiento);

                        if (dtld.Rows.Count > 0)
                        {
                            foreach (DataRow ld in dtld.Rows)
                            {
                                datosCliente.LibretaDirecciones.Add(
                                    new LibretaDireccion()
                                    {
                                        CodDireccion = ld["CODIGO_DIRECCION"].ToString(),
                                        Direccion = ld["DIRECCION"].ToString(),
                                        NombreFactura = ld["NOMBRE_FACTURA"].ToString()
                                    });
                            }
                        }
                    }

                    datosCliente.ExistBD = true;
                }
                else
                {
                    datosCliente = new DatosCliente();
                    datosCliente.ExistBD = false;
                }

                return datosCliente;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }
        }

        [HttpGet]
        public Asunto ObtenerDatosAsunto(string incidente, int solicitud)
        {
            try
            {
                Asunto asunto = null;
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_ASUNTO.USP_SEL_SERVICIOS_LEGALES";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, solicitud.ToString());

                DataTable dt = DA.GetDataTable(Procedimiento);

                if (dt.Rows.Count > 0)
                {
                    //Si el Cliente exite lo obtengo de GENSYS
                    if (!Convert.IsDBNull(dt.Rows[0]["NRO_ASUNTO"]))
                    {
                        asunto = GetAsuntoById(Convert.ToInt32(dt.Rows[0]["NRO_ASUNTO"]));
                    }
                    
                }

                return asunto;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }
        }

        [HttpPost]
        public bool RegistrarAsunto(Asunto asunto)
        {
            try
            {
                Boolean ret = false;
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_ASUNTO.USP_UPD_DATOS_ASUNTO";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 50, asunto.Incidente);
                Procedimiento.AddParametros("P_NRO_ASUNTO", OracleDbType.Decimal, ParameterDirection.Input, 20, asunto.NroAsunto.ToString());
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, asunto.Solicitud.ToString());
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

        [HttpPost]
        public bool EliminarAdjuntos(EliminarAdjunto eliminarAdjunto)
        {
            bool ret = false;
            try
            {
                AttachmentServiceClient client = new AttachmentServiceClient();
                ret = client.DeleteFiles(eliminarAdjunto.Process, "_tmp_" + eliminarAdjunto.UserId.Replace("/", "_"));
                return ret;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.StackTrace);
            }

            return ret;
        }

        [HttpGet]
        public List<IncidenteActivo> GetIncidentesActivosByCliente(int IdCliente)
        {
            List<IncidenteActivo> lista = new List<IncidenteActivo>();
            DataTable dt = null;
            DataTable dtUltimus = null;

            try
            {

                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_INCIDENTES.USP_SEL_INCIDENTES_BY_CLIENTE";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_ID_CLIENTE", OracleDbType.Int32, ParameterDirection.Input, 100, IdCliente.ToString());
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);

                if (dt.Rows.Count > 0)
                {
                    DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringULTIMUS"];
                    foreach (DataRow row in dt.Rows)
                    {
                        string query = "SELECT INCIDENT, SUMMARY " +
                                        "FROM INCIDENTS " +
                                        "WHERE INCIDENT = " + row["INCIDENTE"].ToString() +
                                        " AND PROCESSNAME = 'ServiciosLegales'" +
                                        " AND STATUS = 1";

                        var ret = DA.GetDataTable(query, out dtUltimus);
                        if (ret && dtUltimus.Rows.Count > 0)
                        {
                            lista.Add(new IncidenteActivo()
                            {
                                NroIncidente = row["INCIDENTE"].ToString(),
                                Sumario = row["NOMBRE"].ToString()
                            });
                        }

                        //using (UltimusIntegrationClient client = new UltimusIntegrationClient())
                        //{
                        //    UltimusNativeIncident uni;
                        //    string error;
                        //    bool ret = client.GetUltimusNativeIncident("ServiciosLegales", Convert.ToInt32(row["INCIDENTE"]), out uni, out error);

                        //    if (ret && uni.nIncidentStatus == 1)
                        //    {
                        //        lista.Add(new IncidenteActivo()
                        //        {
                        //            NroIncidente = row["INCIDENTE"].ToString(),
                        //            Sumario = row["NOMBRE"].ToString()
                        //        });
                        //    }
                        //}
                    }
                }                
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;            
        }

        [HttpGet]
        public RegistroSolicitud GetDatosByIncidente(string incidente)
        {
            try
            {
                RegistroSolicitud registroSolicitud = new RegistroSolicitud();
                List<Comunicacion> comunicaciones = GetComunicacionByID(Convert.ToInt32(incidente), "RespondeCorreo");

                if (comunicaciones != null && comunicaciones.Count > 0)
                {
                    registroSolicitud.Abogado = comunicaciones[0].Abogado_Asignado_User;
                    registroSolicitud.IdArea = comunicaciones[0].CodArea;
                    registroSolicitud.Idioma = comunicaciones[0].CodIdioma;
                    registroSolicitud.IdJurisdiccion = comunicaciones[0].CodJurisdiccion;
                    registroSolicitud.IdProcedencia = comunicaciones[0].CodProcedencia;
                    registroSolicitud.IdTramite = comunicaciones[0].CodTramite;
                }

                return registroSolicitud;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }
        }

        [HttpGet]
        public List<Asunto> BuscarAsuntosById(string id)
        {
            List<Asunto> lista = new List<Asunto>();
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query = "SELECT TRVAS.ASUNTO, " +
                                      "(SELECT NVL(CCV61.NOMBRE, '') FROM CCV61 WHERE CCV61.CLIENTE = TRVAS.CLIENTE) AS NOMBRE_CLIENTE, " +
                                      "TRVAS.ESTATUS " +
                                "FROM TRVAS " +
                                "WHERE TRVAS.ASUNTO LIKE '%" + id + "%' " +
                                "ORDER BY TRVAS.ASUNTO";

                DA.GetDataTable(query, out dt);

                foreach (DataRow rowProducto in dt.Rows)
                {
                    Asunto asunto = new Asunto();
                    asunto.NroAsunto = Convert.ToInt32(rowProducto["ASUNTO"]);
                    asunto.NombreCliente = rowProducto["NOMBRE_CLIENTE"].ToString();
                    asunto.Estado = GetDescriptionByEstadoAsunto(rowProducto["ESTATUS"].ToString());
                    lista.Add(asunto);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
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
                UltimusLogs.Error(ex.ToString());
                return false;
            }
        }

        public int GenerarIncidenteServiciosLegales(RespondeCorreo respondeCorreo)
        {
            try
            {
                List<Comunicacion> temp = GetComunicacionByID(respondeCorreo.Incidente, "");

                if (temp.Count == 0)
                    return -1;

                if (!string.IsNullOrEmpty(temp[0].Incidente_Relacionado) && temp[0].Incidente_Relacionado != "0")
                    return Convert.ToInt32(temp[0].Incidente_Relacionado);

                using (UltimusIntegrationClient client = new UltimusIntegrationClient())
                {
                    int incidenteRelacionado;
                    string error;
                    ULA.Quijano.ProcesoLegal.UltimusIntegrationLayer.UltimusIncident ui;
                    client.GetTaskByFilters(respondeCorreo.UserId, "ServiciosLegales", "RegistrarSolicitud", 0, out ui, out error);

                    if (ui == null)
                        return -1;

                    string summary = String.Empty;
                    switch (temp[0].CodArea)
                    {
                        case 1: 
                            summary = "SOCIEDADES - " + temp[0].Nombre;
                            break;
                        case 2: 
                            summary = "FUNDACIONES - " + temp[0].Nombre;
                            break;
                        case 3:
                            summary = "MIGRACIÓN - " + temp[0].Nombre;
                            break;
                        case 5:
                            summary = "NAVES - " + temp[0].Nombre;
                            break;
                        default: 
                            summary = temp[0].Nombre;
                            break;
                    }

                    List<NodeVariables> nodeList = new List<NodeVariables>();
                    NodeVariables node = new NodeVariables();
                    node.NodeName = "TaskData.Global.CodAreaLegal";
                    node.NodeValue = temp[0].CodArea;
                    nodeList.Add(node);

                    NodeVariables node2 = new NodeVariables();
                    node2.NodeName = "TaskData.Global.CodigoSolicitud";
                    node2.NodeValue = respondeCorreo.Solicitud.ToString();
                    nodeList.Add(node2);

                    //client.CompleteTask(ui.User, ui.TaskId, string.Empty, summary, string.Empty, out incidenteRelacionado, out error);
                    client.CompleteTaskWithVariables(ui.User, ui.TaskId, string.Empty, summary, nodeList, out incidenteRelacionado, out error);
                    
                    if (!string.IsNullOrEmpty(error) || incidenteRelacionado == 0)
                        return -1;

                    return (UpdateComunicacionIncidenteRelacionado(respondeCorreo.Incidente, incidenteRelacionado) ? incidenteRelacionado : -1);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return -1;
            }
        }


        [HttpPost]
        public bool DevolverCorreo(DevolverCorreo devolverCorreo)
        {
            bool confirm = false;
            try
            {
                //Actualiza en la base de datos
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                ICollection<OracleStoredName> Procedimientos = new List<OracleStoredName>();
                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_COMUNICACION.USP_UDP_JUSTIFICACION";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Int32, ParameterDirection.Input, 255, devolverCorreo.Incidente.ToString());
                Procedimiento.AddParametros("P_JUSTIFICACION_DEVOLUCION", OracleDbType.Varchar2, ParameterDirection.Input, 1000, devolverCorreo.Justificacion);

                Procedimientos.Add(Procedimiento);
                confirm = DA.EjecutarProcedimientos(Procedimientos);

                if (confirm)
                {
                    List<NodeVariables> nodeList = new List<NodeVariables>();
                    NodeVariables node;

                    node = new NodeVariables();
                    node.NodeName = "TaskData.Global.EsDevuelto";
                    node.NodeValue = "1";
                    nodeList.Add(node);

                    string summary = devolverCorreo.Nombre;
                    var incidente = UltimusManager.CompleteTask(devolverCorreo.UserId, devolverCorreo.TaskId, String.Empty, summary, nodeList);
                    confirm = incidente > 0;
                }

            }
            catch (Exception ex)
            {
                confirm = false;
                UltimusLogs.Error(ex.StackTrace);
            }

            return confirm;

        }


        [HttpGet]
        public List<CatalogoTipoSolicitudMarca> GetTipoSolicitudMarca()
        {
            List<CatalogoTipoSolicitudMarca> lista = new List<CatalogoTipoSolicitudMarca>();
            DataTable dt = new DataTable();

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_MARCA.USP_SEL_TIPO_SOLICITUD_MARCA";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                foreach (DataRow rowProducto in dt.Rows)
                {
                    CatalogoTipoSolicitudMarca are = new CatalogoTipoSolicitudMarca();
                    are.CodTipoSolicitudMarca = Convert.ToInt32(rowProducto["IDTIPO"]);
                    are.Descripcion = rowProducto["DESCRIPCION"].ToString();
                    lista.Add(are);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
        public List<CatalogoTipoRegistro> GetTipoRegistro()
        {
            List<CatalogoTipoRegistro> lista = new List<CatalogoTipoRegistro>();
            DataTable dt = new DataTable();

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_MARCA.USP_SEL_TIPO_REGISTRO";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);

                dt = DA.GetDataTable(Procedimiento);
                foreach (DataRow rowProducto in dt.Rows)
                {
                    CatalogoTipoRegistro are = new CatalogoTipoRegistro();
                    are.CodTipoRegistro = Convert.ToInt32(rowProducto["IDTIPOREGISTRO"]);
                    are.Descripcion = rowProducto["DESCRIPCION"].ToString();
                    lista.Add(are);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }


        #endregion

        #region PRIVATE

        private string GetDescriptionByEstadoAsunto(string estado)
        {
            switch (estado)
            {
                case "A": return "ABIERTO";
                case "P": return "PENDIENTE";
                case "T": return "TERMINADO";
                case "X": return "ELIMINADO";
                default: return "ABIERTO";
            }
        } 

        private string GetDescriptionByTipoES(string tipo)
        {
            switch (tipo)
            {
                case "1": return "Notaría";
                case "2": return "Traducción";
                case "3": return "Transporte";
                case "4": return "Sellos";
                case "5": return "Captación";
                default: return "";
            }
        }

        private string GetDescriptionByEstadoES(string estado)
        {
            switch (estado)
            {
                case "1": return "Pendiente de Asignación";
                case "2": return "Asignada";
                case "3": return "En Curso";
                case "4": return "Rechazada para Cambio";
                case "5": return "Completada";
                case "6": return "Rechazada";
                case "7": return "Traspasada";
                case "8": return "Suspendida";
                case "9": return "Reasignada";
                default: return "";
            }
        }

        #endregion

        [HttpGet]
        public string EnviarTramite(string usr, string taskId, string retornoAbogado)
        {
            UltimusLogs.Trace(String.Format("Usuario:{0}, taskId={1}, retornoAbogado={2}", usr, taskId, retornoAbogado));
            try
            {
                List<NodeVariables> Variable = new List<NodeVariables>();
                Variable.Add(new NodeVariables()
                {
                    NodeName = "TaskData.Global.RetornoAbogado",
                    NodeValue = retornoAbogado
                });

                UltimusManager.CompleteTask(usr, taskId, null, null, Variable);
                return "EXITOSO";
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return "ERROR";
            }
        }
    }
}