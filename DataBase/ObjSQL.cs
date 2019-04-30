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
        public List<Dictionary<String, object>> Lista;
        public ObjSQL()
        {
            Lista = new List<Dictionary<string, object>>();
        }
        public ObjSQL(DataSet ds)
        {
            Lista = new List<Dictionary<string, object>>();
            CallDS(ds);
        }
        public void CallDS(DataSet ds)
        {
            _DS = ds;
            Lista = Datatable.AsEnumerable().Select(
                row => Datatable.Columns.Cast<DataColumn>().ToDictionary(
                    column => column.ColumnName,
                    column => row[column]
                )
            ).ToList();
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
        public void CheckColumns()
        {
            foreach (Dictionary<string, Object> item in Lista)
            {
                foreach (KeyValuePair<string, Object> it in item)
                {
                    Console.Out.WriteLine("Key: " + it.Key + "Valor: " + it.Value);
                }
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
