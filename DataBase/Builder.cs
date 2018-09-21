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
        private String[] _comparadores;
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
                "<="
            };
        }
        public void NewQuery()
        {
            _selectRaw = "";
            _tabla = "";
            _whereRaw = "";
            Tipo = TipoQuery.SELECT;
        }
        public void NewQuery(TipoQuery tipo)
        {
            _selectRaw = "";
            _tabla = "";
            _whereRaw = "";
            Tipo = tipo;
        }

        public void Tabla(String tabla)
        {
            _tabla = tabla;
        }

        public void Where(String campo, Object valor, String comparador = "=")
        {
            if (_comparadores.Contains<String>(comparador))
            {
                _WhereSet(campo + comparador, CheckValor(valor));
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
                //_Where.Add(item.Key, item.Value);
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
        public void Like(String campo, Object valor, TipoLike like = TipoLike.Contiene)
        {
            switch (like)
            {
                case TipoLike.Contiene:
                    _WhereSet(campo, "%" + CheckValor(valor) + "%");
                    break;
                case TipoLike.Comienza:
                    _WhereSet(campo, CheckValor(valor) + "%");
                    break;
                case TipoLike.Termina:
                    _WhereSet(campo, "%" + CheckValor(valor));
                    break;
                default:
                    _WhereSet(campo, "%" + CheckValor(valor) + "%");
                    break;
            }
        }
        public void OrLike(String campo, Object valor, TipoLike like = TipoLike.Contiene)
        {
            switch (like)
            {
                case TipoLike.Contiene:
                    _WhereSet("OR " + campo, "%" + CheckValor(valor) + "%");
                    break;
                case TipoLike.Comienza:
                    _WhereSet("OR " + campo, CheckValor(valor) + "%");
                    break;
                case TipoLike.Termina:
                    _WhereSet("OR " + campo, "%" + CheckValor(valor));
                    break;
                default:
                    _WhereSet("OR " + campo, "%" + CheckValor(valor) + "%");
                    break;
            }
        }
        public void NotLike(String campo, Object valor, TipoLike like = TipoLike.Contiene)
        {
            switch (like)
            {
                case TipoLike.Contiene:
                    _WhereSet("NOT " + campo, "%" + CheckValor(valor) + "%");
                    break;
                case TipoLike.Comienza:
                    _WhereSet("NOT " + campo, CheckValor(valor) + "%");
                    break;
                case TipoLike.Termina:
                    _WhereSet("NOT " + campo, "%" + CheckValor(valor));
                    break;
                default:
                    _WhereSet("NOT " + campo, "%" + CheckValor(valor) + "%");
                    break;
            }
        }
        private void _WhereSet(String campo, String valor)
        {
            if (_whereRaw == "")
            {
                _whereRaw = campo.Replace("OR ", "").Replace("NOT ", "") + "'" + valor + "'";
            }
            else
            {
                _whereRaw += campo + "'" + valor + "'";
            }
        }
        private String CheckValor(Object valor)
        {
            String temp = "";
            if (valor.GetType() == typeof(decimal))
            {

            }
            else if (valor.GetType() == typeof(int))
            {
                temp = valor.ToString();
            }
            else
            {
                temp = valor.ToString();
            }
            
            return temp;
        }
        public void WhereRaw(String w)
        {
            _whereRaw = w;
        }

        public String Generar()
        {
            String sql = "";
            return sql;
        }
    }
}
