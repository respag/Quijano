namespace ULA.Quijano.Model.ModeloDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DATOSLEGALES.CATALOGO_IDIOMA")]
    public partial class CATALOGO_IDIOMA
    {
        [Key]
        public decimal CODIDIOMA { get; set; }

        [StringLength(500)]
        public string DESCRIPCION { get; set; }
    }
}
