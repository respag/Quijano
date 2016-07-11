using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULA.Quijano.Model.ModeloDB
{
    [Table("DATOSLEGALES.INSTRUCCIONES_OTROS_DEPARTAMENTOS")]
    public class INSTRUCCIONES_OTROS_DEPARTAMENTOS
    {
        [Key]
        public int ID_INSTRUCCIONES { get; set; }

        public int ID_DATOS_SOCIEDAD { get; set; }

        [StringLength(1)]
        public string URGENTE { get; set; }

        [StringLength(500)]
        public string URGENTE_COMENTARIO { get; set; }

        public DateTime? FECHA_LIMITE_ENTREGA { get; set; }

        [StringLength(500)]
        public string TRADUCCION_DESCRIPCION { get; set; }

        [StringLength(200)]
        public string NOTARIA_PROTOCOLIZA { get; set; }

        [StringLength(50)]
        public string NOTARIA_NOTARIA { get; set; }

        [StringLength(200)]
        public string NOTARIA_SECCION { get; set; }

        [StringLength(50)]
        public string NOTARIA_MODELO { get; set; }

        [StringLength(200)]
        public string NOTARIA_COMPARECE { get; set; }

        [StringLength(200)]
        public string NOTARIA_REFRENDA { get; set; }

        [StringLength(500)]
        public string NOTARIA_OTROS { get; set; }

        public int NRO_ASUNTO { get; set; }
    }
}
