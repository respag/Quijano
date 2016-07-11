using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
using Oracle.DataAccess.Client;

namespace Ultimus.DataAccessLayer
{
    public class OracleStoredName : IDisposable
    {
        private string nombre = String.Empty;
        private Valores ispadre = Valores.IsNotPadre;
        private int commandtimeout = 30;
        private OracleParameterCollection parametrospadres;
        private OracleCommand cmd = new OracleCommand();
        private OracleCommand cmdpadres = new OracleCommand();
        private OracleParameterCollection parametros;
        private string storednamepadre = String.Empty;
        private Boolean ishereda;
        private int indexstoredhereda;

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
        public OracleParameterCollection Parametros
        {
            get { return parametros; }
            set { parametros = value; }
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
        public OracleParameterCollection ParametrosPadres
        {
            get { return parametrospadres; }
            set { parametrospadres = value; }
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

        /// <summary>
        /// Estructura de los procedimientos almacenados almacenados, incluyendo los parametros. 
        /// </summary>
        public OracleStoredName()
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
        public OracleStoredName(string name, Valores padre, int cmdTimeout)
        {
            Nombre = name;
            IsPadre = padre;
            CommandTimeout = cmdTimeout;
            parametros = cmd.Parameters;
            parametrospadres = cmdpadres.Parameters;
        }

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

        /// <summary>
        /// Agregar parametros a la colección de parametros del procedimiento almacenado.
        /// </summary>
        /// <param name="nombreparametro">Nombre del parametro</param>
        /// <param name="tipodato">Tipo de dato de la enumeración del OracleDbType</param>
        /// <param name="direccion">Establece un valor que indica si el parámetro es sólo de entrada, sólo de salida, bidireccional o un parámetro de un valor devuelto de procedimiento almacenado</param>
        /// <param name="longitud">Longitud o tamaño del parametro</param>
        /// <param name="valor">Valor del parametro, Si debe llegar es null a la Base de Datos debe enviarlo en blanco</param>
        public void AddParametros(string nombreparametro, OracleDbType tipodato, ParameterDirection direccion, int longitud, string valor)
        {

            OracleParameter param = new OracleParameter();
            param.ParameterName = nombreparametro;
            param.OracleDbType = tipodato;
            param.Size = longitud;
            param.Direction = direccion;
            if (tipodato == OracleDbType.Date)
            {
                if (string.IsNullOrEmpty(valor))
                    param.Value = DBNull.Value;
                else
                {
                    if (DateTime.Parse(valor.ToString()).Year == 1900 || DateTime.Parse(valor.ToString()).Year == 1)
                        param.Value = DBNull.Value;
                    else
                        param.Value = valor;
                }
            }
            else if (String.IsNullOrEmpty(valor))
                param.Value = DBNull.Value;
            else
                param.Value = valor;


            parametros.Add(param);
        }

        /// <summary>
        /// Utilizar solo para los tipos de datos OracleDbType.decimales y OracleDbType.Float.
        /// </summary>
        /// <param name="nombreparametro">Nombre del parametro</param>
        /// <param name="tipodato">Tipo de dato de la enumeración del OracleDbType</param>
        /// <param name="direccion">Establece un valor que indica si el parámetro es sólo de entrada, sólo de salida, bidireccional o un parámetro de un valor devuelto de procedimiento almacenado</param>
        /// <param name="Scale">Establece el número de posiciones decimales en el que se resuelve Value</param>
        /// <param name="Precision">Establece el número máximo de dígitos utilizados para representar la propiedad Value</param>
        /// <param name="valor">Valor del parametro, Si debe llegar es null a la Base de Datos debe enviarlo en blanco</param>
        public void AddParametros(string nombreparametro, OracleDbType tipodato, ParameterDirection direccion, int precision, int scale, string valor)
        {
            OracleParameter param = new OracleParameter();
            param.ParameterName = nombreparametro;
            param.OracleDbType = tipodato;
            param.Direction = direccion;
            param.Value = valor;

            if (tipodato == OracleDbType.Double && scale >= 0 && precision >= 0)
            {
                if (string.IsNullOrEmpty(valor))
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
            if (parametros[nombreparametro].IsNullable)
                return string.Empty;
            else
                return parametros[nombreparametro].Value;
        }
    }
}
