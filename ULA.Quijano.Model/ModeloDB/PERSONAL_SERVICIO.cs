namespace ULA.Quijano.Model.ModeloDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DATOSLEGALES.PERSONAL_SERVICIO")]
    public partial class PERSONAL_SERVICIO
    {
        [Key]
        public decimal CODIGO { get; set; }

        [StringLength(200)]
        public string NOMBRE_USUARIO { get; set; }

        [StringLength(200)]
        public string NOMBRE_COMPLETO { get; set; }

        [StringLength(200)]
        public string CORREO_ELECTRONICO { get; set; }

        public decimal? TIPO { get; set; }
    }
}
