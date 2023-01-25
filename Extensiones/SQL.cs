using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Yui.DataBase;
using Yui.DataBase.Atributos;

namespace Yui.Extensiones
{
    public static class SQLExtension
    {
        public static T Select<T>(this T objeto, SQL sql)
        {
            Type temp = typeof(T);
            var Objattrs = temp.GetCustomAttributes(true);
            foreach (var item in Objattrs)
            {
                if (item.GetType().Name == "TablaAttribute")
                {
                    sql.Tabla(((TablaAttribute)item).Nombre);
                }
            }
            PropertyInfo IdProperty = temp.GetProperties().Where(x => x.GetCustomAttributes().Where(y => y.GetType().Name == "IdAttribute").ToList().Count() == 1).ToList().First<PropertyInfo>();
            string columna = "";
            foreach (var attr in IdProperty.GetCustomAttributes())
            {
                if (attr.GetType().Name == "ColumnaAttribute")
                {
                    columna = ((ColumnaAttribute)attr).ColumName;
                }
            }
            sql.Where(columna, IdProperty.GetValue(objeto));
            List<T> tmp = sql.Get<T>();
            return tmp.FirstOrDefault();
        }
        public static T Select<T>(this T objeto, SQL sql, Dictionary<string, object> where)
        {
            Type temp = typeof(T);
            var Objattrs = temp.GetCustomAttributes(true);
            foreach (var item in Objattrs)
            {
                if (item.GetType().Name == "TablaAttribute")
                {
                    sql.Tabla(((TablaAttribute)item).Nombre);
                }
            }
            sql.Where(where);
            List<T> tmp = sql.Get<T>();
            return tmp.FirstOrDefault();
        }
        public static T Select<T>(this T objeto, SQL sql, Dictionary<string, object> where, string select)
        {
            Type temp = typeof(T);
            var Objattrs = temp.GetCustomAttributes(true);
            foreach (var item in Objattrs)
            {
                if (item.GetType().Name == "TablaAttribute")
                {
                    sql.Tabla(((TablaAttribute)item).Nombre);
                }
            }
            sql.Select(select);
            sql.Where(where);
            List<T> tmp = sql.Get<T>();
            return tmp.FirstOrDefault();
        }
        public static List<T> SelectList<T>(this List<T> objeto, SQL sql, Dictionary<string, object> where)
        {
            Type temp = typeof(T);
            var Objattrs = temp.GetCustomAttributes(true);
            foreach (var item in Objattrs)
            {
                if (item.GetType().Name == "TablaAttribute")
                {
                    sql.Tabla(((TablaAttribute)item).Nombre);
                }
            }
            sql.Where(where);
            return sql.Get<T>();
        }
      public static T Insert<T>(this T objeto, SQL sql)
        {
            Type temp = typeof(T);
            var Objattrs = temp.GetCustomAttributes(true);
            foreach (var item in Objattrs)
            {
                if(item.GetType().Name == "TablaAttribute")
                {
                    sql.Tabla(((TablaAttribute)item).Nombre);
                }
            }
            foreach (PropertyInfo item in temp.GetProperties())
            {
                object valor = null;
                switch (item.PropertyType.Name)
                {
                    case "DateTime":
                        if (((DateTime)item.GetValue(objeto)) == DateTime.MinValue)
                        {
                            valor = null;
                        }
                        else
                        {
                            valor = ((DateTime)item.GetValue(objeto)).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        break;
                    default:
                        valor = item.GetValue(objeto);
                        break;
                }
                var atributos = item.GetCustomAttributes();
                if (valor != null)
                {
                    bool ignore = false;
                    string columna = "";
                    string servfunc = "";
                    foreach (var attr in atributos)
                    {
                        if (attr.GetType().Name == "ColumnaAttribute")
                        {
                            columna = ((ColumnaAttribute)attr).ColumName;
                        }
                        if (attr.GetType().Name == "ServerFunctionAttribute")
                        {
                            servfunc = ((ServerFunctionAttribute)attr).Funcion;
                        }
                        if (attr.GetType().Name == "IgnoreAttribute" || attr.GetType().Name == "OtraTablaAttribute" || attr.GetType().Name == "IdAttribute")
                        {
                            ignore = true;
                        }
                    }
                    if (!ignore)
                    {
                        if (servfunc != "")
                        {
                            sql.SetCampos(columna, servfunc);
                        }
                        else
                        {
                            sql.SetCampos(columna, valor);
                        }
                        
                    }
                }                
            }
            var t = sql.Insert();
            long nueId = sql.LastId;
            PropertyInfo IdProperty = temp.GetProperties().Where(x => x.GetCustomAttributes().Where(y => y.GetType().Name == "IdAttribute").ToList().Count() == 1).ToList().First<PropertyInfo>();
            if (IdProperty.PropertyType.Name == "Int64")
            {
                IdProperty.SetValue(objeto, nueId, null);
            }
            if (IdProperty.PropertyType.Name == "Int32")
            {
                IdProperty.SetValue(objeto, Convert.ToInt32(nueId), null);
            }
            return objeto;
        }
        public static T Update<T>(this T objeto, SQL sql)
        {
            //sql.Preserve = true;
            Type temp = typeof(T);
            var Objattrs = temp.GetCustomAttributes(true);
            foreach (var item in Objattrs)
            {
                if (item.GetType().Name == "TablaAttribute")
                {
                    sql.Tabla(((TablaAttribute)item).Nombre);
                }
            }
            foreach (PropertyInfo item in temp.GetProperties())
            {
                object valor = null;
                switch (item.PropertyType.Name)
                {
                    case "DateTime":
                        if (((DateTime)item.GetValue(objeto)) == DateTime.MinValue)
                        {
                            valor = null;
                        }
                        else
                        {
                            valor = ((DateTime)item.GetValue(objeto)).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        break;
                    default:
                        valor = item.GetValue(objeto);
                        break;
                }
                var atributos = item.GetCustomAttributes();
                if (valor != null)
                {
                    bool ignore = false;
                    string columna = "";
                    string servfunc = "";
                    bool id = false;
                    foreach (var attr in atributos)
                    {
                        if (attr.GetType().Name == "IdAttribute")
                        {
                            id = true;
                        }
                        if (attr.GetType().Name == "ColumnaAttribute")
                        {
                            columna = ((ColumnaAttribute)attr).ColumName;
                        }
                        if (attr.GetType().Name == "ServerFunctionAttribute")
                        {
                            servfunc = ((ServerFunctionAttribute)attr).Funcion;
                        }
                        if (attr.GetType().Name == "IgnoreAttribute" || attr.GetType().Name == "OtraTablaAttribute")
                        {
                            ignore = true;
                        }
                    }
                    if (id)
                    {
                        sql.Where(columna, valor);
                    }
                    else
                    {
                        if (!ignore)
                        {
                            if (servfunc != "")
                            {
                                sql.SetCampos(columna, servfunc);
                            }
                            else
                            {
                                sql.SetCampos(columna, valor);
                            }

                        }
                    }
                    
                }
            }

            sql.Update();
            return objeto;
        }
        
    }
}
