using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ULA.Quijano.SendMails
{
    [ServiceContract]
    public interface IWcfMensajeria
    {
        [OperationContract]
        Servicio GetServicioByID(int incidenteID);

        [OperationContract]
        bool ActualizarEstadoServicio(int incidenteID, string estado, string comentariosResolucion, string fechaSeguimiento);

        [OperationContract]
        List<EstadoSolicitud> GetEstadoSolicitud();
    }

    [DataContract]
    public partial class Servicio
    {
        [DataMember]
        public int Incidente { get; set; }
        [DataMember]
        public string Solicitud { get; set; }
        [DataMember]
        public int IdEntidad { get; set; }
        [DataMember]
        public string SociedadEntidadEmpresa { get; set; }
        [DataMember]
        public string NombrePara { get; set; }
        [DataMember]
        public string NombreDe { get; set; }
        [DataMember]
        public string Tramite { get; set; }
        [DataMember]
        public string Detalle { get; set; }
        [DataMember]
        public string Direccion { get; set; }
        [DataMember]
        public string Piso { get; set; }
        [DataMember]
        public string Calle { get; set; }
        [DataMember]
        public string Referencias { get; set; }
        [DataMember]
        public string Horario { get; set; }
        [DataMember]
        public string Telefono { get; set; }
        [DataMember]
        public string Asunto { get; set; }
        [DataMember]
        public int IdAccion { get; set; }
        [DataMember]
        public string Accion { get; set; }
        [DataMember]
        public int Responsable { get; set; }
        [DataMember]
        public string NombreResponsable { get; set; }
        [DataMember]
        public string CorreoResponsable { get; set; }
        [DataMember]
        public string Estado { get; set; }
        [DataMember]
        public string Comentarios { get; set; }
        [DataMember]
        public string ComentariosResolucion { get; set; }
        [DataMember]
        public string NombreEstado { get; set; }
        [DataMember]
        public string FechaSeguimiento { get; set; }
        [DataMember]
        public string SolicitadoPor { get; set; }
    }

    [DataContract]
    public partial class EstadoSolicitud
    {
        [DataMember]
        public string Codigo { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public int CompletarTarea { get; set; }
    }

}
