using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Configuration;

namespace Ultimus.UtilityLayer
{
    public class ClaseBase
    {
        public string Usuario { get; set; }
        public string HostName { get; set; }
        public string IpAdress { get; set; }
        protected Exception _Errores = null;
        
        /// <summary>
        /// Para manejar error en la función. Describe el error en tal caso que ocurra.
        /// </summary>
        public Exception Errores
        {
            get { return _Errores; }
            set { _Errores = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ClaseBase()
        {
            _Errores = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public string GetStringToDateTime(DateTime fecha)
        {
            return string.Format("{0:yyyy-MM-ddTHH:mm:ss.s}", fecha);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public string GetStringToDateTimeOracle(DateTime fecha)
        {
            return string.Format("{0:dd-MMM-yyyy}", fecha);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DateTime FormatDateTime(string date)
        {
            return DateTime.Parse(date);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualpath"></param>
        /// <returns></returns>
        public byte[] ToByteArray(string virtualpath)
        {
            return new WebClient().DownloadData(virtualpath);
        }

        public static string GetOracleDate(DateTime myDate)
        {
            string result = "";

            result = " TO_DATE('" + myDate.Month.ToString() + "-" + myDate.Day.ToString() + "-" + myDate.Year.ToString() + " " + myDate.Hour.ToString() + ":" + myDate.Minute.ToString() + ":" + myDate.Second.ToString() + "', 'MM/DD/YYYY HH24:MI:SS')  ";

            return result;
        }

        
    }
}
