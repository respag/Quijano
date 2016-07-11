namespace ULA.Quijano.Model.ModeloDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DATOSLEGALES.CATALOGO_PROCEDENCIA")]
    public partial class CATALOGO_PROCEDENCIA
    {
        [Key]
        public decimal CODPROCEDENCIA { get; set; }

        [StringLength(200)]
        public string DESCRIPCION { get; set; }
    }
}
