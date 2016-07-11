namespace ULA.Quijano.Model.ModeloDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DATOSLEGALES.BPM_CATWEBAPLICATIONS")]
    public partial class BPM_CATWEBAPLICATIONS
    {
        public BPM_CATWEBAPLICATIONS()
        {
            BPM_CATFORMS = new HashSet<BPM_CATFORMS>();
        }

        [Key]
        public decimal IDWEBAPLICATION { get; set; }

        [Required]
        [StringLength(300)]
        public string WEBAPLICATIONNAME { get; set; }

        [Required]
        [StringLength(1000)]
        public string WEBAPLICATIONPATH { get; set; }

        public virtual ICollection<BPM_CATFORMS> BPM_CATFORMS { get; set; }
    }
}
