namespace ULA.Quijano.Model.ModeloDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DATOSLEGALES.CATALOGO_TIPO_SOLICITUD")]
    public partial class CATALOGO_TIPO_SOLICITUD
    {
        [Key]
        public decimal CODIGO { get; set; }

        [StringLength(200)]
        public string DESCRIPCION { get; set; }
    }
}
