using System;
using System.Reflection;

namespace sqlserver.adapter.test
{
    public static class ReflectionHelper
    {
        public static dynamic CreateInstance(this Assembly dll, string type)
        {
            var typee = dll.GetType(type);
            dynamic obj = Activator.CreateInstance(typee);
            if(null == obj)throw new InvalidOperationException("Cannot create object " + type);
            return obj;
        }
    }
}