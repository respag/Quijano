using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace Ultimus.DataAccessLayer
{
    public class PostgreSQLStoredName : IDisposable
    {
        private string nombre = String.Empty;
        private Valores ispadre = Valores.IsNotPadre;
        private int commandtimeout = 30;
        private NpgsqlParameterCollection parametrospadres;
        private NpgsqlCommand cmd = new NpgsqlCommand();
        private NpgsqlCommand cmdpadres = new NpgsqlCommand();
        private NpgsqlParameterCollection parametros;
        private string storednamepadre = String.Empty;
        private Boolean ishereda;
        private int indexstoredhereda;

        #region Propiedades
        /// <summary>
        /// Nombre del procedimiento almacenado.
        /// </summary>
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        /// <summary>
        /// Enumeración para identificar si es un procedimiento padre.
        /// </summary>
        public Valores IsPadre
        {
            get { return ispadre; }
            set { ispadre = value; }
        }

        /// <summary>
        /// Collección de parametros del procedimiento almacenado. 
        /// </summary>
        public NpgsqlParameterCollection Parametros
        {
            get { return parametros; }
        }

        /// <summary>
        /// Command timeout. Por default es 30.
        /// </summary>
        public int CommandTimeout
        {
            get { return commandtimeout; }
            set { commandtimeout = value; }
        }

        /// <summary>
        /// Nombre de los parametros de los cuales heredan los procedimiento hijos de este procedimiento.
        /// </summary>
        public NpgsqlParameterCollection ParametrosPadres
        {
            get { return parametrospadres; }
        }

        /// <summary>
        /// Nombre del procedimiento almacenado, de la collección, del cual hereda los volores de los parametro. 
        /// </summary>
        public string StoredNamePadre
        {
            get { return storednamepadre; }
            set { storednamepadre = value; }
        }

        /// <summary>
        /// Identificador del procedimiento almacenado indicando que hereda valores de los parametros de otro procedimiento.
        /// </summary>
        public Boolean IsHereda
        {
            get { return ishereda; }
            set { ishereda = value; }
        }

        /// <summary>
        /// Indice del prodecimiento almacenado, de la collección, del cual hereda los volores de los parametro.
        /// </summary>
        public int IndexStoredHereda
        {
            get { return indexstoredhereda; }
            set { indexstoredhereda = value; }
        }

        #endregion

        #region Contructores
        /// <summary>
        /// Estructura de los procedimientos almacenados almacenados, incluyendo los parametros. 
        /// </summary>
        public PostgreSQLStoredName()
        {
            parametros = cmd.Parameters;
            parametrospadres = cmdpadres.Parameters;
        }
        /// <summary>
        /// Estructura de los procedimientos almacenados almacenados, incluyendo los parametros.
        /// </summary>
        /// <param name="name">Nombre del procedimiento almacenado.</param>
        /// <param name="padre">Enumeración para identificar si es un procedimiento padre.</param>
        /// <param name="cmdTimeout">Command timeout. Por default es 30</param>
        public PostgreSQLStoredName(string name, Valores padre, int cmdTimeout)
        {
            Nombre = name;
            IsPadre = padre;
            CommandTimeout = cmdTimeout;
            parametros = cmd.Parameters;
            parametrospadres = cmdpadres.Parameters;
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Ejecuta los metodos para liberar los recuros de los objetos definidos en la clSql.
        /// </summary>
        /// <param name="disposing"></param>
        /// 
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                cmd.Dispose();
                parametros.Clear();
                cmdpadres.Dispose();
                parametrospadres.Clear();
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
        #endregion

        #region Métodos

        /// <summary>
        /// Agregar parametros a la colección de parametros del procedimiento almacenado.
        /// </summary>
        /// <param name="nombreparametro">Nombre del parametro</param>
        /// <param name="tipodato">Tipo de dato de la enumeración del SqlDbType</param>
        /// <param name="direccion">Establece un valor que indica si el parámetro es sólo de entrada, sólo de salida, bidireccional o un parámetro de un valor devuelto de procedimiento almacenado</param>
        /// <param name="longitud">Longitud o tamaño del parametro</param>
        /// <param name="valor">Valor del parametro, Si debe llegar es null a la Base de Datos debe enviarlo en blanco</param>
        public void AddParametros(string nombreparametro, NpgsqlDbType tipodato, ParameterDirection direccion, int longitud, string valor)
        {
            NpgsqlParameter param = new NpgsqlParameter();
            param.ParameterName = nombreparametro;
            param.NpgsqlDbType = tipodato;
            param.Size = longitud;
            param.Direction = direccion;
            //|| tipodato == SqlDbType.Date
            if ((tipodato == NpgsqlDbType.Date) && !String.IsNullOrEmpty(valor))
            {
                if (DateTime.Parse(valor).Year == 1900 || DateTime.Parse(valor).Year == 1)
                    param.Value = DBNull.Value;
                else
                    param.Value = valor;
            }
            else if (String.IsNullOrEmpty(valor))
                param.Value = DBNull.Value;
            else if (NpgsqlDbType.Boolean == tipodato && valor == bool.FalseString)
                param.Value = 0;
            else if (NpgsqlDbType.Boolean == tipodato && valor == bool.TrueString)
                param.Value = 1;
            else
                param.Value = valor;

            parametros.Add(param);
        }


        /// <summary>
        /// Utilizar solo para los tipos de datos SqlDbType.decimales y SqlDbType.Float.
        /// </summary>
        /// <param name="nombreparametro">Nombre del parametro</param>
        /// <param name="tipodato">Tipo de dato de la enumeración del SqlDbType</param>
        /// <param name="direccion">Establece un valor que indica si el parámetro es sólo de entrada, sólo de salida, bidireccional o un parámetro de un valor devuelto de procedimiento almacenado</param>
        /// <param name="Scale">Establece el número de posiciones decimales en el que se resuelve Value</param>
        /// <param name="Precision">Establece el número máximo de dígitos utilizados para representar la propiedad Value</param>
        /// <param name="valor">Valor del parametro, Si debe llegar es null a la Base de Datos debe enviarlo en blanco</param>
        public void AddParametros(string nombreparametro, NpgsqlDbType tipodato, ParameterDirection direccion, int precision, int scale, string valor)
        {
            NpgsqlParameter param = new NpgsqlParameter();
            param.ParameterName = nombreparametro;
            param.NpgsqlDbType = tipodato;
            param.Direction = direccion;
            param.Value = valor;

            if (tipodato == NpgsqlDbType.Numeric && scale >= 0 && precision >= 0)
            {
                if (String.IsNullOrEmpty(valor))
                    param.Value = DBNull.Value;
                else
                    param.Value = valor;
                param.Precision = (byte)precision;
                param.Size = 0;
                param.Scale = (byte)scale;
            }

            parametros.Add(param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object ObtenerValorParametro(string nombreparametro)
        {
            return parametros[nombreparametro].Value;
        }

        #endregion
    }
}
