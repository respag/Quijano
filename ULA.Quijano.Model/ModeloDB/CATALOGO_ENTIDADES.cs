namespace ULA.Quijano.Model.ModeloDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DATOSLEGALES.CATALOGO_ENTIDADES")]
    public partial class CATALOGO_ENTIDADES
    {
        public CATALOGO_ENTIDADES()
        {
            SOLICITAR_SERVICIO = new HashSet<SOLICITAR_SERVICIO>();
        }

        [Key]
        public decimal CODIGOENTIDAD { get; set; }

        [StringLength(200)]
        public string ENTIDAD { get; set; }

        public virtual ICollection<SOLICITAR_SERVICIO> SOLICITAR_SERVICIO { get; set; }
    }
}
