using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ULA.Quijano.ProcesoLegal.Json
{
    public class BaseJsonMetadata
    {
        public int sol_co_solicitud { get; set; }
        public int hiddencl_co_cliente { get; set; }

        public string CodigoRespuesta { get; set; }
        public string DescripcionRespuesta { get; set; }
    }
}