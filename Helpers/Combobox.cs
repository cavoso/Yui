using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yui.Helpers
{
    public static class Combobox
    {
       
        public static void Llenar(System.Windows.Forms.ComboBox c, Dictionary<string, string> d)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Value", typeof(string));

            foreach (KeyValuePair<string, string> p in d)
            {
                dt.Rows.Add(p.Value, p.Key);
            }
            c.DisplayMember = "Text";
            c.ValueMember = "Value";
            c.DataSource = dt;
            c.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        }
        public static void Llenar(System.Windows.Forms.ComboBox c, Dictionary<int, string> d)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Value", typeof(int));

            foreach (KeyValuePair<int, string> p in d)
            {
                dt.Rows.Add(p.Value, p.Key);
            }
            c.DisplayMember = "Text";
            c.ValueMember = "Value";
            c.DataSource = dt;
            c.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        }
        public static void Llenar(System.Windows.Forms.ComboBox c, Dictionary<int, int> d)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Text", typeof(int));
            dt.Columns.Add("Value", typeof(int));

            foreach (KeyValuePair<int, int> p in d)
            {
                dt.Rows.Add(p.Value, p.Key);
            }
            c.DisplayMember = "Text";
            c.ValueMember = "Value";
            c.DataSource = dt;
            c.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        }

        public static void Mes(System.Windows.Forms.ComboBox c)
        {
            Dictionary<int, string> d = new Dictionary<int, string>()
            {
                {1, "Enero"},
                {2, "Febrero"},
                {3, "Marzo"},
                {4, "Abril"},
                {5, "Mayo"},
                {6, "Junio"},
                {7, "Julio"},
                {8, "Agosto"},
                {9, "Septiembre"},
                {10, "Octubre"},
                {11, "Noviembre"},
                {12, "Diciembre"}
            };
            Llenar(c, d);
        }
    }
}
