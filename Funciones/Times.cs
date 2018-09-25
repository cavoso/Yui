using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Yui.Funciones
{
    public static class Times
    {
        public static String FechaFormat(DateTime f, TipoFecha t)
        {
            String fecha = "";
            switch (t)
            {
                case TipoFecha.ddMMyyyySlash:
                    fecha = Strings.Format(f, "dd/MM/yyyy");
                    break;
                case TipoFecha.ddMMyyyySlashHHmmss:
                    fecha = Strings.Format(f, "dd/MM/yyyy HH:mm:ss");
                    break;
                case TipoFecha.yyyyMMddGuion:
                    fecha = Strings.Format(f, "yyyy-MM-dd");
                    break;
                case TipoFecha.yyyyMMddGuionHHmmss:
                    fecha = Strings.Format(f, "yyyy-MM-dd HH:mm:ss");
                    break;
                default:
                    fecha = Strings.Format(f, "dd/MM/yyyy");
                    break;
            }
            return fecha;
        }
        public static String FechaOnly(DateTime f)
        {
          
            return FechaFormat(f, TipoFecha.yyyyMMddGuion);
        }
        public static String DiadSemana(String d)
        {
            String dia = "";
            switch (d)
            {
                case "Sunday":
                    dia = DiaSemana[0];
                    break;
                case "Monday":
                    dia = DiaSemana[1];
                    break;
                case "Tuesday":
                    dia = DiaSemana[2];
                    break;
                case "Wednesday":
                    dia = DiaSemana[3];
                    break;
                case "Thursday":
                    dia = DiaSemana[4];
                    break;
                case "Friday":
                    dia = DiaSemana[5];
                    break;
                case "Saturday":
                    dia = DiaSemana[6];
                    break;
                default:
                    break;
            }
            return dia;
        }
        /// <summary>
        /// Lista de dias de la semana inicia en 0 = domingo
        /// </summary>
        public static Dictionary<int, String> DiaSemana = new Dictionary<int, String>()
        {
            {0, "Domingo"},
            {1, "Lunes"},
            {2, "Martes"},
            {3, "Miercoles"},
            {4, "Jueves"},
            {5, "Viernes"},
            {6, "Sabado"}
        };
        public static Dictionary<int, String> Meses = new Dictionary<int, string>()
        {
            {1, "Enero" },
            {2, "Febrero" },
            {3, "Marzo" },
            {4, "Abril" },
            {5, "Mayo" },
            {6, "Junio" },
            {7, "Julio" },
            {8, "Agosto" },
            {9, "Septiembre" },
            {10, "Octubre" },
            {11, "Noviembre" },
            {12, "Diciembre" }
        };
    }
}
