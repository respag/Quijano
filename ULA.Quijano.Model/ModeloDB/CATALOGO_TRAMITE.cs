namespace ULA.Quijano.Model.ModeloDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DATOSLEGALES.CATALOGO_TRAMITE")]
    public partial class CATALOGO_TRAMITE
    {
        [Key]
        public decimal CODTRAMITE { get; set; }

        [StringLength(200)]
        public string DESCRIPCION { get; set; }
    }
}
