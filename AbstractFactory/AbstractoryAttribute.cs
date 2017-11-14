using System;
using System.Collections.Generic;
using System.Text;

namespace Abstractory
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AbstractoryAttribute : Attribute
    {
        string _name;

        public AbstractoryAttribute(string name)
        {
            _name = name;
        }

        public string GetName()
        {
            return _name;
        }
    }
}
