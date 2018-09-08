using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yui.DataBase.Estructura
{
    public class ObjSQL
    {
        protected DataSet _DS;

        public ObjSQL()
        {

        }
        public ObjSQL(DataSet ds)
        {
            CallDS(ds);
        }
        public void CallDS(DataSet ds)
        {
            _DS = ds;
        }
    }
}
