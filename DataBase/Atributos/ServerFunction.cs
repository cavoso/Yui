using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yui.DataBase.Atributos
{
    public class ServerFunctionAttribute : Attribute
    {
        public ServerFunctionAttribute()
        {

        }
        public ServerFunctionAttribute(string fun)
        {
            this.Funcion = fun;
        }
        public string Funcion { get; set; }
    }
}
