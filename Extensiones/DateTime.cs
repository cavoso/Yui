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
    }
}
