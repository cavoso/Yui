using System;

namespace Yui.DataBase
{  
    public class SQLAttribute : Attribute
    {
        public SQLAttribute()
        {

        }
        public SQLAttribute(string columName)
        {
            ColumnSQLName = columName;
        }
        public SQLAttribute(bool ignorar)
        {
            Ignore = ignorar;
        }
        public string ColumnSQLName { get; set; }
        public bool Ignore { get; set; }
    }
}
