﻿namespace Bespoke.Sph.Domain
{
    public partial class Building : SpatialEntity
    {
        public override string ToString()
        {
            return string.Format("{0} {1}\r\n{2}", this.Name, this.UnitNo, this.Note);
        }

        public string SearchKeywords
        {
            get { return string.Format("{0} {1} {2}", this.Name, this.UnitNo, this.Note); }
        }

    }
}
