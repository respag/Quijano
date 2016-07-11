using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULA.Quijano.Model
{
    public class Asunto
    {
        public string Incidente { get; set; }
        public decimal NroAsunto { get; set; }
        public string Estado { get; set; }
        public string NombreEstado { get; set; }
        public string CodOficina { get; set; }
        public string Oficina { get; set; }
        public string CodCompany { get; set; }
        public string Company { get; set; }
        public string Aplicacion { get; set; }
        public string FechaApertura { get; set; }
        public string CodAsistente { get; set; }
        public string Asistente { get; set; }
        public string Secretaria { get; set; }

        public string CodSociedad { get; set; }
        public string SociedadFundac { get; set; }
        public string CodCliente { get; set; }
        public string NombreCliente { get; set; }
        public string TipoFactura { get; set; }
        public decimal HonorariosFacturados { get; set; }
        public decimal GastosFacurados { get; set; }
        public decimal TotalFacturado { get; set; }
        public decimal TotalPendienteFacturado { get; set; }
        public List<AsuntoEstadoSolicitudes> EstadoSolicitudes { get; set; }

        public string CodJurisdiccion { get; set; }
        public string DescJurisdiccion { get; set; }
        public string CodNave { get; set; }
        public string DescNave { get; set; }
        public string CodMateria { get; set; }
        public string DescMateria { get; set; }

        public string Seccion { get; set; }
        public string PropiedadIntelectual { get; set; }
        public string NombreFactura { get; set; }
        public string CodigoMateria { get; set; }
        public string Corresponsal { get; set; }
        public string Direccion { get; set; }
        public string Referencia { get; set; }
        public string Abogado { get; set; }
        public string Observaciones { get; set; }
        public string ReferidoPor { get; set; }
        public string Responsable { get; set; }
        public string Solicitud { get; set; }
    }

    public class AsuntoEstadoSolicitudes
    {
        public string Tipo { get; set; }
        public string FechaSolicitud { get; set; }
        public string Asignado { get; set; }
        public string Estado { get; set; }
    }
}
