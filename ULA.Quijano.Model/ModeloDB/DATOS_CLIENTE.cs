namespace ULA.Quijano.Model.ModeloDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DATOSLEGALES.DATOS_CLIENTE")]
    public partial class DATOS_CLIENTE
    {
        [Key]
        public decimal ID_DATOS_CLIENTE { get; set; }

        public decimal COD { get; set; }

        [StringLength(60)]
        public string NOMBRE { get; set; }

        [StringLength(1)]
        public string CLIENTE_EXISTENTE { get; set; }

        [StringLength(1)]
        public string ESTADO_CUMPLIMIENTO { get; set; }

        [StringLength(2)]
        public string NIVEL_RIESGO { get; set; }

        public decimal PUNTAJE_RIESGO { get; set; }

        [StringLength(25)]
        public string PASAPORTE { get; set; }

        [StringLength(25)]
        public string IDIOMA { get; set; }

        [StringLength(60)]
        public string TELEFONO_CASA { get; set; }

        [StringLength(4)]
        public string PAIS { get; set; }

        [StringLength(50)]
        public string MONEDA { get; set; }

        [StringLength(100)]
        public string OCUPACION { get; set; }

        [StringLength(21)]
        public string CELULAR { get; set; }

        [StringLength(200)]
        public string EMAIL { get; set; }

        public decimal SECTOR_ECONOMICO { get; set; }

        [StringLength(1)]
        public string PEP { get; set; }

        [StringLength(2)]
        public string TIPO_PERSONA { get; set; }

        [StringLength(5)]
        public string CLASIFICACION_CLIENTE { get; set; }

        [StringLength(1)]
        public string REQ_FORMULARIO { get; set; }

        [StringLength(1)]
        public string REQ_PASAPORTE { get; set; }

        [StringLength(1)]
        public string REQ_REFERENCIA { get; set; }

        [StringLength(1)]
        public string REQ_PROFESIONAL { get; set; }

        [StringLength(1)]
        public string REQ_DOMICILIO { get; set; }

        [StringLength(1)]
        public string REQ_INCORP { get; set; }

        [StringLength(1)]
        public string REQ_PROMOCIONALES { get; set; }

        [StringLength(1)]
        public string REQ_MEMBRESIA { get; set; }
    }
}
