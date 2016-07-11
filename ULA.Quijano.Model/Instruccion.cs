using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULA.Quijano.Model
{
    public class Instruccion
    {
        public int IdInstruccion { get; set; }
        public string Proceso { get; set; }
        public string Incidente { get; set; }
        public int Solicitud { get; set; }
        public string EjecutadoPor { get; set; }
        public int Tipo { get; set; }
        public string Instrucciones { get; set; }
        public string Respuesta { get; set; }
    }
}
