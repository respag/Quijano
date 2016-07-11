using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ULA.Quijano.ProcesoLegal.Commons
{
    public class UltimusIncident
    {
        public int Incident { get; set; }
        public string Process { get; set; }
        public string Step { get; set; }
        public string TaskId { get; set; }
        public string User { get; set; }
        public string UserFullName { get; set; }
    }

    public class Transaccion
    {
        public bool Resultado { get; set; }
        public string Error { get; set; }

        public Transaccion()
        {
            Resultado = true;
        }
    }

    public class ComboBoxDataSource
    {
        public Transaccion ResultadoTransaccion { get; set; }
        public List<ComboBoxItem> Items { get; set; }

        public ComboBoxDataSource()
        {
            ResultadoTransaccion = new Transaccion();
            Items = new List<ComboBoxItem>();
        }
    }

    public class ComboBoxItem
    {
        public int ID { get; set; }
        public string DESC { get; set; }
    }

    public class AutenticacionUltimus
    {
        public Transaccion ResultadoTransaccion { get; set; }
        public bool Autenticado { get; set; }
        public string RutaFirma { get; set; }

        public AutenticacionUltimus()
        {
            ResultadoTransaccion = new Transaccion();
            Autenticado = false;
        }
    }

    public class RevisionCreditoGrid
    {
        public Transaccion ResultadoTransaccion { get; set; }
        public List<RevisionCreditoRecord> Records { get; set; }

        public RevisionCreditoGrid()
        {
            ResultadoTransaccion = new Transaccion();
            Records = new List<RevisionCreditoRecord>();
        }
    }

    public class VentaCruzadaFields
    {
        public Transaccion ResultadoTransaccion { get; set; }
        public string LimitePropuesto { get; set; }
        public string NivelEndeudamiento { get; set; }

        public bool EsPrimeraFirma { get; set; }

        public decimal LimiteAprobado { get; set; }

        public VentaCruzadaFields()
        {
            ResultadoTransaccion = new Transaccion();
        }
    }

    public class RevisionCreditoRecord
    {
        public string Secuencia { get; set; }
        public string EtapaFirma { get; set; }
        public string Usuario { get; set; }
        public string Firma { get; set; }
        public string FechaFirma { get; set; }
        public string Decision { get; set; }
        public string Comentarios { get; set; }
        public string Razon { get; set; }
    }

    public class CredencialesUsuario
    {
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
    }

    public class AprobacionCalificacionCredito
    {
        public string UltimusUserID { get; set; }
        public string UltimusTaskID { get; set; }
        public string UltimusProcess { get; set; }
        public string UltimusStep { get; set; }
        public int UltimusIncident { get; set; }

        public int CodTipoDecision { get; set; }
        public int CodTipoRazon { get; set; }
        public string Comentarios { get; set; }
        public int[] Sujetos { get; set; }

        public DateTime Firma_Fecha { get; set; }
        public string Firma_Usuario { get; set; }
        public string Firma_Contrasena { get; set; }

        public decimal VentaCruzada_LimiteAprobado { get; set; }
        public int VentaCruzada_CodTipoDecision { get; set; }
        public int VentaCruzada_CodTipoRazon { get; set; }
        public string VentaCruzada_Observaciones { get; set; }
    }

    public class UltimusVariable
    {
        public string Nombre { get; set; }
        public string Valor { get; set; }
    }

    public class MenuEntity
    {
        public string Formulario { get; set; }
        public string Etiqueta { get; set; }
        public int Orden { get; set; }
    }
}