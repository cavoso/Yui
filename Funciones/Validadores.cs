using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Yui.Funciones
{
    public static class Validadores
    {
        public static Boolean ValidarEmail(String email)
        {
            System.Text.RegularExpressions.Regex emailRegex = new System.Text.RegularExpressions.Regex("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");
            System.Text.RegularExpressions.Match emailMatch = emailRegex.Match(email);
            return emailMatch.Success;
        }
        public static Boolean ValidarRut(String rut, String ver = "")
        {
            int Digito;
            int Contador;
            int Multiplo;
            int Acumulador;
            String Verificador;
            int Rut;
            rut = rut.Replace(".", "");
            if (rut.Contains('-'))
            {
                String[] nrut = rut.Split('-');
                rut = nrut[0];
                ver = nrut[1];
            }
            if (Information.IsNumeric(rut))
            {
                Rut = Convert.ToInt32(rut);
            }
            else
            {
                Rut = 0;
            }
            Contador = 2;
            Acumulador = 0;
            while (Rut != 0)
            {
                Multiplo = ((Rut % 10) * Contador);
                Acumulador = Acumulador + Multiplo;
                Rut = (Rut / 10);
                Contador += 1;
                if (Contador > 7)
                {
                    Contador = 2;
                }
            }
            Digito = (11 - (Acumulador % 11));
            if (Digito == 10)
            {
                Verificador = "K";
            }
            else if (Digito == 11)
            {
                Verificador = "0";
            }
            else
            {
                Verificador = Digito.ToString();
            }
            if (ver == Verificador)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static String RutFormat(String value, Boolean validar)
        {
            String _rut = "";
            String num = "";
            String dv = "";

            value = value.Replace(".", "");
            if (value.Contains('-'))
            {
                String[] ss = value.Split('-');
                num = ss[0];
                dv = ss[1];
            }
            else
            {
                if (Information.IsNumeric(value))
                {
                    if (value.Length <= 8)
                    {
                        dv = "0";
                    }
                    else
                    {
                        dv = value.Substring(value.Length - 1, 1);
                        value = value.Remove(value.Length - 1, 1);
                    }
                }
                else
                {
                    if (value.Contains('K') || value.Contains('k'))
                    {
                        value.Replace("k", "").Replace("K", "");
                        dv = "K";
                    }
                }
                num = Comunes.SeparadorMiles(Convert.ToInt32(value));
            }
            if (validar)
            {
                if (ValidarRut(num, dv))
                {
                    _rut = num + "-" + dv;
                }
                else
                {
                    _rut = "RUT NO VALIDO";
                }
            }
            else
            {
                _rut = num + "-" + dv;
            }
            return _rut;
        }
    }
}
