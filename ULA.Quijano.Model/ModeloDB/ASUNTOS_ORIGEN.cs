namespace ULA.Quijano.Model.ModeloDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DATOSLEGALES.ASUNTOS_ORIGEN")]
    public partial class ASUNTOS_ORIGEN
    {
        public decimal ID { get; set; }

        [StringLength(200)]
        public string DESCRIPCION { get; set; }
    }
}
