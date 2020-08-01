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
        protected List<Dictionary<String, YUIObject>> Lista;
        public ObjSQL()
        {
            Lista = new List<Dictionary<String, YUIObject>>();
        }
        public ObjSQL(DataSet ds)
        {
            Lista = new List<Dictionary<String, YUIObject>>();
            CallDS(ds);
        }
        public void CallDS(DataSet ds)
        {
            _DS = ds;
            Lista = Tabla.AsEnumerable().Select(
                row => Tabla.Columns.Cast<DataColumn>().ToDictionary(
                    column => column.ColumnName,
                    column => new YUIObject(row[column])
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
        public DataTable Tabla
        {
            get
            {
                if (_DS is null)
                {
                    return new DataTable();
                }
                else
                {
                    return _DS.Tables[0];
                }
                
            }
        }
        public Dictionary<String, YUIObject> Row(int r = 0)
        {
            return Lista[r];
        }
        public YUIObject Row(int r, String c = "")
        {
            if (c != "")
            {
                Dictionary<String, YUIObject> d = Row(r);
                return d[c];
            }
            else
            {
                return new YUIObject();
            }
        }
        public List<Dictionary<String, YUIObject>> Result()
        {
            return Lista;
        }
        public Dictionary<String, YUIObject> Buscar(String dato)
        {
            var j = Lista.SelectMany(x => x).Where(x => x.Value.String.Contains(dato)).FirstOrDefault();
            var d = Lista.Select(x => x).Where(x => x[j.Key].String == dato).ToList();
            Dictionary<String, YUIObject> h = d[0];
            return h;
        }
        public Dictionary<String, YUIObject> Buscar(String campo, String dato)
        {           
            var d = Lista.Select(x => x).Where(x => x[campo].String == dato).ToList();
            Dictionary<String, YUIObject> h = d[0];
            return h;
        }
        public int NumRows
        {
            get
            {
                try
                {
                    return Tabla.Rows.Count;
                }
                catch (Exception)
                {
                    return 0;              
                }
                
            }
        }
        public int NumCol
        {
            get
            {
                try
                {
                    return Tabla.Columns.Count;
                }
                catch (Exception)
                {
                    return 0;
                }
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
