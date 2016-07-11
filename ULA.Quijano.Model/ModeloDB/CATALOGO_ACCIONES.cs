namespace ULA.Quijano.Model.ModeloDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DATOSLEGALES.CATALOGO_ACCIONES")]
    public partial class CATALOGO_ACCIONES
    {
        public CATALOGO_ACCIONES()
        {
            SOLICITAR_SERVICIO = new HashSet<SOLICITAR_SERVICIO>();
        }

        [Key]
        public decimal CODIGOACCION { get; set; }

        [Required]
        [StringLength(200)]
        public string DESCRIPCION { get; set; }

        public virtual ICollection<SOLICITAR_SERVICIO> SOLICITAR_SERVICIO { get; set; }
    }
}
