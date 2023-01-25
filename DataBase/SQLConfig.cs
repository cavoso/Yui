using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yui.DataBase
{
    public class SQLConfig
    {
        public String Host { get; set; }
        public String User { get; set; }
        public String Pass { get; set; }
        public String DB { get; set; }
        public TipoConexion Tipo { get; set; }
        public String Nombre { get; set; }
        public Boolean Predeterminada { get; set; }


        public SQLConfig()
        {
            
        }

        public SQLConfig(String h, String u, String p, String db, TipoConexion t, String n, Boolean pred = false)
        {
            Host = h;
            User = u;
            Pass = p;
            DB = db;
            Tipo = t;
            Nombre = n;
            Predeterminada = pred;
        }
        public Boolean StatusHost
        {
            get
            {
                try
                {
                    System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
                    if (p.Send(Host, 500).Status == System.Net.NetworkInformation.IPStatus.Success)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;                    
                }
            }
        }
    }
}
