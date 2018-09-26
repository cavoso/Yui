using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yui.Extensiones
{
    public static class IntExtension
    {
        public static String SeparadorMiles(this int aInt, String signo = "")
        {
            return Funciones.Comunes.SeparadorMiles(aInt, 0, signo);
        }
        public static String ToRoman(this int aInt)
        {
            return Funciones.Comunes.ToRoman(aInt);
        }
    }
}
