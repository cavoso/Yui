using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yui.DataBase.Atributos
{
    public class CreationDateAttribute : Attribute
    {
        public CreationDateAttribute()
        {
            this.Activo = true;
        }
        public bool Activo { get; set; }
    }
}
