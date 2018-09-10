using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yui.DataBase.Estructura
{
    public class ObjSQL: DynamicObject
    {
        protected DataSet _DS;
        protected Dictionary<int, Object> Datos;
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
            Datos = new Dictionary<int, Object>();
            int i = 0;
            foreach (DataRow row in _DS.Tables[0].Rows)
            {
                OBJ o = new OBJ();
                int a = 0;
                foreach (DataColumn col in _DS.Tables[0].Columns)
                {
                    //o.AddProperty(col.ColumnName, row[a]);
                    //o.nombre = row[a];
                    //o.Add(col.ColumnName, row[a]);
                    a += 1;
                }
                Datos.Add(i, o);
                i += 1;
            }
        }
        public DataSet DataSet()
        {
            return _DS;
        }
        public DataTable DataTable()
        {
            return _DS.Tables[0];
        }
        public int NumRows
        {
            get
            {
                return DataTable().Rows.Count;
            }
        }
        public int NumCol
        {
            get
            {
                return DataTable().Columns.Count;
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
    public class OBJ: DynamicObject
    {
        Dictionary<String, Object> dictionary = new Dictionary<string, object>();

        public int Count
        {
            get
            {
                return dictionary.Count;
            }
        }

        public void SetProperty(String nombre, object valor)
        {
            dictionary[nombre.ToLower()] = valor;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            //return base.TryGetMember(binder, out result);
            String name = binder.Name.ToLower();
            return dictionary.TryGetValue(name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            //return base.TrySetMember(binder, value);

            dictionary[binder.Name.ToLower()] = value;

            return true;
        }
    }

}
