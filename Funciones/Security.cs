using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Yui.Funciones
{
    public static class Security
    {
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
        public static bool IsMD5(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return false;
            }

            return Regex.IsMatch(input, "^[0-9a-fA-F]{32}$", RegexOptions.Compiled);
        }
        /// <summary>
        /// Genera un token con una marca de tiempo
        /// </summary>
        /// <returns>
        /// retorna el Token
        /// </returns>
        public static string Token()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());

            return token;
        }
        /// <summary>
        /// Valida que el token siga siendo valido (solo valido para tokens generados con esta DLL)
        /// </summary>
        /// <param name="token">
        /// token generado
        /// </param>
        /// <param name="horas">
        /// tiempo en horas para validar la duracion del token por defecto 24 hrs
        /// </param>
        /// <returns>
        /// retorna true o false dependiendo si el token lleva mas tiempo que el de validacion
        /// </returns>
        public static bool ValidacionToken(string token, int horas = -24)
        {
            if (horas > 0)
            {
                horas = horas * -1;
            }
            byte[] data = Convert.FromBase64String(token);
            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            if (when < DateTime.UtcNow.AddHours(horas))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static string NuevaPassword(int largo)
        {
            Random rdn = new Random();
            string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890%$#@";
            int longitud = caracteres.Length;
            char letra;
            string contraseniaAleatoria = string.Empty;
            for (int i = 0; i < largo; i++)
            {
                letra = caracteres[rdn.Next(longitud)];
                contraseniaAleatoria += letra.ToString();
            }
            return contraseniaAleatoria;
        }
    }
}
