namespace ULA.Quijano.Model.ModeloDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DATOSLEGALES.TRAMITES_POR_AREA")]
    public partial class TRAMITES_POR_AREA
    {
        [Key]
        [Column(Order = 0)]
        public decimal IDTRAMITE { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal IDAREA { get; set; }
    }
}
