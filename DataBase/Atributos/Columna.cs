using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yui.DataBase.Atributos
{
    public class ColumnaAttribute : Attribute
    {
        public ColumnaAttribute()
        {

        }
        public ColumnaAttribute(string columName)
        {
            this.ColumName = columName;
        }
        public string ColumName { get; set; }
    }
}
