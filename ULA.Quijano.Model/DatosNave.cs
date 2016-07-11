using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULA.Quijano.Model
{
    public class DatosNave
    {
        public string Incidente { get; set; }
        public string NaveExistente { get; set; }
        public string CodNave { get; set; }
        public string NombreNave { get; set; }
        public string CodCia { get; set; }
        public string NombreCia { get; set; }
        public string NombreAnterior { get; set; }
        public string SocPropietaria { get; set; }
        public string Corresponsal { get; set; }
        public string CorresponsalNombre { get; set; }
        public string ClienteCorresponsal { get; set; }
        public string ClienteCorresponsalNombre { get; set; }
        public string Estado { get; set; }
        public string StsConsolidado { get; set; }
        public string PropNave { get; set; }
        public bool SomosRL { get; set; }
        public bool CobrarRL { get; set; }
        public bool CambioRL { get; set; }
        public string NombreRL { get; set; }
        public string CodigoCliente { get; set; }
        public string NombreCliente { get; set; }
        public int Solicitud { get; set; }
    }
}
