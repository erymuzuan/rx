using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "Loop", FontAwesomeIcon = "refresh")]
    public class LoopingFunctoid : Functoid
    {
        public const string DEFAULT_STYLES = "None";

        public override bool Initialize()
        {
            this.ArgumentCollection.Clear();
            this.ArgumentCollection.Add(new FunctoidArg { Name = "sourceCollection", Type = typeof(object) });
            return true;
        }


        public override string GenerateStatementCode()
        {
            var code = new StringBuilder();
            code.AppendLine();

            var source = this["sourceCollection"].GetFunctoid(this.TransformDefinition) as SourceFunctoid;
            if (null == source) throw new InvalidOperationException("Only source functoid is valid for Loop source");

            var dd2 = this.TransformDefinition.MapCollection.OfType<FunctoidMap>()
                .SingleOrDefault(f => f.Functoid == this.WebId);

            if (null == dd2) throw new InvalidOperationException("Cannot determine the destination loop");
            var childType = dd2.DestinationType;
            if (null == childType) throw new InvalidOperationException("The type is not valid for destination child " + dd2.DestinationTypeName);

            code.AppendLinf("var val{0} = from r in item.{1}", this.Index, source.Field);

            var sorted = new List<Functoid>(this.TransformDefinition.FunctoidCollection);
            sorted.Sort(new FunctoidDependencyComparer());

            /*
var val4 = item.SamanCollection.Compoun;
var styles4 = System.Globalization.NumberStyles.None;
             * */
            var functoidStatements = (from f in sorted
                                      where f.GetType() != typeof(LoopingFunctoid)
                                      let statement = f.GenerateStatementCode()
                                      where !string.IsNullOrWhiteSpace(statement)
                                      && statement.Contains(source.Field)
                                      select statement.Replace("var ", "let ")
                                      .Replace(";", "")
                                      .Replace("item." + source.Field, "r")
                                      ).ToList();
            code.Append(string.Join("\r\n", functoidStatements));


            code.AppendLinf("               select new {0} {{", childType.FullName);

            var directMaps = this.TransformDefinition.MapCollection.OfType<DirectMap>()
                .Where(d => d.Source.StartsWith(source.Field));
            foreach (var map in directMaps)
            {
                code.AppendLinf("{0} = r.{1},", map.Destination.Replace(dd2.Destination + ".", ""), map.Source.Replace(source.Field + ".", ""));
            }

            var functoidMaps = this.TransformDefinition.MapCollection.OfType<FunctoidMap>()
                .Where(f => !string.IsNullOrWhiteSpace(f.Destination))
                .Where(f => f.Destination.StartsWith(dd2.Destination + "."))
                .ToList();
            foreach (var map in functoidMaps)
            {
                var fnct = map.GetFunctoid(this.TransformDefinition);
                var assignmentCode = fnct.GenerateAssignmentCode();
                code.AppendLinf("{0} = {1},", map.Destination.Replace(dd2.Destination + ".", ""), assignmentCode);
            }

            code.AppendLine("};");



            code.AppendLinf("dest.{1}.AddRange(val{0});", this.Index, dd2.Destination);


            return code.ToString();
        }
        public override string GenerateAssignmentCode()
        {
            return string.Empty;
        }
    }
}