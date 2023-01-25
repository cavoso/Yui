using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Yui.Funciones
{
    public static class Comunes
    {
        
        public static int NumeroAleatorio(int inicio = 10, int termino = 100)
        {
            Random num = new Random();
            return num.Next(inicio, termino);
        }
        public static void CopiarDataGrid(DataGridView g)
        {
            Clipboard.Clear();
            g.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            g.SelectAll();
            Clipboard.SetDataObject(g.GetClipboardContent());
            MessageBox.Show("DataGrid Copiado");
        }
        public static int ContarPalabras(String frase)
        {
            int palabras = 0;

            for (int i = 0; i < frase.Length; i++)
            {
                if (frase[i] == ' ' || frase[i] == '.' || frase[i] == ',')
                {
                    palabras++;
                }
            }
            return palabras;
        }        
        public static String SeparadorMiles(int valor, int decimales = 0, String signo = "")
        {
            try
            {
                return signo + Strings.FormatNumber(valor, decimales);
            }
            catch (Exception)
            {
                return valor.ToString();
            }
        }
        public static String ToRoman(int num)
        {
            if (num < 0 && num > 999)
            {
                return "Error";
            }
            Dictionary<int, String> r_ones = new Dictionary<int, string>()
            {
                {1, "I"},
                { 2, "II"},
                { 3, "III"},
                { 4, "IV"},
                { 5, "V"},
                { 6, "VI"},
                { 7, "VII"},
                { 8, "VIII"},
                { 9, "IX"}
            };
            Dictionary<int, String> r_tens = new Dictionary<int, string>()
            {
                {1, "X"},
                {2, "XX"},
                {3, "XXX"},
                {4, "XL"},
                {5, "L"},
                {6, "LX"},
                {7, "LXX"},
                {8, "LXXX"},
                {9, "XC"}
            };
            Dictionary<int, String> r_hund = new Dictionary<int, string>()
            {
                {1, "C"},
                {2, "CC"},
                {3, "CCC"},
                {4, "CD"},
                {5, "D"},
                {6, "DC"},
                {7, "DCC"},
                {8, "DCCC"},
                {9, "CM"}
            };
            Dictionary<int, String> r_thou = new Dictionary<int, string>()
            {
                {1, "M"},
                {2, "MM"},
                {3, "MMM"},
                {4, "MMMM"},
                {5, "MMMMM"},
                {6, "MMMMMM"},
                {7, "MMMMMMM"},
                {8, "MMMMMMMM"},
                {9, "MMMMMMMMM"}
            };

            int ones = (num % 10);
            int tens = ((num - ones) % 100);
            int hundreds = ((num - tens - ones) % 1000);
            int thou = ((num - hundreds - tens - ones) % 10000);

            tens = (tens / 10);
            hundreds = (hundreds / 100);
            thou = (thou / 1000);

            String rnum = "";

            if (thou > 0)
            {
                rnum = rnum + r_thou[thou];
            }
            if (hundreds > 0)
            {
                rnum = rnum + r_hund[hundreds];
            }
            if (tens > 0)
            {
                rnum = rnum + r_tens[tens];
            }
            if (ones > 0)
            {
                rnum = rnum + r_ones[ones];
            }

            return rnum;
        }
        public static String NumberK(int num)
        {
            String n = num.ToString();
            double d = 0;
            if (num >= 999 & num <= 999999)
            {
                d = Math.Round(Convert.ToDouble((num / 1000)), 1);
                n = d + "K";
            }
            if (num >= 999999)
            {
                d = Math.Round(Convert.ToDouble((num / 1000000)), 1);
                n = d + "M";
            }

            return n;
        }
        public static Boolean Enter(KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                return true;
            } else
            {
                return false;
            }
        }
        public static void ChangeSizeImageButton(Button btn, int s = 32)
        {
            btn.Image = new Bitmap(btn.Image, s, s);
        }
        public static void Message(string contenido)
        {
            MessageBox.Show(contenido);
        }
    }
}
