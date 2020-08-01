using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yui.Helpers
{
    public static class Pantalla
    {
        [DllImport("user32.DLL", EntryPoint ="ReleaseCapture")]
        public static extern int ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint="SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        public static void MoverPantall(Form f)
        {
            ReleaseCapture();
            SendMessage(f.Handle, 0x112, 0xF012, 0);
        }

        public static void PanelForm(Panel p, Form f)
        {
            try
            {
                if (p.Controls.Count > 0)
                {
                    p.Controls.RemoveAt(0);
                }
                f.TopLevel = false;
                f.Parent = p;
                f.FormBorderStyle = FormBorderStyle.None;
                f.Dock = DockStyle.Fill;
                p.Controls.Add(f);
                p.Tag = f;
                f.Show();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void ClearPanel(Panel p)
        {
            try
            {
                p.Controls.RemoveAt(0);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static void ClearPanel(Panel p, Control c)
        {
            try
            {
                p.Controls.Remove(c);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
