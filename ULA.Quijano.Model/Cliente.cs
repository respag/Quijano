using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULA.Quijano.Model
{
    public class Cliente
    {
        public int ID { get; set; }
        public string Nombre { get; set; } 
        public string Email { get; set; }
        public string idioma { get; set; }
    }

    public class DatosCliente
    {
        public string Incidente { get; set; }
        public string ClienteExistente { get; set; }
        public int Cod { get; set; }
        public string Nombre { get; set; }
        public string EstadoCumplimiento { get; set; }
        public string NivelRiesgo { get; set; }
        public decimal PuntajeRiesgo { get; set; }
        public string Pasaporte { get; set; }
        public string Idioma { get; set; }
        public string TelefonoCasa { get; set; }
        public string Pais { get; set; }
        public string Moneda { get; set; }
        public string Ocupacion { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public decimal SectorEconomico { get; set; }
        public string Pep { get; set; }
        public string TipoPersona { get; set; }
        public string ClasificacionCliente { get; set; }
        public bool ReqFormulario { get; set; }
        public bool ReqPasaporte { get; set; }
        public bool ReqReferencia { get; set; }
        public bool ReqProfesional { get; set; }
        public bool ReqDomicilio { get; set; }
        public bool ReqIncorp { get; set; }
        public bool ReqPromocionales { get; set; }
        public bool ReqMembresia { get; set; }
        public List<LibretaDireccion> LibretaDirecciones { get; set; }
        public List<Moneda> Monedas { get; set; }
        public bool ExistBD { get; set; }
        public int Solicitud { get; set; }
    }
}
