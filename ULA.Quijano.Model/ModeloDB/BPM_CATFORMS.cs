namespace ULA.Quijano.Model.ModeloDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DATOSLEGALES.BPM_CATFORMS")]
    public partial class BPM_CATFORMS
    {
        public BPM_CATFORMS()
        {
            BPM_FORMSPROCESS = new HashSet<BPM_FORMSPROCESS>();
        }

        [Key]
        public decimal IDFORM { get; set; }

        [Required]
        [StringLength(1000)]
        public string FORMFILE { get; set; }

        [Required]
        [StringLength(200)]
        public string FORMLABEL { get; set; }

        public decimal IDWEBAPLICATION { get; set; }

        public virtual BPM_CATWEBAPLICATIONS BPM_CATWEBAPLICATIONS { get; set; }

        public virtual ICollection<BPM_FORMSPROCESS> BPM_FORMSPROCESS { get; set; }
    }
}
