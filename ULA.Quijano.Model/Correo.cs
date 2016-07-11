using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULA.Quijano.Model
{
    public class RespondeCorreo
    {
        public string UserId { get; set; }
        public string TaskId { get; set; }
        public string Nombre { get; set; }
        public int CodCliente { get; set; }
        public int Incidente { get; set; }
        public string Respuesta { get; set; }
        public string AbogadoResponde { get; set; }
        public int Devuelta { get; set; }
        public int ServicioLegal { get; set; }
        public string Etapa { get; set; }
        public string CorreoCC { get; set; }
        public int Solicitud { get; set; }
    }

    public class ActualizarCorreo
    {
        public string Proceso { get; set; }
        public int Incidente { get; set; }
        public string UserId { get; set; }
        public string TaskId { get; set; }
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public int IncidenteRelacionado { get; set; }
        public int CodIdioma { get; set; }
        public int CodProcedencia { get; set; }
        public int CodArea { get; set; }
        public int CodJurisdiccion { get; set; }
        public int CodTramite { get; set; }
        public string AbogadoAsignado { get; set; }
        public string UsuarioAbogadoAsignado { get; set; }
        public string CorreoOrigen { get; set; }
        public string CorreoPara { get; set; }
        public string Asunto { get; set; }
        public string Contenido { get; set; }
        public string AsuntoOrigen { get; set; }
        public string CorreoInfractor { get; set; }
        public string Respuesta { get; set; }
        public string AbogadoResponde { get; set; }
        public string Etapa { get; set; }
        public string CorreoCC { get; set; }
    }

    public class RegistrarCorreo
    {
        public string Proceso { get; set; }
        public string TempIncident { get; set; }
        public string UserId { get; set; }
        public string TaskId { get; set; }
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public int IncidenteRelacionado { get; set; }
        public int CodIdioma { get; set; }
        public int CodProcedencia { get; set; }
        public int CodArea { get; set; }
        public int CodJurisdiccion { get; set; }
        public int CodTramite { get; set; }
        public string AbogadoAsignado { get; set; }
        public string UsuarioAbogadoAsignado { get; set; }
        public string CorreoOrigen { get; set; }
        public string CorreoPara { get; set; }
        public string Asunto { get; set; }
        public string Contenido { get; set; }
        public string AsuntoOrigen { get; set; }
        public string CorreoInfractor { get; set; }
        public string Respuesta { get; set; }
        public string AbogadoResponde { get; set; }
        public string Etapa { get; set; }
        public string CorreoCC { get; set; }
    }

    public class DevolverCorreo
    {
        public string UserId { get; set; }
        public string TaskId { get; set; }
        public string Nombre { get; set; }
        public int Incidente { get; set; }
        public string Justificacion { get; set; }
    }
}
