using System.Collections.Generic;

namespace Bespoke.Sph.Domain
{
    partial class FeatureDefinition : DomainObject
    {

        public List<int> AvailableQuantityOptions
        {
            get
            {
                var q = new List<int>();
                for (int i = 0; i <= this.AvailableQuantity; i++)
                {
                    q.Add(i);
                };

                return q;
            }

        }
    }
}
