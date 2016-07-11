using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULA.Quijano.Model
{
    public class Servicio
    {
        public int Incidente { get; set; }
        public string Solicitud { get; set; }
        public int IdEntidad { get; set; }
        public string SociedadEntidadEmpresa { get; set; }
        public string NombrePara { get; set; }
        public string NombreDe { get; set; }
        public string Tramite { get; set; }
        public string Detalle { get; set; }
        public string Direccion { get; set; }
        public string Piso { get; set; }
        public string Calle { get; set; }
        public string Referencias { get; set; }
        public string Horario { get; set; }
        public string Telefono { get; set; }
        public string Asunto { get; set; }
        public int IdAccion { get; set; }
        public string Accion { get; set; }
        public int Responsable { get; set; }
        public string NombreResponsable { get; set; }
        public string CorreoResponsable { get; set; }
        public string Estado { get; set; }
        public string Comentarios { get; set; }
        public string ComentariosResolucion { get; set; }
        public string NombreEstado { get; set; }
        public string FechaSeguimiento { get; set; }
        public string SolicitadoPor { get; set; }
    }
}
