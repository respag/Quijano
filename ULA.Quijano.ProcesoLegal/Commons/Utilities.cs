using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web.Configuration;
using System.Xml.Linq;
using System.Text;

namespace ULA.Quijano.ProcesoLegal.Commons
{
    public static class Utilities
    {


        public static string GetIpAdress()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return "null";
        }

        public static string GetValueParamPersonal(string value)
        {

            string[] resultadoArray = value.Split('^');

            if (resultadoArray.Length > 1)
            {

                value = resultadoArray[1];

            }

            return value;
        }


        public static string GetUltimusXmlValue(string xml, string VariableName)
        {
            Logger logger = LogManager.GetLogger("Utilities");

            logger.Error("xml=" + xml);

            string valor = string.Empty;

            XDocument xdocument = XDocument.Parse(xml);
            XElement xelement = Enumerable.FirstOrDefault<XElement>(xdocument.Root.Elements(), (Func<XElement, bool>)(q => q.Name.LocalName.ToLower() == "global"));
            if (xelement != null)
            {

                logger.Error("VariableName=" + VariableName);

                string[] nodos = VariableName.Split('.');
                for (int i = 0; i < nodos.Length; i++)
                {
                    VariableName = nodos[i];
                    logger.Error("VariableName nodos[i]=" + VariableName);
                    xelement = Enumerable.FirstOrDefault(xelement.Elements().Where((Func<XElement, bool>)(q => q.Name.LocalName.ToLower() == VariableName.ToLower())));
                }
                valor = xelement.Value;
                logger.Error("valor=" + valor);
            }
            else
                logger.Error("xelement != null");

            return valor;
        }

        public enum TipoSeccion { Decision = 1, DecisionVentaCruzadaBanvivenda = 2, DecisionVentaCruzadaCliente = 3 }

        public static string NumeroALetras(string num)
        {
            string res, dec = "";
            Int64 entero;
            int decimales;
            double nro;

            try
            {
                nro = Convert.ToDouble(num);
            }
            catch
            {
                return "";
            }

            entero = Convert.ToInt64(Math.Truncate(nro));
            decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));

            if (decimales > 0)
            {
                dec = " CON " + decimales.ToString() + "/100";
            }

            res = Utilities.toText(Convert.ToDouble(entero)) + dec;

            return res;
        }

        private static string toText(double value)
        {
            string Num2Text = "";
            value = Math.Truncate(value);

            if (value == 0) Num2Text = "CERO";
            else if (value == 1) Num2Text = "UNO";
            else if (value == 2) Num2Text = "DOS";
            else if (value == 3) Num2Text = "TRES";
            else if (value == 4) Num2Text = "CUATRO";
            else if (value == 5) Num2Text = "CINCO";
            else if (value == 6) Num2Text = "SEIS";
            else if (value == 7) Num2Text = "SIETE";
            else if (value == 8) Num2Text = "OCHO";
            else if (value == 9) Num2Text = "NUEVE";
            else if (value == 10) Num2Text = "DIEZ";
            else if (value == 11) Num2Text = "ONCE";
            else if (value == 12) Num2Text = "DOCE";
            else if (value == 13) Num2Text = "TRECE";
            else if (value == 14) Num2Text = "CATORCE";
            else if (value == 15) Num2Text = "QUINCE";
            else if (value < 20) Num2Text = "DIECI" + toText(value - 10);
            else if (value == 20) Num2Text = "VEINTE";
            else if (value < 30) Num2Text = "VEINTI" + toText(value - 20);
            else if (value == 30) Num2Text = "TREINTA";
            else if (value == 40) Num2Text = "CUARENTA";
            else if (value == 50) Num2Text = "CINCUENTA";
            else if (value == 60) Num2Text = "SESENTA";
            else if (value == 70) Num2Text = "SETENTA";
            else if (value == 80) Num2Text = "OCHENTA";
            else if (value == 90) Num2Text = "NOVENTA";
            else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " Y " + toText(value % 10);
            else if (value == 100) Num2Text = "CIEN";
            else if (value < 200) Num2Text = "CIENTO " + toText(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) Num2Text = "QUINIENTOS";
            else if (value == 700) Num2Text = "SETECIENTOS";
            else if (value == 900) Num2Text = "NOVECIENTOS";
            else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);
            else if (value == 1000) Num2Text = "MIL";
            else if (value < 2000) Num2Text = "MIL " + toText(value % 1000);
            else if (value < 1000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);
            }
            else if (value == 1000000) Num2Text = "UN MILLON";
            else if (value < 2000000) Num2Text = "UN MILLON " + toText(value % 1000000);
            else if (value < 1000000000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);
            }
            else if (value == 1000000000000) Num2Text = "UN BILLON";
            else if (value < 2000000000000) Num2Text = "UN BILLON " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            else
            {
                Num2Text = toText(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            }

            return Num2Text;
        }

        public static int GetAge(DateTime vDateOfBirth)
        {
            return new DateTime(DateTime.Now.Subtract(vDateOfBirth).Ticks).Year - 1;
        }

    }
}