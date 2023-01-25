using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yui.DataBase.Atributos
{
    public class OtraTablaAttribute : Attribute
    {
        public OtraTablaAttribute()
        {
            this.Activo = true;
        }
        public bool Activo { get; set; }
    }
}
