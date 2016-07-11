namespace ULA.Quijano.Model.ModeloDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DATOSLEGALES.ESTADO_SOLICITUD")]
    public partial class ESTADO_SOLICITUD
    {
        [Key]
        public decimal CODIGO { get; set; }

        [Required]
        [StringLength(200)]
        public string NOMBRE_ESTADO { get; set; }

        public decimal COMPLETAR_TAREA { get; set; }
    }
}
