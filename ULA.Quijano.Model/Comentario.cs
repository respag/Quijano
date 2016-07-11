using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULA.Quijano.Model
{
    public class Comentario
    {
        public string Proceso { get; set; }
        public string Incidente { get; set; }
        public string Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public string strFecha { get; set; }
        public string Texto { get; set; }
    }
}
