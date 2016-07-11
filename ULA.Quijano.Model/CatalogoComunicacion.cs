using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULA.Quijano.Model
{

    //Catalogos definicion
    public class CatalogoIdioma
    {
        public int CodIdioma { get; set; }
        public string Descripcion { get; set; }
    }

    public class CatalogoProcedencia
    {
        public int CodProcedencia { get; set; }
        public string Descripcion { get; set; }
    }

    public class CatalogoArea
    {
        public int CodArea { get; set; }
        public string Descripcion { get; set; }
    }

    public class CatalogoTramite
    {
        public int CodTramite { get; set; }
        public string Descripcion { get; set; }
    }

    public class CatalogoJurisdiccion
    {
        public int CodJurisdiccion { get; set; }
        public string Nombre { get; set; }
    }

    public class CatalogoFormasMigracion
    {
        public int CodFormasMigracion { get; set; }
        public string Descripcion { get; set; }
    }

    public class CatalogoTipoSolicitudMarca
    {
        public int CodTipoSolicitudMarca { get; set; }
        public string Descripcion { get; set; }
    }

    public class CatalogoTipoRegistro
    {
        public int CodTipoRegistro { get; set; }
        public string Descripcion { get; set; }
    }

}
