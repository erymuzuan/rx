using System.Diagnostics;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WebJavascriptUtils
{
    public static class SimpleMemberExtension
    {
        internal static string GenerateJavascriptInitValue(this SimpleMember sm, string ns)
        {
            return $"model.{sm.Name}(optionOrWebid.{sm.Name});";
        }


        internal static string GenerateJavascriptMember(this SimpleMember sm, string ns)
        {
            return sm.AllowMultiple ?
                $"     {sm.Name}: ko.observableArray([])," :
                $"     {sm.Name}: ko.observable(),";
        }

        internal static string GenerateJavascriptClass(this SimpleMember sm, string jns, string csNs,
            string assemblyName)
        {
            Debug.WriteLine($"class definition for  {jns}.{sm.Name} in {csNs},{assemblyName} is called");
            return null;
        }
        
        internal static string GenerateJavascriptContructor(this SimpleMember sm, string ns)
        {
            Debug.WriteLine($"ctor for  {ns}.{sm.Name} is called");
            return null;
        }

    }
}
