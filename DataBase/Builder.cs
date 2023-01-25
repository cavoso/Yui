using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yui.DataBase
{
    /// <summary>
    /// Constructor de consultas SQL, solo funciona dentro del Objeto SQL
    /// </summary>
    public class Builder
    {
        //private String _selectRaw;
        private String _tabla;
        private String _whereRaw;
        private String _orderby;
        private String _limit;
        private List<string> _join = new List<string>();
        private String[] _comparadores;
        private Dictionary<String, Object> _campos;
        private Boolean _group = false;
        private Boolean _distinct = false;
        private String _groupby;
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
            //_selectRaw = "";
            _tabla = "";
            _whereRaw = "";
            _join = new List<string>();
            _orderby = "";
            _limit = "";
            _campos = new Dictionary<string, object>();
            _group = false;
            _distinct = false;
            _groupby = "";
            Tipo = TipoQuery.SELECT;
        }
        public void NewQuery(TipoQuery tipo)
        {
            //_selectRaw = "";
            _tabla = "";
            _whereRaw = "";
            _join = new List<string>();
            _orderby = "";
            _limit = "";
            _groupby = "";
            Tipo = tipo;
            _group = false;
            _distinct = false;
        }
        public void Tabla(String tabla)
        {
            _tabla = tabla;
        }
        public void Join(String tabla, string condicion, TipoJoin tipo = TipoJoin.INNER)
        {
            string xjoin = "";
            switch (tipo)
            {
                case TipoJoin.INNER:
                    xjoin = string.Format("INNER JOIN {0} ON {1}", tabla, condicion);
                    break;
                case TipoJoin.LEFT:
                    xjoin = string.Format("LEFT JOIN {0} ON {1}", tabla, condicion);
                    break;
                case TipoJoin.RIGHT:
                    xjoin = string.Format("RIGHT JOIN {0} ON {1}", tabla, condicion);
                    break;
                case TipoJoin.FULL:
                    xjoin = string.Format("FULL JOIN {0} ON {1}", tabla, condicion);
                    break;
                case TipoJoin.LEFTOUTER:
                    xjoin = string.Format("LEFT OUTER JOIN {0} ON {1}", tabla, condicion);
                    break;
                case TipoJoin.RIGHTOUTER:
                    xjoin = string.Format("RIGHT OUTER JOIN {0} ON {1}", tabla, condicion);
                    break;
                case TipoJoin.FULLOUTER:
                    xjoin = string.Format("FULL OUTER JOIN {0} ON {1}", tabla, condicion);
                    break;
            }
            if (_join.Count() > 0)
            {
                _join.Add(" " + xjoin);
            }
            else
            {
                _join.Add(xjoin);
            }
        }
        public void Distinct()
        {
            _distinct = true;
        }
        public void Distinct(String campos)
        {
            _distinct = true;
            SetCampos(campos);
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
        public void WhereIsNull(string campo)
        {
            _WhereSet("AND " + campo, "IS NULL");
        }
        public void WhereIsNotNull(string campo)
        {
            _WhereSet("AND " + campo, "IS NOT NULL");
        }
        public void OrWhereIsNull(string campo)
        {
            _WhereSet("OR " + campo, "IS NULL");
        }
        public void OrWhereIsNotNull(string campo)
        {
            _WhereSet("OR " + campo, "IS NOT NULL");
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
        public void YearWhere(String campo, Object valor, String comparador = "=")
        {
            if (_whereRaw != "")
            {
                _whereRaw += string.Format(" AND YEAR({0}){1}{2}", campo, comparador, valor);
            }
            else
            {
                _whereRaw += string.Format("YEAR({0}){1}{2}", campo, comparador, valor);
            }
           
        }
        public void MonthWhere(String campo, Object valor, String comparador = "=")
        {
            if (_whereRaw != "")
            {
                _whereRaw += string.Format(" AND MONTH({0}){1}{2}", campo, comparador, valor);
            }
            else
            {
                _whereRaw += string.Format("MONTH({0}){1}{2}", campo, comparador, valor);
            }
               
        }
        public void DayWhere(String campo, Object valor, String comparador = "=")
        {
            if (_whereRaw != "")
            {
                _whereRaw += string.Format(" AND DAY({0}){1}{2}", campo, comparador, valor);
            }
            else
            {
                _whereRaw += string.Format("DAY({0}){1}{2}", campo, comparador, valor);
            }                
        }
        public void DATEADDWhere(string campo, string interval, int number, string date = "getdate()", String comparador = "=")
        {
            if (_whereRaw != "")
            {
                _whereRaw += string.Format(" AND {0} {1} DATEADD({2}, {3}, {4})", campo, comparador, interval, number, date);
            }
            else
            {
                _whereRaw += string.Format("{0} {1} DATEADD({2}, {3}, {4})", campo, comparador, interval, number, date);
            }
        }
        public void Group_Start()
        {
            if (_whereRaw != "")
            {
                _whereRaw += " AND (";
            }
            else
            {
                _whereRaw += " (";
            }
           
            _group = true;
        }
        public void Group_End()
        {
            _whereRaw += ")";
        }
        public void Like(String campo, Object valor, TipoLike like = TipoLike.Contiene)
        {
            switch (like)
            {
                case TipoLike.Contiene:
                    _WhereSet("AND " + campo + " LIKE ", "%" + CheckValor(valor) + "%");
                    break;
                case TipoLike.Comienza:
                    _WhereSet("AND " + campo + " LIKE ", CheckValor(valor) + "%");
                    break;
                case TipoLike.Termina:
                    _WhereSet("AND " + campo + " LIKE ", "%" + CheckValor(valor));
                    break;
                default:
                    _WhereSet("AND " + campo + " LIKE ", "%" + CheckValor(valor) + "%");
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
                    _WhereSet("AND " + campo + " NOT LIKE ", "%" + CheckValor(valor) + "%");
                    break;
                case TipoLike.Comienza:
                    _WhereSet("AND " + campo + " NOT LIKE ", CheckValor(valor) + "%");
                    break;
                case TipoLike.Termina:
                    _WhereSet("AND " + campo + " NOT LIKE ", "%" + CheckValor(valor));
                    break;
                default:
                    _WhereSet("AND " + campo + " NOT LIKE ", "%" + CheckValor(valor) + "%");
                    break;
            }
        }
        public void FechaBetween(String campo, string inicio, string termino)
        {
            _WhereSet(string.Format(" AND {0} BETWEEN '{1} 00:00:00' xx '{2} 23:59:59'", campo, inicio, termino), "BETWEEN");
        }
        private void _WhereSet(String campo, String valor)
        {
            if (_whereRaw == "" || _group)
            {
                string ng = "";
                if (_group)
                {
                    ng = _whereRaw;
                }
                if (valor == "IS NULL")
                {
                    _whereRaw = campo.Replace("OR ", "").Replace("NOT ", "").Replace("AND", "") + " IS NULL";
                }
                else if (valor == "IS NOT NULL")
                {
                    _whereRaw = campo.Replace("OR ", "").Replace("AND", "") + " IS NOT NULL";
                }
                else if (valor == "BETWEEN")
                {
                    _whereRaw = campo.Replace("OR ", "").Replace("NOT ", "").Replace("AND", "").Replace("xx", "AND");
                }
                else
                {
                    _whereRaw = campo.Replace("OR ", "").Replace("NOT ", "").Replace("AND", "") + "'" + valor + "'";                    
                }
                if (_group)
                {
                    _whereRaw = ng + _whereRaw;
                    _group = false;
                }
            }
            else
            {
                if (valor == "IS NULL")
                {
                    _whereRaw += " " + campo + " IS NULL";
                }
                else if (valor == "IS NOT NULL")
                {
                    _whereRaw += " " + campo + " IS NOT NULL";
                }
                else if (valor == "BETWEEN")
                {
                    _whereRaw += campo.Replace("xx", "AND");
                }
                else
                {
                    _whereRaw += " " + campo + "'" + valor + "'";                                       
                }                
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
        public void Limit(Int64 count, Int64 init = 0)
        {
            if (TipoDB == TipoConexion.MYSQL)
            {
                _limit = string.Format("LIMIT {0}, {1}", init, count);
            }
            else
            {
                _limit = string.Format("TOP {0} ", count);
            }
        }
        public void GroupBy(string value)
        {
            _groupby = value;
        }
        public String Generar()
        {
            String sql = "";
            switch (Tipo)
            {
                case TipoQuery.SELECT:
                    sql = "SELECT ";
                    if (_limit != "" && TipoDB == TipoConexion.MSSQL)
                    {
                        sql += " " + _limit;
                    }
                    if (_distinct)
                    {
                        sql += " DISTINCT ";
                    }
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
                    if (_join.Count() > 0)
                    {
                        foreach (string item in _join)
                        {
                            sql += " " + item;
                        }
                        
                    }
                    if (_whereRaw != "")
                    {
                        sql += " WHERE " + _whereRaw;
                    }
                    if (_groupby != "")
                    {
                        sql += " GROUP BY " + _groupby;
                    }
                    if (_orderby != "")
                    {
                        sql += " ORDER BY " + _orderby;
                    }
                    if (_limit != "" && TipoDB == TipoConexion.MYSQL)
                    {
                        sql += " " + _limit;
                    }
                    break;
                case TipoQuery.UPDATE:
                    ///UPDATE Customers SET ContactName = 'Alfred Schmidt', City = 'Frankfurt' WHERE CustomerID = 1;
                    sql = "UPDATE " + _tabla;
                    String c = "";
                    foreach (var item in _campos)
                    {
                        if (c != "")
                        {
                            c += ", ";
                        }
                        c += item.Key + " = '" + CheckValor(item.Value) + "'";
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
            sql = sql.Replace("'now()'", "NOW()");
            sql = sql.Replace("'NOW()'", "NOW()");
            //Console.WriteLine(sql);
            return sql;
        }
    }
}
