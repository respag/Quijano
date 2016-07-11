using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
using Ultimus.UtilityLayer;
using Npgsql;
using NpgsqlTypes;

namespace Ultimus.DataAccessLayer
{
    public class PostgreSQLDataAccess : ClaseBase
    {
            NpgsqlDataAdapter da = new NpgsqlDataAdapter();
            private NpgsqlCommand cmd;
            private NpgsqlConnection conn;
            private NpgsqlTransaction trans;
            public string ConnectionString { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="string_connection"></param>
            public PostgreSQLDataAccess(string connectionstring)
            {
                cmd = new NpgsqlCommand();
                conn = new NpgsqlConnection();
                ConnectionString = connectionstring;
            }

            /// <summary>
            /// 
            /// </summary>
            public PostgreSQLDataAccess()
            {
                cmd = new NpgsqlCommand();
                conn = new NpgsqlConnection();
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
            public bool OpenPostgreSQLTransaction()
            {
                ObtieneParametros2 ObjParametros = new ObtieneParametros2();
                bool ReturnValue = false;

                try
                {
                    if (string.IsNullOrEmpty(ConnectionString))
                        ConnectionString = ObjParametros.PostgreSQLConnectionString;
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
                catch (NpgsqlException Sqlex)
                {
                    ReturnValue = false;
                    Errores = Sqlex;
                }
                catch (Exception ex)
                {
                    ReturnValue = false;
                    Errores = ex;
                }
                finally
                {
                    ObjParametros = null;
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

            /// <summary>
            ///  Ejecutar un procedimiento almacenado y retorna los registros en un DatSqlt
            /// </summary>
            /// <param name="StoredProcedure">Información del procedimiento y los parametros.</param>
            /// <returns>Retorna un Datset</returns>
            public DataSet GetDataSet(PostgreSQLStoredName storedProcedure)
            {
                NpgsqlDataAdapter ad = new NpgsqlDataAdapter();
                DataSet ds = new DataSet("Consulta");
                ds.Locale = CultureInfo.InvariantCulture;
                NpgsqlParameter parm;
                try
                {
                    if (storedProcedure != null)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = storedProcedure.Nombre;
                        cmd.Parameters.Clear();

                        foreach (NpgsqlParameter parms in storedProcedure.Parametros)
                        {
                            parm = cmd.Parameters.Add(parms.ParameterName, parms.NpgsqlDbType, parms.Size);
                            parm.Direction = parms.Direction;
                            parm.Value = parms.Value;
                        }
                        if (conn.State == ConnectionState.Closed)
                            OpenPostgreSQLTransaction();
                        cmd.CommandTimeout = storedProcedure.CommandTimeout;
                        cmd.Connection = conn;
                        ad.SelectCommand = cmd;
                        ad.Fill(ds);
                    }
                }
                catch (NpgsqlException Sqlex)
                {
                    _Errores = Sqlex;
                }
                catch (Exception ex)
                {
                    _Errores = ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
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
            public DataTable GetDataTable(PostgreSQLStoredName storedProcedure)
            {
                NpgsqlDataAdapter ad = new NpgsqlDataAdapter();
                NpgsqlParameter parm;
                DataTable dt = new DataTable("Consulta");

                try
                {
                    dt.Locale = CultureInfo.InvariantCulture;
                    if (storedProcedure != null)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = storedProcedure.Nombre;
                        foreach (NpgsqlParameter parms in storedProcedure.Parametros)
                        {
                            parm = cmd.Parameters.Add(parms.ParameterName, parms.NpgsqlDbType, parms.Size);
                            parm.Direction = parms.Direction;
                            parm.Value = parms.Value;
                        }
                        if (conn.State == ConnectionState.Closed)
                            OpenPostgreSQLTransaction();
                        cmd.CommandTimeout = storedProcedure.CommandTimeout;
                        cmd.Connection = conn;
                        ad.SelectCommand = cmd;
                        ad.Fill(dt);
                    }
                }
                catch (NpgsqlException Sqlex)
                {
                    _Errores = Sqlex;
                }
                catch (Exception ex)
                {
                    _Errores = ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    ad.Dispose();
                    cmd.Dispose();
                    conn.Dispose();
                }
                return dt;
            }

            /// <summary>
            /// Ejecutar una consulta y retorna los registros en un DataTable
            /// </summary>
            /// <param name="query">Query sql.</param>
            /// <returns>retorn un DataTable</returns>
            public bool GetDataTable(string query, out DataTable dt)
            {
                NpgsqlDataAdapter ad = new NpgsqlDataAdapter();
                bool ret = true;
                dt = new DataTable();

                try
                {
                    dt.Locale = CultureInfo.InvariantCulture;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    if (conn.State == ConnectionState.Closed)
                        OpenPostgreSQLTransaction();
                    cmd.Connection = conn;
                    ad.SelectCommand = cmd;
                    ad.Fill(dt);
                }
                catch (NpgsqlException Sqlex)
                {
                    ret = false;
                    _Errores = Sqlex;
                }
                catch (Exception ex)
                {
                    ret = false;
                    _Errores = ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    ad.Dispose();
                    cmd.Dispose();
                    conn.Dispose();
                }

                return ret;
            }

            /// <summary>
            /// Ejecuta procedimientos almacenados, Se insertan registros padre e hijos a la vez.
            /// La herencia de parametros puede ser por nombre e indice.
            /// </summary>
            /// <param name="StoredName">Información del procedimiento y los parametros.</param>
            /// <returns>Estructura con los valores del error, la excepción y el volor de retorno producto de la transacción</returns>
            public bool EjecutarProcedimientos(List<PostgreSQLStoredName> procedimientos)
            {
                bool ReturnValue = true;
                NpgsqlParameter parm;
                NpgsqlParameterCollection NombreParametroPadre = null;
                string NombrePadre = "";

                try
                {
                    if (conn.State == ConnectionState.Closed)
                        ReturnValue = OpenPostgreSQLTransaction();

                    if (procedimientos != null && ReturnValue)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region  Recorrer la lista de procedimientos almacenados a ejecutar.
                        foreach (PostgreSQLStoredName procedimiento in procedimientos)
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
                            foreach (NpgsqlParameter parms in procedimiento.Parametros)
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
                                        && procedimientos[procedimiento.IndexStoredHereda].Parametros.Contains(parms.ParameterName))
                                    parms.Value = procedimientos[procedimiento.IndexStoredHereda].Parametros[parms.ParameterName].Value;
                                else if ((parms.Value == DBNull.Value || parms.Value == null) && procedimiento.IsPadre == Valores.IsPadre
                                        && procedimiento.IsHereda
                                        && parms.Direction != ParameterDirection.Output
                                        && procedimientos[procedimiento.IndexStoredHereda].Parametros.Contains(parms.ParameterName.Replace("@i_", "@o_")))
                                    parms.Value = procedimientos[procedimiento.IndexStoredHereda].Parametros[parms.ParameterName.Replace("@i_", "@o_")].Value;

                                parm = cmd.Parameters.Add(parms.ParameterName, parms.NpgsqlDbType, parms.Size);
                                parm.Direction = parms.Direction;
                                parm.Value = parms.Value;

                                if (parms.NpgsqlDbType == NpgsqlDbType.Numeric)
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
                                foreach (NpgsqlParameter nombre in procedimiento.ParametrosPadres)
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
                                foreach (NpgsqlParameter nombre in procedimiento.Parametros)
                                    if (nombre.Direction != ParameterDirection.Input)
                                        nombre.Value = cmd.Parameters[nombre.ParameterName].Value;
                            }
                        }

                        #endregion
                    }
                }
                catch (NpgsqlException Sqlex)
                {
                    ReturnValue = false;
                    Errores = Sqlex;
                }
                catch (Exception ex)
                {
                    ReturnValue = false;
                    Errores = ex;
                }
                return ReturnValue;
            }
        }
}