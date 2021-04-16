using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Yui.Extensiones
{
    public static class StringExtension
    {
        public static String SeparadorMiles(this String aString, int dec = 0, String signo = "")
        {
            try
            {
                return Funciones.Comunes.SeparadorMiles(Convert.ToInt32(aString), dec, signo);
            }
            catch (Exception)
            {
                return aString;              
            }
            
        }
        public static String OnlyFecha(this String aString)
        {
            try
            {
                return Funciones.Times.FechaOnly(Convert.ToDateTime(aString));
            }
            catch (Exception)
            {
                return aString;
            }
            
        }
        public static String FormatoRut(this String aString)
        {
            try
            {
                return Funciones.Validadores.RutFormat(aString, false);
            }
            catch (Exception)
            {
                return aString;
            }
        }
        public static String ToRoman(this String aString)
        {
            try
            {
                return Funciones.Comunes.ToRoman(Convert.ToInt32(aString));
            }
            catch (Exception)
            {
                return aString;
            }
        }
        /// <summary>
        /// Recibe el nombre en inglés y lo transforma a español
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static String DiaDeLaSemana(this String d)
        {
            try
            {
                return Funciones.Times.DiadSemana(d);
            }
            catch (Exception)
            {
                return d;
            }
        }
        public static Boolean IsNumeric(this String aString)
        {
            int i;
            bool bNum = int.TryParse(aString, out i);
            return bNum;
        }
        public static int Number(this String aString)
        {
            try
            {
                return Convert.ToInt32(aString);
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
