using System;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.RoslynScriptEngines
{
    public class HostObject<T>
    {
        public T Item { get; set; }

        public string @UserName
        {
            get
            {
                var ad = ObjectBuilder.GetObject<IDirectoryService>();
                return ad.CurrentUserName;
            }
        }
        public DateTime @Today => DateTime.Today;
        public DateTime @Now => DateTime.Now;


        public string Format(string format, object obj)
        {
            var fs = "{0:" + format + "}";
            return string.Format(fs, obj);
        }

    }
}