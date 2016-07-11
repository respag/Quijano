using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULA.Quijano.Model
{
    public class DatosSociedad
    {
        public string Incidente { get; set; }
        public string SociedadExistente { get; set; }
        public string CodSociedad { get; set; }
        public string NombreSociedad { get; set; }
        public string IdTipoCorporacion { get; set; }
        public List<KeyValuePair<string, string>> TiposCorporacion { get; set; }
        public string IdClienteAnterior { get; set; }
        public string NombreClienteAnterior { get; set; }
        public string IdTipoCapital { get; set; }
        //public List<string> TiposCapital { get; set; }
        public string MontoCapital { get; set; }
        public string NAcciones { get; set; }
        public string ValorAccion { get; set; }
        public string IdTipoAcciones { get; set; }
        public List<KeyValuePair<string, string>> TiposAcciones { get; set; }
        public string IdTipoDirector { get; set; }
        //public List<string> TiposDirector { get; set; }
        public string Cantidad { get; set; }
        //public string Instrucciones { get; set; }
        public string IdRazones { get; set; }
        //public List<string> Razones { get; set; }
        public string IdPropositos { get; set; }
        //public List<string> Propositos { get; set; }
        public string DetalleProposito { get; set; }
        public string ProveeDirectores { get; set; }
        public string Tasa { get; set; }
        public string AgenteResid { get; set; }
        public string DirectoresQA { get; set; }
        public string Honorarios { get; set; }
        public string Gastos { get; set; }
        public string Total { get; set; }

        public string CodigoCliente { get; set; }
        public string NombreCliente { get; set; }

        public List<Dignatario> Dignatarios { get; set; }
        public List<AnalisisAntiguedad> AnalisisAntiguedad { get; set; }

        public int Solicitud { get; set; }
    }
}
