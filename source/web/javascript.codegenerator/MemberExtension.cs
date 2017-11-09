using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WebJavascriptUtils
{
    internal  static class MemberExtension
    {
        public static string GenerateJavascriptInitValue(this Member mbr, string ns)
        {
            switch (mbr)
            {
                case ComplexMember cm:
                    return cm.GenerateJavascriptInitValue(ns);
                case SimpleMember sm:
                    return sm.GenerateJavascriptInitValue(ns);
                case ValueObjectMember vom:
                    return vom.GenerateJavascriptInitValue(ns);
            }
            return null;
        }

        public static string GenerateJavascriptMember(this Member mbr, string ns)
        {
            switch (mbr)
            {
                case ComplexMember cm:
                    return cm.GenerateJavascriptMember(ns);
                case SimpleMember sm:
                    return sm.GenerateJavascriptMember(ns);
                case ValueObjectMember vom:
                    return vom.GenerateJavascriptMember(ns);
            }
            return null;
        }
        
        public static string GenerateJavascriptContructor(this Member mbr, string ns)
        {
            switch (mbr)
            {
                case ComplexMember cm:
                    return cm.GenerateJavascriptContructor(ns);
                case SimpleMember sm:
                    return sm.GenerateJavascriptContructor(ns);
                case ValueObjectMember vom:
                    return vom.GenerateJavascriptContructor(ns);
            }
            return null;
        }


        public static string GenerateJavascriptClass(this Member mbr,string jns, string csNs, string assemblyName)
        {
            switch (mbr)
            {
                case ComplexMember cm:
                    return cm.GenerateJavascriptClass(jns,csNs,assemblyName);
                case SimpleMember sm:
                    return sm.GenerateJavascriptClass(jns, csNs, assemblyName);
                case ValueObjectMember vom:
                    return vom.GenerateJavascriptClass(jns, csNs, assemblyName);
            }
            return null;
        }

    }
}