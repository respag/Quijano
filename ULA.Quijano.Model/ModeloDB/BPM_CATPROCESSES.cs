namespace ULA.Quijano.Model.ModeloDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DATOSLEGALES.BPM_CATPROCESSES")]
    public partial class BPM_CATPROCESSES
    {
        public BPM_CATPROCESSES()
        {
            BPM_CATSTEPS = new HashSet<BPM_CATSTEPS>();
        }

        [Key]
        public decimal IDPROCESS { get; set; }

        [Required]
        [StringLength(100)]
        public string PROCESSNAME { get; set; }

        public decimal PROCESSVERSION { get; set; }

        public virtual ICollection<BPM_CATSTEPS> BPM_CATSTEPS { get; set; }
    }
}
