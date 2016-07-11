using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULA.Quijano.Model
{
    public class DatosFundacion
    {
        public string Incidente { get; set; }
        public string FundacionExistente { get; set; }
        public string CodFundacion { get; set; }
        public string NombreFundacion { get; set; }
        public string TipoFundacion { get; set; }
        public string CodCia { get; set; }
        public string DescripcionCia { get; set; }
        public string Fundador { get; set; }
        public string CodBajoLeyes { get; set; }
        public string DescripcionBajoLeyes { get; set; }
        public bool IsPendienteCambioClt { get; set; }
        public string PendienteCambioClt { get; set; }
        public string CodigoCliente { get; set; }
        public string NombreCliente { get; set; }
        public string Duracion { get; set; }
        public string RespAnualidadCodigo { get; set; }
        public string RespAnualidadNombre { get; set; }
        public string NoReg { get; set; }
        public string NombreAnterior { get; set; }
        public string RUC { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public string Idioma { get; set; }
        public string Oficina { get; set; }
        public string PropositoFundacion { get; set; }
        public string Patrimonio { get; set; }
        public string Estatus { get; set; }
        public string ProveeDirectores { get; set; }
        public int Solicitud { get; set; }

        //Tarifa
        public string Tasa { get; set; }
        public string AgenteResid { get; set; }
        public string DirectoresQA { get; set; }
        public string Honorarios { get; set; }
        public string Gastos { get; set; }
        public string Total { get; set; }

        public List<ConsejoFundacional> Consejos { get; set; }
        public List<AnalisisAntiguedad> AnalisisAntiguedad { get; set; }
    }

    public class CheckListControlFundacion
    {
        public string Incidente { get; set; }
        public int IdDatosFundacion { get; set; }
        public string CodigoFundacion { get; set; }
        public string ActaFundacional { get; set; }
        public string Reglamentos { get; set; }
        public string DesignacionProtector { get; set; }
        public string PoderGenEsp { get; set; }
        public string PINombrePrimerBen { get; set; }
        public string PruebaDomPrimerBen { get; set; }
        public string CopiaPasIdPrimerBen { get; set; }
        public string CartaRefBancaria { get; set; }
        public string CartaRefProfesional { get; set; }
        public string AccountingRecordLocation { get; set; }
        public string EPINombrePrimerBen { get; set; }
        public string DireccionPrimerBen { get; set; }
        public int Solicitud { get; set; }
    }
}
