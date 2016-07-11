using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULA.Quijano.Model.ModeloDB
{
    [Table("DATOSLEGALES.DATOS_FACTURACION")]
    public class DATOS_FACTURACION
    {
        [Key]
        public string INCIDENTE { get; set; }
        public string COD_CLIENTE { get; set; }
        public string TIPO_FACTURACION { get; set; }
        public string FORMA_PAGO { get; set; }
        public int PROFORMA { get; set; }
        public string TIPO_FACTURA { get; set; }
        public string REFERENCIA { get; set; }
        public string NOMBRE_FACTURA { get; set; }
        public string DIRECCION_FACTURA { get; set; }
        public string LEYENDA_FACTURA { get; set; }
        public string RESPONSABLE_NOTIF { get; set; }
        public string FORMA_ENVIO_CURRIER { get; set; }
        public string FORMA_ENVIO_EMAIL { get; set; }
        public string FORMA_ENVIO_PERSONAL { get; set; }
        public string FORMA_ENVIO_MENSAJERIA { get; set; }
        public string DIRECCION_ENVIO { get; set; }
        public int FORMA_ENVIO { get; set; } //OBSOLETE
        public decimal NUMERO_SOLICITUD { get; set; }
        public decimal NUMERO_GUIA { get; set; }
        public string MONEDA { get; set; }
        public string HONORARIOS { get; set; }
        public string GASTOS { get; set; }
        public string TOTAL { get; set; }
        public int Solicitud { get; set; }
    }
}
