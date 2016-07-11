namespace ULA.Quijano.Model.ModeloDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DATOSLEGALES.BITACORA_COMENTARIOS")]
    public partial class BITACORA_COMENTARIOS
    {
        [Key]
        public decimal ID_COMENTARIO { get; set; }

        [Required]
        [StringLength(100)]
        public string PROCESO { get; set; }

        [Required]
        [StringLength(100)]
        public string INCIDENTE { get; set; }

        [Required]
        [StringLength(200)]
        public string USUARIO { get; set; }

        public DateTime FECHA { get; set; }

        [Required]
        public string COMENTARIO { get; set; }
    }
}
