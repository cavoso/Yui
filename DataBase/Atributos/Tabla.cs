using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yui.DataBase.Atributos
{
    public class TablaAttribute : Attribute
    {
        public TablaAttribute()
        {

        }
        public TablaAttribute(string _nombre)
        {
            this.Nombre = _nombre;
        }
        public string Nombre { get; set; }
    }
}
