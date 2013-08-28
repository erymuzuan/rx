using System;
using System.Collections.Generic;
using System.Linq;
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

        public Func<IEnumerable<int>, int> SUM
        {
            get
            {
                Func<IEnumerable<int>, int> sum = (list) => list.Sum();
                return sum;
            }
        }
    }
}