using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
using Ultimus.Utilitarios;
using Ultimus.UtilityLayer;
using Oracle.DataAccess.Client;



namespace Ultimus.DataAccessLayer
{
    public class OracleDataAccess : ClaseBase
    {

        LogFile UltimusLogs = new LogFile();

        private OracleCommand cmd;
        private OracleConnection conn;
        private OracleTransaction trans;
        public string ConnectionString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="string_connection"></param>
        public OracleDataAccess(string connectionstring)
        {
            cmd = new OracleCommand();
            conn = new OracleConnection();
            ConnectionString = connectionstring;
        }

        /// <summary>
        /// 
        /// </summary>
        public OracleDataAccess()
        {
            cmd = new OracleCommand();
            conn = new OracleConnection();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                cmd.Dispose();
                conn.Dispose();
                if (trans != null)
                    trans.Dispose();
            }
        }

        /// <summary>
        /// Libera los recurso en memoria, ejecuta los metodos para liberar los recuros de los objetos definidos en la clSql.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool OpenOracleTransaction()
        {
            ObtieneParametros2 ObjParametros = new ObtieneParametros2();
            bool ReturnValue = false;

            try
            {
                if (string.IsNullOrEmpty(ConnectionString))
                    ConnectionString = ObjParametros.OracleConnectionString;
                if (conn.State == ConnectionState.Closed)
                {
                    conn.ConnectionString = ConnectionString;
                    conn.Open();
                    trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Connection = conn;
                    cmd.Transaction = trans;
                }
                ReturnValue = true;
            }
            catch (OracleException e)
            {
                ReturnValue = false;
                Errores = e;                
            }
            catch (Exception ex)
            {
                ReturnValue = false;
                Errores = ex;
            }
            return ReturnValue;
        }

        public bool Open()
        {
            ObtieneParametros2 ObjParametros = new ObtieneParametros2();
            bool ReturnValue = false;

            try
            {
                if (string.IsNullOrEmpty(ConnectionString))
                    ConnectionString = ObjParametros.OracleConnectionString;
                conn.ConnectionString = ConnectionString;
                conn.Open();
                cmd.Connection = conn;
                ReturnValue = true;
            }
            catch (OracleException e)
            {
                ReturnValue = false;
                Errores = e;
            }
            catch (Exception ex)
            {
                ReturnValue = false;
                Errores = ex;
            }
            return ReturnValue;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void Commit()
        {
            trans.Commit();
            conn.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Rollback()
        {
            trans.Rollback();
            conn.Close();
        }

        public void Close()
        {
            conn.Close();
        }

        /// <summary>
        ///  Ejecutar un procedimiento almacenado y retorna los registros en un DatSqlt
        /// </summary>
        /// <param name="StoredProcedure">Información del procedimiento y los parametros.</param>
        /// <returns>Retorna un Datset</returns>
        public DataSet GetDataSet(OracleStoredName storedProcedure)
        {
            OracleDataAdapter ad = new OracleDataAdapter();
            DataSet ds = new DataSet("Consulta");
            ds.Locale = CultureInfo.InvariantCulture;
            OracleParameter parm;
            try
            {
                cmd.Parameters.Clear();
                if (storedProcedure != null)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedure.Nombre;
                    foreach (OracleParameter parms in storedProcedure.Parametros)
                    {
                        parm = cmd.Parameters.Add(parms.ParameterName, parms.OracleDbType, parms.Size);
                        parm.Direction = parms.Direction;
                        parm.Value = parms.Value;
                    }
                    if (conn.State == ConnectionState.Closed)
                        OpenOracleTransaction();
                    cmd.CommandTimeout = storedProcedure.CommandTimeout;
                    cmd.Connection = conn;
                    ad.SelectCommand = cmd;
                    ad.Fill(ds);
                }
            }
            catch (OracleException Oracleex)
            {
                _Errores = Oracleex;
            }
            catch (Exception ex)
            {
                _Errores = ex;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed) conn.Close();
                ad.Dispose();
                cmd.Dispose();
                conn.Dispose();
            }
            return ds;
        }

