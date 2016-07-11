using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULA.Quijano.Model
{
    public class DatosMigracion
    {
        public string Incidente { get; set; }
        public decimal DepositoRepatriacion { get; set; }
        public decimal CambioCatMigratoria { get; set; }
        public decimal CarneTramite { get; set; }
        public decimal VisaMultipleEntSal { get; set; }
        public decimal Registro { get; set; }
        public string AplicaDependientes { get; set; }
        public string FechaProgRegistro { get; set; }
        public string FechaProgPresentacion { get; set; }
        public string Honorarios { get; set; }
        public string Gastos { get; set; }
        public string Total { get; set; }
        public List<Solicitante> Solicitantes { get; set; }
        public List<OpcionCheque> OpcionesCheques { get; set; }
        public List<RequisitoMigracion> Requisitos { get; set; }

        public string CodigoCliente { get; set; }
        public string NombreCliente { get; set; }
    }

    public class Solicitante
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Identificacion { get; set; }
        public string Tipo { get; set; }
        public int IdDatosMigracion { get; set; }
    }

    public class OpcionCheque
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Valor { get; set; }
    }

    public class RequisitoMigracion
    {
        public int IdRequisitoMigracion { get; set; }
        public string Tipo { get; set; }
        public string Nombre { get; set; }
        public string Instruccion { get; set; }
        public string Notarizado { get; set; }
        public string Traduccion { get; set; }
        public string Apostillado_Legalizado { get; set; }
        public int IdDatosMigracion { get; set; }
        public int CodFormasMig { get; set; }
    }

    public class Cheque
    {
        public decimal DepositoRepatriacion { get; set; }
        public decimal CambioCatMigratoria { get; set; }
        public decimal CarneTramite { get; set; }
        public decimal VisaMultipleEntSal { get; set; }
        public decimal Registro { get; set; }
    }

    public class TipoRequisito
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }
}
