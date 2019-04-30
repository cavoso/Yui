using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Yui.Herramientas_Personalizadas
{
    public partial class YUITextbox : TextBox
    {
        private Boolean flag = false;
        [Category("Placeholder"), Description("Agrega texto auto descriptivo, solo se muestra si el textbox está vacío")]
        public String Placeholder { get; set; } = "<Descripcion>";
        [Category("Placeholder"), Description("Establece el color del texto auto descriptivo")]
        public Color PlaceholderColor { get; set; } = Color.Gray;
        [Category("Configuracion"), Description("Establece el tipo de texbox que se utilizara")]
        public TipoCampo Tipo { get; set; } = TipoCampo.General;
        [Category("Configuracion"), Description("Establece si se utilizara algún signo de moneda, solo para campos de tipo numero")]
        public Signo SignoMoneda { get; set; } = Signo.Ninguno;
        [Category("Configuracion"), Description("Establece la cantidad de decimales que tendrá el campo, solo valido para campos de tipo numero")]
        public int Decimales { get; set; } = 0;
        [Category("Configuracion"), Description("Establece si el campo llevara separador de miles, solo valido para campo de tipo numero")]
        public RespuestaSimple SeparadorMiles { get; set; } = RespuestaSimple.NO;
        [Category("Configuracion"), Description("Establece si se desea validar Rut, para utilizar el campo como validador de Rut (el campo debe estar en general)")]
        public RespuestaSimple ValidarRut { get; set; } = RespuestaSimple.NO;
        [Category("Configuracion"), Description("Establece si se permitirán tildes en el textbox, solo valido para texbox de tipo letras")]
        public RespuestaSimple Tildes { get; set; } = RespuestaSimple.SI;
        [Category("Configuracion"), Description("Devuelve el valor del campo")]
        public String SimpleText
        {
            get
            {
                if (Tipo == TipoCampo.Numero && SeparadorMiles == RespuestaSimple.SI)
                {
                    String text = base.Text;
                    if (SignoMoneda == Signo.Peso || SignoMoneda == Signo.Dolar)
                    {
                        text = text.Replace("$ ", "");
                    }
                    if (SignoMoneda == Signo.Euro)
                    {
                        text = text.Replace("€ ", "");
                    }
                    return text.Replace(".", "");
                }
                else
                {
                    return base.Text;
                }
            }
        }
        [Category("Configuracion"), Description("Establece el valor del campo con su formato ya definido")]
        public String UpdateText
        {
            set
            {
                base.Text = value;
                TextFormat();
            }
        }
        public YUITextbox()
        {
            InitializeComponent();
        }
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            VerificaTextoVacio();
        }
        protected override void OnTextChanged(EventArgs e)
        {
            switch (Tipo)
            {
                case TipoCampo.Numero:
                    if (!Information.IsNumeric(SimpleText) && base.Text !="")
                    {
                        base.Text = "0";
                        MessageBox.Show(
                            "Este campo solo admite números",
                            "Error",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Error
                        );
                    }
                    break;
                case TipoCampo.Letras:
                    String temp = "";
                    for (int i = 1; i < base.Text.Length; i++)
                    {
                        if (
                            Char.IsLetter(Convert.ToChar(Strings.Mid(base.Text, i, 1))) || 
                            Char.IsControl(Convert.ToChar(Strings.Mid(base.Text, i, 1))) ||
                            Char.IsSeparator(Convert.ToChar(Strings.Mid(base.Text, i, 1)))
                            )
                        {
                            temp = temp + Strings.Mid(base.Text, i, 1);
                        }
                        if (Tildes == RespuestaSimple.NO)
                        {
                            base.Text = SignosDiacriticos(temp);
                            base.SelectionStart = i;
                        }
                        else
                        {
                            base.Text = temp;
                        }
                    }
                    break;
                case TipoCampo.General:
                    if (Tildes == RespuestaSimple.NO)
                    {
                        if (base.Text != "")
                        {
                            base.Text = SignosDiacriticos(base.Text);
                            base.SelectionStart = base.Text.Length;
                        }
                    }
                    if (ValidarRut == RespuestaSimple.SI)
                    {
                        String s = base.Text.Trim();
                        if (s.Contains('-'))
                        {
                            String[] v = s.Split('-');
                            if (v[1] != "")
                            {
                                if (Funciones.Validadores.ValidarRut(v[0], v[1]) == false)
                                {
                                    MessageBox.Show(
                                        "El Rut indicado no es válido como Rut nacional.",
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error
                                    );
                                }
                            }
                        }
                    }
                    break;
            }
            base.OnTextChanged(e);
            VerificaTextoVacio();
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            if (flag)
            {
                pe.Graphics.DrawString(Placeholder, new Font(base.Font, FontStyle.Italic), new SolidBrush(PlaceholderColor), new Point(0, 0));
            }
            else
            {
                pe.Graphics.DrawString(base.Text, base.Font, new SolidBrush(base.ForeColor), new Point(0, 0));
            }
            base.OnPaint(pe);
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            switch (Tipo)
            {
                case TipoCampo.General:
                    if (char.IsControl(e.KeyChar))
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        if (Tildes == RespuestaSimple.NO)
                        {
                            if (
                                e.KeyChar == Convert.ToChar(39) ||
                                e.KeyChar == Convert.ToChar(34) ||
                                e.KeyChar == Convert.ToChar(180)
                                )
                            {
                                e.Handled = true;
                            }
                            else
                            {
                                e.Handled = false;
                            }
                        }
                        else
                        {
                            if (e.KeyChar == Convert.ToChar(180))
                            {
                                e.Handled = true;
                            }
                            else
                            {
                                e.Handled = false;
                            }
                        }
                    }
                    break;
                case TipoCampo.Numero:
                    bool resp = false;
                    if (Decimales > 0)
                    {
                        if (
                            char.IsDigit(e.KeyChar) ||
                            char.IsNumber(e.KeyChar) ||
                            char.IsControl(e.KeyChar) ||
                            e.KeyChar == ','
                            )
                        {
                            if (
                                base.Text.Contains(',') &&
                                !char.IsControl(e.KeyChar)
                                )
                            {
                                String[] t = base.Text.Split(',');
                                if (
                                    t[1].ToString().Length < Decimales
                                    )
                                {
                                    resp = false;
                                }
                                else
                                {
                                    resp = false;
                                }
                            }
                            else
                            {
                                resp = false;
                            }
                        }
                        else
                        {
                            resp = true;
                        }
                    }
                    else
                    {
                        if (
                            char.IsDigit(e.KeyChar) ||
                            char.IsNumber(e.KeyChar) ||
                            char.IsControl(e.KeyChar)
                            )
                        {
                            resp = false;
                        }
                        else
                        {
                            resp = true;
                        }
                    }
                    e.Handled = resp;
                    break;
                case TipoCampo.Letras:
                    if (Tildes == RespuestaSimple.NO)
                    {
                        if (
                            char.IsControl(e.KeyChar) ||
                            char.IsLetter(e.KeyChar) ||
                            char.IsSeparator(e.KeyChar) ||
                            char.IsWhiteSpace(e.KeyChar)
                            )
                        {
                            e.Handled = false;
                        }
                        else
                        {
                            e.Handled = true;
                        }
                    }
                    else
                    {
                        if (
                            char.IsLetter(e.KeyChar) ||
                            char.IsControl(e.KeyChar) ||
                            char.IsWhiteSpace(e.KeyChar)
                            )
                        {
                            e.Handled = false;
                        }
                        else
                        {
                            e.Handled = true;
                        }
                    }
                    break;
                default:
                    break;
            }
            base.OnKeyPress(e);
        }
        protected override void OnEnter(EventArgs e)
        {
            if (Tipo == TipoCampo.Numero && SeparadorMiles == RespuestaSimple.SI)
            {
                base.Text = SimpleText;
            }
            base.OnEnter(e);
        }
        protected override void OnLeave(EventArgs e)
        {
            TextFormat();
            base.OnLeave(e);
        }

        /*
         * Establecemos metodos privados para uso interno
         */
        protected void VerificaTextoVacio()
        {
            if (base.Text.Length > 0)
            {
                flag = false;
            }
            else
            {
                flag = true;
            }
            base.SetStyle(ControlStyles.UserPaint, flag);
            base.Refresh();
        }
        protected String SignosDiacriticos(String thestring)
        {
            String A;
            String B;
            const String AccChars = "ŠŽšžŸÀÁÂÃÄÅÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖÙÚÛÜÝàáâãäåçèéêëìíîïðñòóôõöùúûüýÿ";
            const String RegChars = "SZszYAAAAAACEEEEIIIIDNOOOOOUUUUYaaaaaaceeeeiiiidnooooouuuuyy";
            for (int i = 0; i < AccChars.Length; i++)
            {
                A = Strings.Mid(AccChars, i, 1);
                B = Strings.Mid(RegChars, i, 1);
                thestring = thestring.Replace(A, B);
            }
            return thestring;
        }
        protected void TextFormat()
        {
            if (Tipo == TipoCampo.Numero && SeparadorMiles == RespuestaSimple.SI)
            {
                try
                {
                    Double valor = Convert.ToDouble(base.Text);
                    if (SignoMoneda == Signo.Peso || SignoMoneda == Signo.Dolar)
                    {
                        base.Text = "$ " + Strings.FormatNumber(valor, Decimales);
                    }
                    else if (SignoMoneda == Signo.Euro)
                    {
                        base.Text = "€ " + Strings.FormatNumber(valor, Decimales);
                    }
                    else
                    {
                        base.Text = Strings.FormatNumber(valor, Decimales);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Imposible aplicar formato al textbox\n" +
                        "Error: " + ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                   );
                }
            }
        }
        public bool IsVelidNumerico()
        {
            if (Tipo == TipoCampo.Numero)
            {
                if (base.Text != "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool IsValidRut()
        {
            bool var = false;
            if (ValidarRut == RespuestaSimple.SI)
            {
                String s = base.Text.Trim();
                if (s.Contains("-"))
                {
                    String[] v = s.Split('-');
                    if (v[1] != "")
                    {
                        var = Funciones.Validadores.ValidarRut(v[0], v[1]);
                    }
                }
            }
            else
            {
                var = true;
            }
            return var;
        }
    }  
}
