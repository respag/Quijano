using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULA.Quijano.Model
{
    public class InstruccionOtrosDepartamentos
    {
        public string Incidente { get; set; }
        public bool Traduccion { get; set; }
        public bool Autenticacion { get; set; }
        public bool Notaria { get; set; }
        public bool Apostilla { get; set; }
        public bool MinrexGobJustici { get; set; }
        public bool Consulado { get; set; }
        public bool SeInscribe { get; set; }
        public bool AlteracionTurno { get; set; }
        public bool AperturaCuenta { get; set; }
        public bool Urgente { get; set; }
        public string UrgenteComentario { get; set; }
        public DateTime FechaLimiteEntrega { get; set; }
        public string TraduccionDescripcion { get; set; }
        public string NotariaProtocoliza { get; set; }
        public string NotariaNotaria { get; set; }
        public string NotariaSeccion { get; set; }
        public string NotariaModelo { get; set; }
        public string NotariaComparece { get; set; }
        public string NotariaRefrenda { get; set; }
        public string NotariaOtros { get; set; }
        public int NroAsunto { get; set; }
        public string Instrucciones { get; set; }
        public int Solicitud { get; set; }
    }
}
