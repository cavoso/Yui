using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
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
        public DataSet DataSet
        {
            get
            {
                return _DS;
            }
        }
        public DataTable Datatable
        {
            get
            {
                return _DS.Tables[0];
            }
        }
        public int NumRows
        {
            get
            {
                return Datatable.Rows.Count;
            }
        }
        public int NumCol
        {
            get
            {
                return Datatable.Columns.Count;
            }
        }    
        public int Position { get; set; }
        public Boolean Status
        {
            get
            {
                if (NumRows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public Boolean IsOk
        {
            get
            {
                return Status;
            }
        }
    }
    
}
