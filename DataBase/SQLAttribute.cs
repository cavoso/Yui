using System;

namespace Yui.DataBase
{  
    public class SQLAttribute : Attribute
    {
        public string ColumnSQLName { get; set; }
    }
}
