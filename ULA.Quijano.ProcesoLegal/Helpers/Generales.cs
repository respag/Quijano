using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ULA.Quijano.ProcesoLegal.Helpers
{
    public static class Generales
    {

        public static List<string> OperadoresLogicos()
        {

            return new List<string> { ">", "<", "=", "<>" };
        }

        public static string VerifyStringObject(string valor)
        {

            if (valor != string.Empty)
            {
                return valor;
            }
            else
                return string.Empty;
        }

        public static long? VerifyNullableLongObject(long? valor)
        {

            if (valor != 0 && valor != null)
            {
                return valor;
            }
            else

                return null;
        }

        public static int? VerifyNullableIntObject(int? valor)
        {

            if (valor != 0 && valor != null)
            {
                return valor;
            }
            else

                return null;
        }

    }


}