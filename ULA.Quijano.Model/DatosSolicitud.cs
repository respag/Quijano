using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULA.Quijano.Model
{
    public class DatosSolicitud
    {
        public Comunicacion comunicacion { get; set; }
        public DatosSociedad datosSociedad { get; set; }
        public DatosFundacion datosFundacion { get; set; }
        public DatosNave datosNave { get; set; }
        public DatosMigracion datosMigracion { get; set; }
        public List<CatalogoIdioma> idiomas { get; set; }
        public List<CatalogoProcedencia> procedencias { get; set; }
        public List<CatalogoArea> areas { get; set; }
        public List<CatalogoJurisdiccion> jurisdicciones { get; set; }
        public List<Abogado> abogados { get; set; }
        public List<CatalogoTramite> tramites { get; set; }
        public List<CatalogoFormasMigracion> formasMigracion { get; set; }
        public string InstruccionesEspecificas { get; set; }
        public DatosMarcas datosMarcas { get; set; }
        public int Solicitud { get; set; }
    }

    public class RegistroSolicitud
    {
        public int Idioma { get; set; }
        public int IdProcedencia { get; set; }
        public int IdArea { get; set; }
        public int IdJurisdiccion { get; set; }
        public int IdTramite { get; set; }
        public string Abogado { get; set; }
    }

    public class DatosMarcas
    {
        public string Incidente { get; set; }
        public int TIPO_SOLICITUD_MARCA { get; set; }
        public int TIPO_REGISTRO { get; set; }
        public string PODER { get; set; }
        public string CHEQUE { get; set; }
        public string ETIQUETAS { get; set; }
        public string DECLARACION { get; set; }
        public string FORMULARIO { get; set; }
        public string OTROS { get; set; }
        public string ANEXOS { get; set; }
        public string PETICION { get; set; }
        public string NUMERO_REGISTRO { get; set; }
        public int Solicitud { get; set; }
    }
}
