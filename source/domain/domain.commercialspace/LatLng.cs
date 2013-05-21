using System;
using System.Linq;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public class LatLng : DomainObject
    {
        private double m_lat;
        private double m_lng;
        private double? m_elevation;
        public string ToWKT()
        {
            return string.Format("POINT({0} {1}{2})", this.Lng, this.Lat, this.Elevation.HasValue ? " " + this.Elevation : string.Empty);
        }

        public override string ToString()
        {
            return string.Format("{0} {1}{2}", this.Lng, this.Lat, this.Elevation.HasValue ? " " + this.Elevation : string.Empty);
        }
        public LatLng()
        {

        }

        public LatLng(string point, bool isWkt = false)
        {
            if (isWkt)
            {
                var points2 = point.Replace("POINT (", "").Replace(")", "").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                this.Lat = double.Parse(points2.Last());
                this.Lng = double.Parse(points2.First());

                return;
            }
            var points = point.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            this.Lat = double.Parse(points.First());
            this.Lng = double.Parse(points.Last());

        }

        public double Lng
        {
            get { return m_lng; }
            set
            {
                m_lng = value;
                RaisePropertyChanged();
            }
        }


        public double? Elevation
        {
            get { return m_elevation; }
            set
            {
                m_elevation = value;
                RaisePropertyChanged();
            }
        }
        public double Lat
        {
            get { return m_lat; }
            set
            {
                m_lat = value;
                RaisePropertyChanged();
            }
        }
    }
}
