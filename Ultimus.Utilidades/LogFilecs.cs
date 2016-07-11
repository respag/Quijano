using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Ultimus.UtilityLayer
{
    public class LogFilecs
    {
        private bool Habilitar = true;
        public LogFilecs()
        {
            Habilitar = true;
        }
        public LogFilecs(bool habilitar)
        {
            Habilitar = habilitar;
        }

        public void EscribirArchivo(string application, string message)
        {
            if (Habilitar)
            {
                ObtieneParametros2 ObjParametros = new ObtieneParametros2();
                try
                {
                    string pathName = ObjParametros.RutaLogGenerados;

                    if (!System.IO.Directory.Exists(pathName))
                        System.IO.Directory.CreateDirectory(pathName);

                    string pathFile = string.Format(@"{0}\{1}-{2}.txt", pathName, application, DateTime.Now.ToString("yyyy-MM-dd"));
                    if (!System.IO.File.Exists(pathFile))
                    {
                        System.IO.FileStream f = System.IO.File.Create(pathFile);
                        f.Close();
                    }
                    using (StreamWriter sw = new StreamWriter(pathFile, true))
                    {
                        sw.WriteLine(string.Format("\r\nFecha:{0} \r\n {1}", DateTime.Now, message));
                    }
                }
                catch { }
                finally
                {
                    ObjParametros = null;
                }
            }
        }

        public void EscribirArchivo(string application, Exception ex)
        {
            if (Habilitar)
            {
                string message = string.Format("{0} \r\n {1}", ex.Message, ex.StackTrace);
                EscribirArchivo(application, message);
            }
        }
    }
}
