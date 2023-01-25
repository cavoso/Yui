using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Globalization;

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
        public static String HiddenEmail(this string aString)
        {
            string[] email = aString.Split('@');
            string simple = "";
            if (email[0].Length <= 5)
            {
                simple = email[0].Substring(0, 3);
            }
            else
            {
                simple = email[0].Substring(0, 5);
            }

            return string.Format("{0}******@{1}", simple, email[1]);
        }
        public static String Base64MimeType(this string aString)
        {
            if (aString.Contains("base64,"))
            {
                string[] datos = aString.Split(new string[] { "base64," }, StringSplitOptions.None);
                string[] data = datos[0].Split(':');
                return data[1].Replace(";", "");
            }
            else
            {
                return "text/plain";
            }
        }
        public static String Base64String(this string aString)
        {
            return aString.Replace(string.Format("data:{0};base64,", aString.Base64MimeType()), "");
        }
        public static string RemoveAccents(this string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }
    }
}
