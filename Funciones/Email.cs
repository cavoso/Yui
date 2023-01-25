using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Windows.Forms;
using System.Net;

namespace Yui.Funciones
{
    public class Email
    {
        private MailMessage Correo;

        public bool Status { get; set; } = false;
        public string SMTP { get; set; }
        public MailAddress Usuario { get; set; }
        public string Password { get; set; }
        public List<MailAddress> Para { get; set; } = new List<MailAddress>();
        public List<MailAddress> CC { get; set; } = new List<MailAddress>();
        public List<MailAddress> BCC { get; set; } = new List<MailAddress>();
        public string Cuerpo { get; set; }
        public string Asunto { get; set; }
        public int Puerto { get; set; }
        public bool IsHtml { get; set; }
        public bool SSL { get; set; }
        public bool IsInit { get; set; }

        public Email()
        {
            IsHtml = true;
            Puerto = 25;
            SSL = false;
            IsInit = false;
            BaseInit();
        }
        public Email(string _smtp)
        {
            SMTP = _smtp;
            IsHtml = true;
            Puerto = 25;
            SSL = false;
            IsInit = false;
            BaseInit();
        }
        public Email(string _smtp, MailAddress _usuario)
        {
            SMTP = _smtp;
            Usuario = _usuario;
            IsHtml = true;
            Puerto = 25;
            SSL = false;
            IsInit = false;
            BaseInit();
        }
        public Email(string _smtp, MailAddress _usuario, string _pass)
        {
            SMTP = _smtp;
            Usuario = _usuario;
            Password = _pass;
            IsHtml = true;
            Puerto = 25;
            SSL = false;
            IsInit = false;
            BaseInit();
        }
        public Email(string _smtp, MailAddress _usuario, string _pass, MailAddress _para)
        {
            SMTP = _smtp;
            Usuario = _usuario;
            Password = _pass;
            Para.Add(_para);
            IsHtml = true;
            Puerto = 25;
            SSL = false;
            IsInit = false;
            BaseInit();
        }
        public Email(string _smtp, MailAddress _usuario, string _pass, MailAddress _para, string _cuerpo)
        {
            SMTP = _smtp;
            Usuario = _usuario;
            Password = _pass;
            Para.Add(_para);
            Cuerpo = _cuerpo;
            IsHtml = true;
            Puerto = 25;
            SSL = false;
            IsInit = false;
            BaseInit();
        }
        public Email(string _smtp, MailAddress _usuario, string _pass, MailAddress _para, string _cuerpo, string _asunto)
        {
            SMTP = _smtp;
            Usuario = _usuario;
            Password = _pass;
            Para.Add(_para);
            Cuerpo = _cuerpo;
            Asunto = _asunto;
            IsHtml = true;
            Puerto = 25;
            SSL = false;
            IsInit = false;
            BaseInit();
        }
        public Email(string _smtp, MailAddress _usuario, string _pass, MailAddress _para, string _cuerpo, string _asunto, int _puerto)
        {
            SMTP = _smtp;
            Usuario = _usuario;
            Password = _pass;
            Para.Add(_para);
            Cuerpo = _cuerpo;
            Asunto = _asunto;
            IsHtml = true;
            Puerto = _puerto;
            SSL = false;
            IsInit = false;
            BaseInit();
        }
        public void BaseInit()
        {
            Para = new List<MailAddress>();
            CC = new List<MailAddress>();
            BCC = new List<MailAddress>();
        }
        public void Inicializar()
        {
            try
            {
                Correo = new MailMessage()
                {
                    From = Usuario,
                    Subject = Asunto,
                    IsBodyHtml = IsHtml,
                    Body = Cuerpo
                };

                foreach (MailAddress item in Para)
                {
                    Correo.To.Add(item);
                }
                foreach (MailAddress item in CC)
                {
                    Correo.CC.Add(item);
                }
                foreach (MailAddress item in BCC)
                {
                    Correo.Bcc.Add(item);
                }
                IsInit = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error al intentar incializar los parametros, error: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void Adjunto(string Adjunto)
        {
            if (IsInit)
            {
                Correo.Attachments.Add(new Attachment(Adjunto));
            }
            else
            {
                MessageBox.Show("Debe inicializar los parametros antes de poder agregar un adjunto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public Boolean Send()
        {
            SmtpClient Servidor = new SmtpClient()
            {
                Host = SMTP,
                Port = Puerto,
                EnableSsl = SSL,
                Credentials = new NetworkCredential(Usuario.Address, Password)
            };
            try
            {
                Servidor.Send(Correo);
                Status = true;
            }
            catch (Exception ex)
            {
                Status = false;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally{
                Servidor.Dispose();
            }

            return Status;
        }

        public static MailAddress SetEmail(string email, string nombre = "")
        {
            return new MailAddress(email, nombre);            
        }
    }
}
