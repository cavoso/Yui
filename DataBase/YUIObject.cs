using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yui.DataBase
{
    public class YUIObject : Object
    {
        /// <summary>
        /// Devuelve la variable en formato String, por defecto vacio
        /// </summary>
        public String String { get; set; } = "";
        /// <summary>
        /// Devuelve la variable en formato integer, si la variable es de tipo double, el valor integer queda redondeado, por defecto 0
        /// </summary>
        public Int32 Integer { get; set; } = 0;
        /// <summary>
        /// Devuelve la variable en formato Long (Int64), por defecto 0
        /// </summary>
        public Int64 Long { get; set; } = 0;
        /// <summary>
        /// Devuelve la variable en formato Double, por defecto 0
        /// </summary>
        public Double Double { get; set; } = 0;
        /// <summary>
        /// Devuelve la variable en formato Boolean, este valor se devuelve en base al valor de integer en caso de ser 0 o 1, por defecto es false
        /// </summary>
        public Boolean Boolean {
            get
            {
                if (Integer == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private Object Obj;
        public YUIObject()
        {

        }
        public YUIObject(Object o)
        {
            ChekValue(o);
        }
        private void ChekValue(Object o)
        {
            Obj = o;
            String = o.ToString();
            // Convertimos a integer
            try
            {
                Integer = Convert.ToInt32(o);
            }
            catch (Exception)
            {
                Integer = 0;
            }
            //convertimos a long
            try
            {
                Long = Convert.ToInt64(o);
            }
            catch (Exception)
            {
                Long = 0;
            }
            //convertimos a double
            try
            {
                Double = Convert.ToDouble(o);
            }
            catch (Exception)
            {
                Double = 0;
            }
        }
        public new Type GetType()
        {
            return Obj.GetType();
        }


    }
}
