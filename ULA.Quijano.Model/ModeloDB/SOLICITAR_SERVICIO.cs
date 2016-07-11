namespace ULA.Quijano.Model.ModeloDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DATOSLEGALES.SOLICITAR_SERVICIO")]
    public partial class SOLICITAR_SERVICIO
    {
        [StringLength(20)]
        public string SOLICITUD { get; set; }

        public decimal? ID_ENTIDAD { get; set; }

        [StringLength(100)]
        public string SOCIEDAD_ENTIDAD_EMPRESA { get; set; }

        [StringLength(250)]
        public string NOMBRE_PARA { get; set; }

        [StringLength(250)]
        public string NOMBRE_DE { get; set; }

        [StringLength(100)]
        public string TRAMITE { get; set; }

        [StringLength(500)]
        public string DETALLE { get; set; }

        [StringLength(500)]
        public string DIRECCION { get; set; }

        [StringLength(20)]
        public string PISO { get; set; }

        [StringLength(20)]
        public string CALLE { get; set; }

        [StringLength(500)]
        public string REFERENCIAS { get; set; }

        [StringLength(100)]
        public string HORARIO { get; set; }

        [StringLength(50)]
        public string TELEFONO { get; set; }

        [StringLength(250)]
        public string ASUNTO { get; set; }

        public decimal? ID_ACCION { get; set; }

        public decimal? ID_RESPONSABLE { get; set; }

        [StringLength(50)]
        public string ESTADO { get; set; }

        [StringLength(500)]
        public string COMENTARIOS { get; set; }

        [Key]
        public decimal INCIDENTE { get; set; }

        public virtual CATALOGO_ACCIONES CATALOGO_ACCIONES { get; set; }

        public virtual CATALOGO_ENTIDADES CATALOGO_ENTIDADES { get; set; }
    }
}
