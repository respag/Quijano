namespace ULA.Quijano.Model.ModeloDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DATOSLEGALES.CATALOGO_AREA")]
    public partial class CATALOGO_AREA
    {
        [Key]
        public decimal CODAREA { get; set; }

        [StringLength(200)]
        public string DESCRIPCION { get; set; }
    }
}
