using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Yui.DataBase
{
    public class Builder
    {
        private Dictionary<String, Object> _Where;
        private String _WhereRaw;
        public Builder()
        {
            NewQuery();
        }
        public void NewQuery()
        {
            _Where = new Dictionary<String, Object>();
            _WhereRaw = "";
        }

        public void Where(String campo, Object valor)
        {
            _Where.Add(campo, valor);
        }
        public void Where(Dictionary<String, Object> w)
        {
            foreach (var item in w)
            {
                _Where.Add(item.Key, item.Value);
            }
        }
        public void WhereRaw(String w)
        {
            _WhereRaw = w;
        }
    }
}
