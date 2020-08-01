using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Yui.DataBase
{
    public class Builder
    {
        private String _selectRaw;
        private String _tabla;
        private String _whereRaw;
        private String _orderby;
        private String[] _comparadores;
        private Dictionary<String, Object> _campos;
        public TipoQuery Tipo { get; set; }
        public TipoConexion TipoDB { get; set; }

        public Builder()
        {
            NewQuery();
            _comparadores = new String[]
            {
                "=",
                "<>",
                "!=",
                ">",
                "<",
                ">=",
                "<=",
                "IS NOT",
                "IS"
            };
        }
        public void NewQuery()
        {
            _selectRaw = "";
            _tabla = "";
            _whereRaw = "";
            _orderby = "";
            _campos = new Dictionary<string, object>();
            Tipo = TipoQuery.SELECT;
        }
        public void NewQuery(TipoQuery tipo)
        {
            _selectRaw = "";
            _tabla = "";
            _whereRaw = "";
            _orderby = "";
            Tipo = tipo;
        }

        public void Tabla(String tabla)
        {
            _tabla = tabla;
        }
        public void Select(String campos)
        {
            SetCampos(campos);
        }
        public void SetCampos(String campos)
        {
            foreach (var item in campos.Split(','))
            {
                SetCampos(item, new object());
            }
        }
        public void SetCampos(String campo, Object valor)
        {
            if (_campos.ContainsKey(campo.ToLower()))
            {
                _campos[campo.ToLower()] = valor;
            }
            else
            {
                _campos.Add(campo.ToLower(), valor);
            }
        }
        public void SetCampos(Dictionary<String, Object> campos)
        {
            foreach (var item in campos)
            {
                SetCampos(item.Key, item.Value);
            }
        }
        public void Where(String campo, Object valor, String comparador = "=")
        {
            if (_comparadores.Contains<String>(comparador))
            {
                _WhereSet("AND " + campo + comparador, CheckValor(valor));
            }
            else
            {
                Console.Out.WriteLine("El Comparador ingresado (" + comparador.ToString() + ") no es admitido");
            }
        }
        public void Where(Dictionary<String, Object> w)
        {
            foreach (var item in w)
            {
                Where(item.Key, item.Value);
            }
        }
        public void OrWhere(String campo, Object valor, String comparador = "=")
        {
            if (_comparadores.Contains<String>(comparador))
            {
                _WhereSet("OR " + campo + comparador, CheckValor(valor));
            }
            else
            {
                Console.Out.WriteLine("El Comparador ingresado (" + comparador.ToString() + ") no es admitido");
            }
        }
        public void OrWhere(Dictionary<String, Object> w)
        {
            foreach (var item in w)
            {
                //_Where.Add(item.Key, item.Value);
                OrWhere(item.Key, item.Value);
            }
        }
        public void NotWhere(String campo, Object valor, String comparador = "=")
        {
            if (_comparadores.Contains<String>(comparador))
            {
                _WhereSet("NOT " + campo + comparador, CheckValor(valor));
            }
            else
            {
                Console.Out.WriteLine("El Comparador ingresado (" + comparador.ToString() + ") no es admitido");
            }
        }
        public void InWere(String campo, String en)
        {
            //_WhereSet(campo + " IN ", en);
            _whereRaw += (campo + " IN " + en);
        }
        public void NotInWere(String campo, String en)
        {
            //_WhereSet(campo + " IN ", en);
            _whereRaw += (campo + " NOT IN " + en);
        }
        public void Like(String campo, Object valor, TipoLike like = TipoLike.Contiene)
        {
            switch (like)
            {
                case TipoLike.Contiene:
                    _WhereSet(campo + " LIKE ", "%" + CheckValor(valor) + "%");
                    break;
                case TipoLike.Comienza:
                    _WhereSet(campo + " LIKE ", CheckValor(valor) + "%");
                    break;
                case TipoLike.Termina:
                    _WhereSet(campo + " LIKE ", "%" + CheckValor(valor));
                    break;
                default:
                    _WhereSet(campo + " LIKE ", "%" + CheckValor(valor) + "%");
                    break;
            }
        }
        public void OrLike(String campo, Object valor, TipoLike like = TipoLike.Contiene)
        {
            switch (like)
            {
                case TipoLike.Contiene:
                    _WhereSet("OR " + campo + " LIKE ", "%" + CheckValor(valor) + "%");
                    break;
                case TipoLike.Comienza:
                    _WhereSet("OR " + campo + " LIKE ", CheckValor(valor) + "%");
                    break;
                case TipoLike.Termina:
                    _WhereSet("OR " + campo + " LIKE ", "%" + CheckValor(valor));
                    break;
                default:
                    _WhereSet("OR " + campo + " LIKE ", "%" + CheckValor(valor) + "%");
                    break;
            }
        }
        public void NotLike(String campo, Object valor, TipoLike like = TipoLike.Contiene)
        {
            switch (like)
            {
                case TipoLike.Contiene:
                    _WhereSet(campo + "NOT LIKE", "%" + CheckValor(valor) + "%");
                    break;
                case TipoLike.Comienza:
                    _WhereSet(campo + "NOT LIKE", CheckValor(valor) + "%");
                    break;
                case TipoLike.Termina:
                    _WhereSet(campo + "NOT LIKE", "%" + CheckValor(valor));
                    break;
                default:
                    _WhereSet(campo + "NOT LIKE", "%" + CheckValor(valor) + "%");
                    break;
            }
        }
        private void _WhereSet(String campo, String valor)
        {
            if (_whereRaw == "")
            {
                _whereRaw = campo.Replace("OR ", "").Replace("NOT ", "").Replace("AND", "") + "'" + valor + "'";
            }
            else
            {
                _whereRaw += " " + campo + "'" + valor + "'";
            }
        }
        private String CheckValor(Object valor)
        {
            String temp = "";
            if (valor is null)
            {
                temp = "";
            } else
            {
                if (
                    valor.GetType() == typeof(decimal) ||
                    valor.GetType() == typeof(float) ||
                    valor.GetType() == typeof(double)
                    )
                {
                    temp = valor.ToString().Replace(",", ".");
                }
                else if (valor.GetType() == typeof(int))
                {
                    temp = valor.ToString();
                }
                else if (valor.GetType() == typeof(DateTime))
                {
                    temp = Funciones.Times.FechaFormat(Convert.ToDateTime(valor), TipoFecha.yyyyMMddGuionHHmmss);
                }
                else
                {
                    temp = valor.ToString();
                }
            }
            
            
            return temp;
        }
        public void WhereRaw(String w)
        {
            _whereRaw = w;
        }
        public void OrderBy(String campo, String dir)
        {
            if (_orderby == "")
            {
                _orderby = campo + " " + dir.ToUpper();
            }
            else
            {
                _orderby += ", " +campo + " " + dir.ToUpper();
            }
        }
        public void OrderByASC(String campo)
        {
            OrderBy(campo, "ASC");
        }
        public void OrderByDESC(String campo)
        {
            OrderBy(campo, "DESC");
        }
        public String Generar()
        {
            String sql = "";
            switch (Tipo)
            {
                case TipoQuery.SELECT:
                    sql = "SELECT ";
                    String ca = "";
                    foreach (var item in _campos)
                    {
                        if (ca == "")
                        {
                            ca = item.Key;
                        }
                        else
                        {
                            ca = ca + ", " + item.Key;
                        }
                    }
                    if (ca != "")
                    {
                        sql += ca + " ";
                    }
                    else
                    {
                        sql += "* ";
                    }
                    sql += "FROM " + _tabla;
                    if (_whereRaw != "")
                    {
                        sql += " WHERE " + _whereRaw;
                    }
                    if (_orderby != "")
                    {
                        sql += " ORDER BY " + _orderby;
                    }
                    break;
                case TipoQuery.UPDATE:
                    ///UPDATE Customers SET ContactName = 'Alfred Schmidt', City = 'Frankfurt' WHERE CustomerID = 1;
                    sql = "UPDATE " + _tabla;
                    String c = "";
                    foreach (var item in _campos)
                    {
                        if (c == "")
                        {
                            c = item.Key + " = '" + CheckValor(item.Value) + "'";
                        }
                        else
                        {
                            c += ", " + item.Key + " = '" + CheckValor(item.Value) + "'";
                        }
                    }
                    sql += " SET " + c;
                    if (_whereRaw != "")
                    {
                        sql += " WHERE " + _whereRaw;
                    }
                    break;
                case TipoQuery.INSERT:
                    //INSERT INTO CUSTOMERS (ID,NAME,AGE,ADDRESS,SALARY)
                    //VALUES(6, 'Komal', 22, 'MP', 4500.00);
                    sql = "INSERT INTO " + _tabla;
                    string campos = "";
                    string valores = "";
                    foreach (var item in _campos)
                    {
                        if (campos =="")
                        {
                            campos = item.Key;
                            valores = "'" + CheckValor(item.Value) + "'";
                        }
                        else
                        {
                            campos += ", " + item.Key;
                            valores += ", '" + CheckValor(item.Value) + "'";
                        }
                    }
                    if (campos != "" & valores != "")
                    {
                        sql += "(" + campos + ")";
                        sql += "VALUES(" + valores + ")";
                    }
                    break;
                case TipoQuery.DELETE:
                    sql = "DELETE FROM " + _tabla ;
                    if (_whereRaw != "")
                    {
                        sql += " WHERE " + _whereRaw;
                    }
                   
                    break;
                default:
                    break;
            }
            sql += ";";
            //limpiamos las funciones especificas
            sql = sql.Replace("'GETDATE()'", "GETDATE()");
            //Console.WriteLine(sql);
            return sql;
        }
    }
}
