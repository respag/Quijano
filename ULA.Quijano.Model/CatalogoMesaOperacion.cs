using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULA.Quijano.Model
{
    public class CatalogoEntidades
    {
        public int Codigo { get; set; }
        public string Entidad { get; set; }
    }

    public class CatalogoAcciones
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
    }

    public class CatalogoTipoSolicitud
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
    }
}
