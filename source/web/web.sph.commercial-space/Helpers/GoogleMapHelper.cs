using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Helpers
{

    /// <summary>
    /// Utility class to manipulate Google Encoded GPolylines
    /// </summary>
    public static class GoogleMapHelper
    {
        public static double GetAzimuth(LatLng origin, LatLng destination)
        {
            var longitudinalDifference = destination.Lng - origin.Lng;
            var latitudinalDifference = destination.Lat - origin.Lat;
            var azimuth = (Math.PI * .5d) - Math.Atan(latitudinalDifference / longitudinalDifference);
            if (longitudinalDifference > 0) return azimuth;
            if (longitudinalDifference < 0) return azimuth + Math.PI;
            if (latitudinalDifference < 0) return Math.PI;
            return 0d;
        }

        //public static double GetDegreesAzimuth(LatLng origin, LatLng destination)
        //{
        //    return RadiansToDegreesConversionFactor * GetAzimuth(origin, destination);
        //}



        public static string ConvertGeomFromTextToGoogleMapEncoded(string wkt)
        {
            const string pattern = @".*?(?<point>\d{1,3}\.\d{5,18} \d{1,3}\.\d{5,18})";
            var macthes = Regex.Matches(wkt, pattern);
            var points = from Match m in macthes
                         select
                     new LatLng(m.Groups["point"].Value, true);

            return EncodeLatLong(points.ToList());
        }

        public static string ToWkt(this IEnumerable<LatLng> line)
        {
            var sb = new StringBuilder("LINESTRING(");
            var list = from l in line select string.Format("{0:F5} {1:F5}", l.Lng, l.Lat);
            sb.Append(string.Join(",", list));
            sb.Append(")");
            return sb.ToString();
        }

        public static double GetAngle2Points(string wkt, LatLng point)
        {
            if (string.IsNullOrWhiteSpace(wkt)) return double.NaN;
            var line = ConvertWktToLines(wkt).OrderByDescending(l => l.Count()).FirstOrDefault();
            if (null != line)
            {
                var points = line.Reverse().Take(2).ToList();
                if (points.Count == 2)
                {
                    point.Lat = points.First().Lat;
                    point.Lng = points.First().Lng;
                    return GetAzimuth(points.Last(), points.First());
                }

            }
            return double.NaN;
        }

        public static double GetAngle2Points(IEnumerable<LatLng> line, LatLng point)
        {
            if (null != line)
            {
                var points = line.Take(2).ToList();
                if (points.Count == 2)
                {
                    point.Lat = points.First().Lat;
                    point.Lng = points.First().Lng;
                    return GetAzimuth(points.Last(), points.First());
                }

            }
            return double.NaN;
        }

        public static double GetAngle2PointsReverse(IEnumerable<LatLng> line, LatLng point)
        {
            if (null != line)
            {
                var points = line.Reverse().Take(2).ToList();
                if (points.Count == 2)
                {
                    point.Lat = points.First().Lat;
                    point.Lng = points.First().Lng;
                    return GetAzimuth(points.Last(), points.First());
                }

            }
            return double.NaN;
        }

        public static IEnumerable<IEnumerable<LatLng>> ConvertWktToLines(string wkt)
        {
            if (string.IsNullOrWhiteSpace(wkt)) return new List<IEnumerable<LatLng>>();

            if (wkt.StartsWith("LINESTRING"))
            {
                var lines = wkt.Replace("LINESTRING (", string.Empty).Replace(")",
                    string.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var points
                      = from l in lines
                        let p = l.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                        let lng = p.First()
                        let lat = p.Last()
                        select new LatLng
                        {
                            Lat = double.Parse(lat),
                            Lng = double.Parse(lng)
                        };
                return new List<IEnumerable<LatLng>> { points };
            }

            /*
             MULTILINESTRING ((101.5698675758791 3.1691293035556045, 101.56849998641755 3.1667900062393111), (101.57085827548453 3.1654261791654958, 101.56849998641755 3.1667900062393111, 101.56724997514212 3.1646400149318241, 101.56714405462428 3.1644482159477283))
             * */
            if (wkt.StartsWith("MULTILINESTRING"))
            {
                const string pattern = @"\((?<p>\d.*?\d)\)";
                var multiLinestrings = from Match m in Regex.Matches(wkt, pattern)
                                       select m.Groups["p"].Value;

                var list = new List<IEnumerable<LatLng>>();
                foreach (var line in multiLinestrings)
                {
                    var spatialPoints = line.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var points
                          = from l in spatialPoints
                            let p = l.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                            let lng = p.First()
                            let lat = p.Last()
                            select new LatLng
                            {
                                Lat = double.Parse(lat),
                                Lng = double.Parse(lng)
                            };
                    list.Add(points);

                }

                return list;
            }

            return new List<IEnumerable<LatLng>>();
        }

        public static IEnumerable<string> ConvertWktToEncodedPolylines(string wkt)
        {
            if (string.IsNullOrWhiteSpace(wkt)) return new List<string>();


            var points = from l in ConvertWktToLines(wkt)
                         select EncodeLatLong(l.ToList());
            return points;
        }



        /// <summary>
        /// encoded a list of latlon objects into a string
        /// </summary>
        /// <param name="points">the list of latlon objects to encode</param>
        /// <returns>the encoded string</returns>
        public static string EncodeLatLong(IList<LatLng> points)
        {
            int plat = 0;
            int plng = 0;
            int len = points.Count;

            var encodedPoints = new StringBuilder();

            for (int i = 0; i < len; ++i)
            {
                //Round to 5 decimal places and drop the decimal
                var late5 = (int)(points[i].Lat * 1e5);
                var lnge5 = (int)(points[i].Lng * 1e5);

                //encode the differences between the points
                encodedPoints.Append(EncodeSignedNumber(late5 - plat));
                encodedPoints.Append(EncodeSignedNumber(lnge5 - plng));

                //store the current point
                plat = late5;
                plng = lnge5;
            }
            return encodedPoints.ToString();
        }

        /// <summary>
        /// Encode a signed number in the encode format.
        /// </summary>
        /// <param name="num">the signed number</param>
        /// <returns>the encoded string</returns>
        private static string EncodeSignedNumber(int num)
        {
            int sgnNum = num << 1; //shift the binary value
            if (num < 0) //if negative invert
            {
                sgnNum = ~(sgnNum);
            }
            return (EncodeNumber(sgnNum));
        }

        /// <summary>
        /// Encode an unsigned number in the encode format.
        /// </summary>
        /// <param name="num">the unsigned number</param>
        /// <returns>the encoded string</returns>
        private static string EncodeNumber(int num)
        {
            var encodeString = new StringBuilder();
            const decimal minAscii = 63;
            while (num >= 0x20)
            {
                //while another chunk follows
                encodeString.Append((char)((0x20 | (num & 0x1f)) + minAscii));
                //OR value with 0x20, convert to decimal and add 63
                const int binaryChunkSize = 5;
                num >>= binaryChunkSize; //shift to next chunk
            }
            encodeString.Append((char)(num + minAscii));
            return encodeString.ToString();
        }

        /// <summary>
        /// Decode a Google Encoded GPolyline
        /// </summary>
        /// <remarks>
        /// The Google Encoded GPolyline specification is defined
        /// at http://code.google.com/apis/maps/documentation/polylinealgorithm.html
        /// </remarks>
        /// <param name="gPolyString">Encoded GPolyline</param>
        /// <returns>Ordered list of floating point numbers</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the GPolyline has a malformed byte</exception>
        public static Collection<double> DecodeGPolyline(string gPolyString)
        {
            const int maxGPolyChunks = 7;
            var gPolyElementList = new Collection<double>();

            //
            // Step 11: Convert the GPolyline string back into an array of chunks
            //

            var gPolyBytes = Encoding.UTF8.GetBytes(gPolyString);

            //
            // Walk through the array of chunks splitting it into a list of elements
            //

            var tempChunk = new byte[maxGPolyChunks];
            int tempChunkIndex = 0;

            foreach (byte t in gPolyBytes)
            {
                //
                // A element cannot have more than 7 chunks
                //
                if (tempChunkIndex >= maxGPolyChunks)
                    throw new ArgumentOutOfRangeException();
                //
                // Step 10 & 9: Chunks are ASCII "base" 63 values
                //
                if (t < 63)
                    throw new ArgumentOutOfRangeException();

                tempChunk[tempChunkIndex] = (byte)(t - 63);
                //
                // A chunk is a 6 bit value: 5 bits data with an upper continuation bit
                //
                if ((tempChunk[tempChunkIndex] & 0xC0) != 0)
                    throw new ArgumentOutOfRangeException();

                if ((tempChunk[tempChunkIndex] & 0x20) != 0)
                {
                    //
                    // Step 8: Clear the continuation bit
                    //
                    tempChunk[tempChunkIndex] &= 0x1F;
                    tempChunkIndex++;
                    continue;
                }

                //
                // Step 7 & 6: Reverse the chunk order and convert to a 32-bit integer
                uint polyPointDecimal = 0;

                for (var l = 0; l < tempChunkIndex + 1; l++)
                {
                    polyPointDecimal += (uint)((1 << (5 * l)) * tempChunk[l]);
                }

                //
                // Step 5: If the low order bit is set, the original value was negative, invert.
                if ((polyPointDecimal & 0x1) == 1)
                    polyPointDecimal = ~polyPointDecimal;

                //
                // Step 4: Right shift the value 1 bit
                // Step 3 & 2: Convert back to floating point by dividing by 1e5
                var polyPoint = (double)((int)polyPointDecimal >> 1) / 100000;

                //
                // Add chunks to the element list
                gPolyElementList.Add(polyPoint);

                //
                // Start a new chunk
                //

                Array.Clear(tempChunk, 0, maxGPolyChunks);
                tempChunkIndex = 0;
            }

            //
            // The last chunk in the GPolyline must not be a continution chunk
            //

            if (tempChunkIndex != 0)
                throw new ArgumentOutOfRangeException();

            return gPolyElementList;
        }

        /// <summary>
        /// Decode a Google Encoded GPolyline into a LocationCollection
        /// </summary>
        /// <remarks>
        /// The Google Encoded GPolyline specification is defined
        /// at http://code.google.com/apis/maps/documentation/polylinealgorithm.html
        /// </remarks>
        /// <param name="polyString">Encoded GPolyline</param>
        /// <param name="polyPoints">Collection Locations will be added to</param>
        /// <returns>Void</returns>
        /// <exception cref="System.ArgumentException">Thrown when there are no points in the GPolyline or an odd number of points</exception>
        public static IEnumerable<LatLng> Decode(this string polyString)
        {
            //
            // The caller may have sent us an empty string
            IList<LatLng> polyPoints = new List<LatLng>();
            if (String.IsNullOrWhiteSpace(polyString))
                return polyPoints;

            var polyElementList = DecodeGPolyline(polyString);

            if ((polyElementList.Count % 2) != 0)
                throw new ArgumentException();

            if (polyElementList.Count == 0)
                return polyPoints;

            var lastLocation = new LatLng();
            bool isLat = true;
            LatLng elLoc = null;

            foreach (var el in polyElementList)
            {
                if (isLat)
                {
                    elLoc = new LatLng { Lat = el + lastLocation.Lat };

                    isLat = false;
                }
                else
                {
                    elLoc.Lng = el + lastLocation.Lng;
                    isLat = true;
                    polyPoints.Add(elLoc);
                    lastLocation = elLoc;
                }
            }

            return polyPoints;
        }
    }
}