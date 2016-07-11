namespace ULA.Quijano.Model.ModeloDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DATOSLEGALES.BPM_FORMSPROCESS")]
    public partial class BPM_FORMSPROCESS
    {
        [Key]
        public decimal IDFORMPROCESS { get; set; }

        public decimal IDSTEP { get; set; }

        public decimal FORMORDER { get; set; }

        public decimal IDFORM { get; set; }

        public virtual BPM_CATFORMS BPM_CATFORMS { get; set; }

        public virtual BPM_CATSTEPS BPM_CATSTEPS { get; set; }
    }
}
