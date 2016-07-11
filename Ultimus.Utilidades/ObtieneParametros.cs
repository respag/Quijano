using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Ultimus.UtilityLayer
{
    public class ObtieneParametros2 : Crypt
    {
        private const string KeyRegedit = "ParametrosProcesoUltimus";
        public string SqlConnectionString { get { return GetValueKeyRegedit("SQlStringConnection", true); } }
        public string SQlStringConnectionTemp { get { return GetValueKeyRegedit("SQlStringConnectionTemp", true); } }
        public string OracleConnectionString { get { return GetValueKeyRegedit("OracleConnectionString", true); } }
        public string PostgreSQLConnectionString { get { return GetValueKeyRegedit("PostgreSQLConnectionString", true); } }
        public string SqlStringConnectionEntityFramework { get { return GetValueKeyRegedit("SqlStringConnectionEntityFramework", true); } }
        public string RutaLogGenerados { get { return GetValueKeyRegedit("RutaLogGenerados", false); } }
        public string DominioCustomOc { get { return GetValueKeyRegedit("DominioCustomOc", false); } }
        public string NombreCustomOc { get { return GetValueKeyRegedit("NombreCustomOc", false); } }
        public string SQlStringConnectionBPMUltimus { get { return GetValueKeyRegedit("SQlStringConnectionBPMUltimus", true); } }
        
        public string GetValueKeyRegedit(string KeyStringValue, bool decrypt)
        {
            string cadena = LeerResgistry(KeyRegedit, KeyStringValue);
            if (decrypt)
                cadena = DecryptString(cadena);
            return cadena;
        }

        private string LeerResgistry(string Pathkey, string stringname)
        {
            string retorno = string.Empty;
            try
            {
                RegistryKey localMachineRegistry = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);
                RegistryKey rk = localMachineRegistry.OpenSubKey(string.Format(@"SOFTWARE\\{0}", Pathkey, false));
                if (rk != null)
                {
                    retorno = (string)rk.GetValue(stringname);
                    rk.Close();
                }
            }
            catch (Exception ex)
            {
                retorno = string.Empty;
                EscribirArchivo("LeerResgistry", "Pathkey: " + Pathkey + " - Stringname: " + stringname);
                EscribirArchivo("LeerResgistry", ex);
            }
            return retorno;
        }
    }
}