        /// <summary>
        /// Ejecutar un procedimiento almacenado y retorna los registros en un DataTable
        /// </summary>
        /// <param name="StoredProcedure">Información del procedimiento y los parametros.</param>
        /// <returns>retorn un DataTable</returns>
        public DataTable GetDataTable(OracleStoredName storedProcedure)
        {
            OracleDataAdapter ad = new OracleDataAdapter();
            OracleParameter parm;
            DataTable dt = new DataTable("Consulta");

            try
            {
                cmd.Parameters.Clear();
                dt.Locale = CultureInfo.InvariantCulture;
                if (storedProcedure != null)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedure.Nombre;
                    foreach (OracleParameter parms in storedProcedure.Parametros)
                    {
                        parm = cmd.Parameters.Add(parms.ParameterName, parms.OracleDbType, parms.Size);
                        parm.Direction = parms.Direction;
                        parm.Value = parms.Value;
                    }
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn = new OracleConnection();
                        conn.ConnectionString = ConnectionString;
                        conn.Open();
                    }
                    cmd.CommandTimeout = storedProcedure.CommandTimeout;
                    cmd.Connection = conn;
                    ad.SelectCommand = cmd;
                    ad.Fill(dt);
                }
            }
            catch (OracleException Oracleex)
            {
                _Errores = Oracleex;
            }
            catch (Exception ex)
            {
                _Errores = ex;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed) conn.Close();
                ad.Dispose();
                cmd.Dispose();
                conn.Dispose();
            }
            return dt;
        }

        /// <summary>
        /// Ejecutar sentencias directas con la base de datos
        /// </summary>
        /// <param name="query">Query sql.</param>
        /// <returns>retorn un DataTable</returns>
        public bool GetDataTable(string query, out DataTable dt)
        {
            bool ret = true;
            dt = new DataTable();

            try
            {
                using (OracleConnection conn = new OracleConnection(ConnectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = conn;
                        cmd.CommandText = query;
                        OracleDataReader odr = cmd.ExecuteReader();
                        dt.Load(odr, LoadOption.OverwriteChanges);
                    }
                }
            }
            catch (OracleException Oracleex)
            {
                ret = false;
                _Errores = Oracleex;
            }
            catch (Exception ex)
            {
                ret = false;
                _Errores = ex;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return ret;
        }

        /// <summary>
        /// Iserta, actualiza y elimina registros mediante una consulta directa
        /// </summary>
        /// <param name="query">Consulta a ejecutar</param>
        /// <param name="rowsUpdated">Cantidad de Filas actualizadas</param>
        /// <returns></returns>
        public bool InsertUpdateDelete(string query, out int rowsUpdated)
        {
            bool ret = true;
            rowsUpdated = 0;

            try
            {
                using (OracleConnection conn = new OracleConnection(ConnectionString))
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = conn;
                        cmd.CommandText = query;
                        rowsUpdated = cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (OracleException Oracleex)
            {
                ret = false;
                rowsUpdated = 0;
                _Errores = Oracleex;
            }
            catch (Exception ex)
            {
                ret = false;
                rowsUpdated = 0;
                _Errores = ex;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return ret;
        }

        /// <summary>
        /// Ejecuta procedimientos almacenados, Se insertan registros padre e hijos a la vez.
        /// La herencia de parametros puede ser por nombre e indice.
        /// </summary>
        /// <param name="StoredName">Información del procedimiento y los parametros.</param>
        /// <returns>Estructura con los valores del error, la excepción y el volor de retorno producto de la transacción</returns>
        public bool EjecutarProcedimientos(ICollection<OracleStoredName> procedimientos)
        {
            bool ReturnValue = true;
            OracleParameter parm;
            OracleParameterCollection NombreParametroPadre = null;
            string NombrePadre = "";

            try
            {
                if (conn.State == ConnectionState.Closed)
                    ReturnValue = OpenOracleTransaction();

                if (procedimientos != null && ReturnValue)
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    #region  Recorrer la lista de procedimientos almacenados a ejecutar.
                    foreach (OracleStoredName procedimiento in procedimientos)
                    {
                        cmd.CommandText = procedimiento.Nombre;
                        cmd.CommandTimeout = procedimiento.CommandTimeout;
                        cmd.Parameters.Clear();

                        if (procedimiento.IsPadre == Valores.IsPadre && procedimiento.ParametrosPadres.Count > 0)
                        {
                            NombreParametroPadre = procedimiento.ParametrosPadres;
                            NombrePadre = procedimiento.Nombre;
                        }

                        #region Recorrer la lista de parametros por procedimientos
                        foreach (OracleParameter parms in procedimiento.Parametros)
                        {
                            if (NombreParametroPadre != null && procedimiento.IsPadre == Valores.IsNotPadre)
                            {
                                if (parms.Value == DBNull.Value
                                    && NombreParametroPadre.Contains(parms.ParameterName)
                                    && NombrePadre == procedimiento.StoredNamePadre)
                                    parms.Value = NombreParametroPadre[parms.ParameterName].Value;
                                else if (parms.Value == DBNull.Value
                                    && NombreParametroPadre.Contains(parms.ParameterName.Replace("@i_", "@o_"))
                                    && NombrePadre == procedimiento.StoredNamePadre)
                                    parms.Value = NombreParametroPadre[parms.ParameterName.Replace("@i_", "@o_")].Value;
                            }
                            else if ((parms.Value == DBNull.Value || parms.Value == null) && procedimiento.IsPadre == Valores.IsPadre
                                    && procedimiento.IsHereda
                                    && parms.Direction != ParameterDirection.Output
                                    && procedimientos.ElementAt(procedimiento.IndexStoredHereda).Parametros.Contains(parms.ParameterName))
                                parms.Value = procedimientos.ElementAt(procedimiento.IndexStoredHereda).Parametros[parms.ParameterName].Value;
                            else if ((parms.Value == DBNull.Value || parms.Value == null) && procedimiento.IsPadre == Valores.IsPadre
                                    && procedimiento.IsHereda
                                    && parms.Direction != ParameterDirection.Output
                                    && procedimientos.ElementAt(procedimiento.IndexStoredHereda).Parametros.Contains(parms.ParameterName.Replace("@i_", "@o_")))
                                parms.Value = procedimientos.ElementAt(procedimiento.IndexStoredHereda).Parametros[parms.ParameterName.Replace("@i_", "@o_")].Value;

                            parm = cmd.Parameters.Add(parms.ParameterName, parms.OracleDbType, parms.Size);
                            parm.Direction = parms.Direction;
                            parm.Value = parms.Value;

                            if (parms.OracleDbType == OracleDbType.Double)
                            {
                                parm.Precision = parms.Precision;
                                parm.Size = parms.Size;
                                parm.Scale = parms.Scale;
                            }
                        }
                        #endregion

                        cmd.ExecuteNonQuery();                        

                        if (procedimiento.IsPadre == Valores.IsPadre)
                        {
                            foreach (OracleParameter nombre in procedimiento.ParametrosPadres)
                                if (cmd.Parameters.Contains(nombre.ParameterName))
                                {
                                    procedimiento.Parametros[nombre.ParameterName].Value = cmd.Parameters[nombre.ParameterName].Value;
                                    NombreParametroPadre[nombre.ParameterName].Value = cmd.Parameters[nombre.ParameterName].Value;
                                    if (procedimiento.Parametros.Contains(nombre.ParameterName.Replace("@o_", "@i_")))
                                        procedimiento.Parametros[nombre.ParameterName.Replace("@o_", "@i_")].Value = cmd.Parameters[nombre.ParameterName].Value;
                                }
                        }
                        else
                        {
                            foreach (OracleParameter nombre in procedimiento.Parametros)
                                if (nombre.Direction != ParameterDirection.Input)
                                    nombre.Value = cmd.Parameters[nombre.ParameterName].Value;
                        }
                    }

                    #endregion

                    Commit();
                }
            }
            catch (OracleException e)
            {
                ReturnValue = false;
                Errores = e;
                UltimusLogs.EscribirArchivo("ULA", e);
                Rollback();

            }
            catch (Exception ex)
            {
                ReturnValue = false;
                Errores = ex;
                Rollback();
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
            return ReturnValue;
        }
    }
}
