using System;

namespace Yui.DataBase.Serialization.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class YUIElementAttribute : Attribute
    {
        public YUIElementAttribute()
        {

        }
        public YUIElementAttribute(string elementName)
        {
            ElementName = elementName;
        }

        public string ElementName { get; }
    }
}
