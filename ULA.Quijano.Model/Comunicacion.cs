using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULA.Quijano.Model
{
    public class Comunicacion
    {
        public int Incidente { get; set; }
        public int? IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Incidente_Relacionado { get; set; }
        public string Fecha { get; set; }
        public int CodIdioma { get; set; }
        public int CodProcedencia { get; set; }
        public int CodArea { get; set; }
        public int CodJurisdiccion { get; set; }
        public int CodTramite { get; set; }
        public int CodFormasMig { get; set; }
        public string Abogado_Asignado { get; set; }
        public string Abogado_Asignado_User { get; set; }        
        public string Correo_Origen { get; set; }        
        public string Correo_Para { get; set; }
        public string Asunto { get; set; }
        public string Contenido { get; set; }
        public int Asunto_Origen_Queja { get; set; }
        public int Asunto_Origen_Devuelta { get; set; }
        public int Asunto_Origen_CPersonal { get; set; }
        public string Fecha_Resp { get; set; }
        public string Correo_Infractor { get; set; }
        public string Respuesta { get; set; }
        public string Abogado_Responde { get; set; }
        public int Genera_Instruccion { get; set; }
        public string ParametroAdjuntos { get; set; }
        public string Etapa { get; set; }
        public string EtapaActual { get; set; }
        public string EtapaAnterior { get; set; }
        public string ListaAdjuntos { get; set; }
        public string CorreoCC { get; set; }
        public string JustificacionDevolucion { get; set; }
        public int Solicitud { get; set; }
    }
}
