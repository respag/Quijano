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
    public class DatosNaveController : ApiController
    {
        #region LOGGER DEFINITION

        UltimusLogs UltimusLogs = new UltimusLogs("DatosNaveController");

        #endregion

        [HttpGet]
        public List<DatosNave> BuscarNavesById(string id)
        {
            List<DatosNave> lista = new List<DatosNave>();
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query = "SELECT NVV09.CODNAVE, NVV09.NOMBRE " +
                                 "FROM NVV09 " +
                                "WHERE NVV09.CODNAVE LIKE '%" + id + "%' " + 
                                "ORDER BY NVV09.NOMBRE";

                DA.GetDataTable(query, out dt);

                foreach (DataRow rowProducto in dt.Rows)
                {
                    DatosNave nave = new DatosNave();
                    nave.CodNave = rowProducto["CODNAVE"].ToString();
                    nave.NombreNave = rowProducto["NOMBRE"].ToString();
                    lista.Add(nave);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
        public List<DatosNave> BuscarNavesByName(string nombre)
        {
            List<DatosNave> lista = new List<DatosNave>();
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query = "SELECT NVV09.CODNAVE, NVV09.NOMBRE " +
                                 "FROM NVV09 " +
                                "WHERE NVV09.NOMBRE LIKE '%" + nombre.ToUpper() + "%' " +
                                "ORDER BY NVV09.NOMBRE";

                DA.GetDataTable(query, out dt);

                foreach (DataRow rowProducto in dt.Rows)
                {
                    DatosNave nave = new DatosNave();
                    nave.CodNave = rowProducto["CODNAVE"].ToString();
                    nave.NombreNave = rowProducto["NOMBRE"].ToString();
                    lista.Add(nave);
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return lista;
        }

        [HttpGet]
        public DatosNave GetNaveById(string id)
        {
            DatosNave datosNave = null;
            DataTable dt = null;
            DataTable dt2 = null;
            DataTable dt3 = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string consultaNombres = "Select Nombre from CCV61 where cliente = {0}";

                string query = "SELECT NVV09.*, " +
                                      "(SELECT GSVCIAS.NOMBRECIA FROM GSVCIAS WHERE GSVCIAS.CODCIA = NVV09.CODCIA) AS DESCRIPCION_CIA " +
                               "FROM NVV09 " +
                               "WHERE CODNAVE = '" + id + "'";

                DA.GetDataTable(query, out dt);
                string IdCorresponsal1 = dt.Rows[0]["CLIENTE"].ToString() == "NULL" ? "" : dt.Rows[0]["CLIENTE"].ToString();
                string IdCorresponsal2 = dt.Rows[0]["CLIENTERESP"].ToString() == "NULL" ? "" : dt.Rows[0]["CLIENTERESP"].ToString();
                
                string query2 = string.Format(consultaNombres, IdCorresponsal1);
                string query3 = string.Format(consultaNombres, IdCorresponsal2);
                DA.GetDataTable(query2, out dt2);
                DA.GetDataTable(query3, out dt3);
                string NombreCorresponsal = dt2.Rows[0]["NOMBRE"].ToString() == "NULL" ? "" : dt2.Rows[0]["NOMBRE"].ToString();
                string NombreClienteCorresponsal = dt3.Rows[0]["Nombre"].ToString() == "NULL" ? "" : dt3.Rows[0]["Nombre"].ToString();
                if (dt.Rows.Count > 0)
                {
                    datosNave = new DatosNave()
                    {
                        NaveExistente = "1",
                        CodNave = dt.Rows[0]["CODNAVE"].ToString(),
                        NombreNave = dt.Rows[0]["NOMBRE"].ToString(),
                        CodCia = dt.Rows[0]["CODCIA"].ToString(),
                        NombreCia = dt.Rows[0]["DESCRIPCION_CIA"].ToString(),
                        NombreAnterior = dt.Rows[0]["NOMBREANTERIOR"].ToString(),
                        SocPropietaria = dt.Rows[0]["CODSOCIEDAD"].ToString() == "NULL" ? "" : dt.Rows[0]["CODSOCIEDAD"].ToString(),
                        Corresponsal = dt.Rows[0]["CLIENTE"].ToString() == "NULL" ? "" : dt.Rows[0]["CLIENTE"].ToString(),
                        CorresponsalNombre = NombreCorresponsal,
                        ClienteCorresponsal = dt.Rows[0]["CLIENTERESP"].ToString(),
                        ClienteCorresponsalNombre =  NombreClienteCorresponsal,
                        Estado = dt.Rows[0]["STATUS"].ToString(),
                        StsConsolidado = dt.Rows[0]["ESTATUS_CONSOLIDADO"].ToString(),
                        PropNave = dt.Rows[0]["PROPIETARIO"].ToString(),
                        SomosRL = Convert.IsDBNull(dt.Rows[0]["AGENTERESANUAL"]) ? false : true,
                        CobrarRL = Convert.IsDBNull(dt.Rows[0]["JDIRECTIVA"]) ? false : true,
                        CambioRL = Convert.IsDBNull(dt.Rows[0]["CAMBIOAGENTER"]) ? false : true,
                        NombreRL = "", //FALTA, VER SI VA
                    };
                }
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
            }

            return datosNave;
        }

        [HttpGet]
        public DatosNave GetNaveByNombre(string nombre)
        {
            DataTable dt = null;

            try
            {
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionStringGENSYS"];

                string query = "SELECT NVV09.CODNAVE FROM NVV09 WHERE NVV09.NOMBRE LIKE '%" + nombre.ToUpper() + "%'";
                DA.GetDataTable(query, out dt);

                if (dt.Rows.Count > 0)
                    return GetNaveById(dt.Rows[0]["CODNAVE"].ToString());
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
        public DatosNave ObtenerDatosNave(string incidente, string etapa, int solicitud)
        {
            try
            {
                DatosNave n = null;
                OracleDataAccess DA = new OracleDataAccess();
                DA.ConnectionString = WebConfigurationManager.AppSettings["OracleConnectionString"];

                OracleStoredName Procedimiento = new OracleStoredName();
                Procedimiento.Nombre = "PAQ_DATOS_NAVE.USP_SEL_DATOS_NAVE";
                Procedimiento.IsPadre = Valores.IsNotPadre;
                Procedimiento.IsHereda = false;
                Procedimiento.AddParametros("P_INCIDENTE", OracleDbType.Varchar2, ParameterDirection.Input, 100, incidente);
                Procedimiento.AddParametros("OUT", OracleDbType.RefCursor, ParameterDirection.Output, 0, string.Empty);
                Procedimiento.AddParametros("P_SOLICITUD", OracleDbType.Int32, ParameterDirection.Input, 255, solicitud.ToString());

                DataTable dt = DA.GetDataTable(Procedimiento);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["NAVE_EXISTENTE"].ToString() == "1" && !string.IsNullOrEmpty(dt.Rows[0]["COD_NAVE"].ToString()))
                        n = GetNaveById(dt.Rows[0]["COD_NAVE"].ToString());

                    if (n != null)
                    {
                        n.Incidente = incidente;
                    }
                    else
                    {
                        n = new DatosNave()
                        {
                            Incidente = incidente,
                            NaveExistente = dt.Rows[0]["NAVE_EXISTENTE"].ToString(),
                            CodNave = dt.Rows[0]["COD_NAVE"].ToString(),
                            NombreNave = dt.Rows[0]["NOMBRE_NAVE"].ToString(),
                            CodCia = dt.Rows[0]["COD_CIA"].ToString(),
                            NombreCia = dt.Rows[0]["NOMBRE_CIA"].ToString(),
                            NombreAnterior = dt.Rows[0]["NOMBRE_ANTERIOR"].ToString(),
                            SocPropietaria = dt.Rows[0]["SOC_PROPIETARIA"].ToString(),
                            Corresponsal = dt.Rows[0]["CORRESPONSAL"].ToString(),
                            ClienteCorresponsal = dt.Rows[0]["CLIENTE_CORRESPONSAL"].ToString(),
                            Estado = dt.Rows[0]["ESTADO"].ToString(),
                            StsConsolidado = dt.Rows[0]["STS_CONSOLIDADO"].ToString(),
                            PropNave = dt.Rows[0]["PROP_NAVE"].ToString(),
                            SomosRL = dt.Rows[0]["SOMOS_RL"].ToString() == "1",
                            CobrarRL = dt.Rows[0]["COBRAR_RL"].ToString() == "1",
                            CambioRL = dt.Rows[0]["CAMBIO_RL"].ToString() == "1",
                            NombreRL = dt.Rows[0]["NOMBRE_RL"].ToString()
                        };
                    }
                }
                else
                {
                    n = new DatosNave();
                }

                //Consultar informacion ingresada en pestaña Datos Cliente
                SolicitudAPIController s = new SolicitudAPIController();
                DatosCliente c = s.ObtenerDatosCliente(incidente,solicitud);
                if (c != null)
                {
                    n.CodigoCliente = c.Cod == 0 ? "" : c.Cod.ToString();
                    n.NombreCliente = c.Nombre == null ? "" : c.Nombre;
                }

                return n;
            }
            catch (Exception ex)
            {
                UltimusLogs.Error(ex.ToString());
                return null;
            }
        }
    }
}