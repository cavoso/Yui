using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Yui.Extensiones
{
    public static class DateTimeExtension
    {
        public static String OnlyFecha(this DateTime f)
        {
            return Funciones.Times.FechaOnly(f);
        }
        public static String MesEnPalabras(this DateTime f)
        {
            return Funciones.Times.Meses[f.Month];
        }
        public static String DiaDeLaSemana(this DateTime f)
        {
            return Funciones.Times.DiaSemana[(int)f.DayOfWeek];
        }
        public static Double TimeStamp(this DateTime f)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = f - epoch;
            double timestamp = timeSpan.TotalSeconds;
            return (timestamp * 1000);
        }
    }
}
