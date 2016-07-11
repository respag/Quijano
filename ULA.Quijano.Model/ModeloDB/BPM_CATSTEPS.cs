namespace ULA.Quijano.Model.ModeloDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DATOSLEGALES.BPM_CATSTEPS")]
    public partial class BPM_CATSTEPS
    {
        public BPM_CATSTEPS()
        {
            BPM_FORMSPROCESS = new HashSet<BPM_FORMSPROCESS>();
        }

        [Key]
        public decimal IDSTEP { get; set; }

        [Required]
        [StringLength(200)]
        public string STEPNAME { get; set; }

        public decimal IDPROCESS { get; set; }

        public virtual BPM_CATPROCESSES BPM_CATPROCESSES { get; set; }

        public virtual ICollection<BPM_FORMSPROCESS> BPM_FORMSPROCESS { get; set; }
    }
}
