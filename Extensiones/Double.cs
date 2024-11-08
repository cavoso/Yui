using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yui.Extensiones
{
    public static class DoubleExtension
    {
        public static String SeparadorMiles(this double aString, int dec = 0, String signo = "")
        {
            try
            {
                return Funciones.Comunes.SeparadorMiles(Convert.ToInt32(aString), dec, signo);
            }
            catch (Exception)
            {
                return aString.ToString();
            }

        }
    }
}
