using System;
using Bespoke.SphCommercialSpaces.Domain;

namespace roslyn.scriptengine
{
    public class HostObject
    {
        public Entity Item { get; set; }

        public string @UserName
        {
            get
            {
                var ad = ObjectBuilder.GetObject<IDirectoryService>();
                return ad.CurrentUserName;
            }
        }
        public DateTime @Today { get { return DateTime.Today; } }
        public DateTime @Now { get { return DateTime.Now; } }

     
    }
}