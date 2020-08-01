using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yui.Herramientas_Personalizadas
{
    public class YUICombobox  : ComboBox
    {
        public bool ReadOnly { get; set; }
        public CharacterCasing CharacterCasing { get; set; }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //return base.ProcessCmdKey(ref msg, keyData);
            return ReadOnly;
        }
        protected override void WndProc(ref Message m)
        {
            if (ReadOnly && (m.Msg == 0x0201 || m.Msg == 0x0203))
            {
                return;
            }
            base.WndProc(ref m);

        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            switch (CharacterCasing)
            {
                case CharacterCasing.Normal:
                    e.KeyChar = e.KeyChar;
                    break;
                case CharacterCasing.Upper:
                    e.KeyChar = char.ToUpper(e.KeyChar);
                    break;
                case CharacterCasing.Lower:
                    e.KeyChar = char.ToLower(e.KeyChar);
                    break;
                default:
                    break;
            }
            base.OnKeyPress(e);
        }
    }
}
