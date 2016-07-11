namespace ULA.Quijano.Model.ModeloDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DATOSLEGALES.ABOGADO_AREAS")]
    public partial class ABOGADO_AREAS
    {
        [Key]
        public decimal CODIGO { get; set; }
        
        [StringLength(200)]
        public string NOMBRE_USUARIO { get; set; }

        [StringLength(200)]
        public string NOMBRE_COMPLETO { get; set; }
    }
}
