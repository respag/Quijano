namespace ULA.Quijano.Model.ModeloDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DATOSLEGALES.COMUNICACION")]
    public partial class COMUNICACION
    {
        [Key]
        public decimal INCIDENTE { get; set; }

        public decimal? IDCLIENTE { get; set; }

        [StringLength(100)]
        public string NOMBRE { get; set; }

        public string INCIDENTE_RELACIONADO { get; set; }

        public DateTime? FECHA { get; set; }

        public decimal? CODIDIOMA { get; set; }

        public decimal? CODPROCEDENCIA { get; set; }

        public decimal? CODAREA { get; set; }

        public decimal? CODJURISDICCION { get; set; }

        public decimal? CODTRAMITE { get; set; }

        [StringLength(200)]
        public string ABOGADO_ASIGNADO { get; set; }

        [StringLength(200)]
        public string CORREO_ORIGEN { get; set; }

        [StringLength(200)]
        public string CORREO_PARA { get; set; }

        [StringLength(200)]
        public string ASUNTO { get; set; }

        [StringLength(5)]
        public string ASUNTO_ORIGEN { get; set; }

        public DateTime? FECHA_RESP { get; set; }

        public string CONTENIDO { get; set; }

        [StringLength(200)]
        public string CORREO_INFRACTOR { get; set; }

        public string RESPUESTA { get; set; }

        [StringLength(200)]
        public string ABOGADO_RESPONDE { get; set; }

        public decimal? GENERA_INSTRUCCION { get; set; }
    }
}
