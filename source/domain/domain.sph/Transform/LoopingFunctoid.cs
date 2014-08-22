using System;
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
            code.AppendLinf("               select new {0} {{", childType.FullName);

            var childMaps = this.TransformDefinition.MapCollection.OfType<DirectMap>()
                .Where(d => d.Source.StartsWith(source.Field));
            foreach (var map in childMaps)
            {
                code.AppendLinf("{0} = r.{1},", map.Destination.Replace(dd2.Destination + ".", ""), map.Source.Replace(source.Field + ".", ""));
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