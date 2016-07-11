namespace ULA.Quijano.Model.ModeloDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DATOSLEGALES.LIBRETA_DIRECCIONES")]
    public partial class LIBRETA_DIRECCIONES
    {
        [Key]
        public decimal ID_DIRECCION { get; set; }

        [Required]
        [StringLength(100)]
        public string NOMBRE_FACTURA { get; set; }

        [Required]
        [StringLength(100)]
        public string CODIGO_DIRECCION { get; set; }

        [Required]
        [StringLength(50)]
        public string DIRECCION { get; set; }

        [Required]
        public decimal CODIGO_CLIENTE { get; set; }
    }
}
