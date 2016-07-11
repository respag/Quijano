using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ULA.Quijano.ProcesoLegal.Json
{

    public class DatosBusquedaJSON : BaseJsonMetadata
    {
        public string IdentificacionCliente { get; set; }
        public string IdentificacionCodigoPais { get; set; }
        public string CodigoTipoIdentificacion { get; set; }
        public string usuarioDataPro { get; set; }

        public string CodigoRespuesta { get; set; }
        public string DescripcionRespuesta { get; set; }
    }

    
}