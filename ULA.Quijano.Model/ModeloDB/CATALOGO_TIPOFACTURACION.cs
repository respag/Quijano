using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULA.Quijano.Model.ModeloDB
{
    [Table("DATOSLEGALES.CATALOGO_TIPOFACTURACION")]
    public class CATALOGO_TIPOFACTURACION
    {
        [Key]
        public decimal COD_FACTURACION  { get; set; }

        [StringLength(500)]
        public string DESCRIPCION { get; set; }
    }
}
