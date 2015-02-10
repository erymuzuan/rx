using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class TransformDefinition : CustomProject
    {

        public async Task<object> TransformAsync(object source)
        {
            var sb = new StringBuilder("{");
            sb.AppendLine();
            var tasks = from m in this.MapCollection
                        select m.ConvertAsync(source);
            var maps = await Task.WhenAll(tasks);
            sb.AppendLine(string.Join(",\r\n    ", maps.ToArray()));
            sb.AppendLine();
            sb.Append("}");
            return sb.ToString();
        }


        public override bool Validate()
        {
            this.FunctoidCollection.ForEach(x => x.TransformDefinition = this);
            this.MapCollection.ForEach(x => x.TransformDefinition = this);
            if (string.IsNullOrWhiteSpace(this.Name))
                return false;
            return this.MapCollection.All(x => x.Validate());

        }


        public void AddFunctoids(params Functoid[] functoids)
        {
            this.FunctoidCollection.AddRange(functoids);
        }
        
        public async Task<BuildValidationResult> ValidateBuildAsync()
        {
            this.FunctoidCollection.ForEach(x => x.TransformDefinition = this);
            this.MapCollection.ForEach(x => x.TransformDefinition = this);

            var result = new BuildValidationResult();
            if (string.IsNullOrWhiteSpace(this.Name))
                result.Errors.Add(new BuildError { Message = "Name cannot be null or empty", ItemWebId = this.WebId });
            var validName = new Regex(@"^[A-Za-z][A-Za-z0-9_]*$");
            if (!validName.Match(this.Name).Success)
                result.Errors.Add(new BuildError(this.WebId) { Message = "Name must start with letter.You cannot use symbol or number as first character" });

            var tasks = from m in this.MapCollection
                        select m.ValidateAsync();
            var maps = (await Task.WhenAll(tasks)).SelectMany(x => x.ToArray())
                .Select(x => new BuildError(x.ErrorLocation, x.Message));
            result.Errors.AddRange(maps);

            var fntTasks = from m in this.FunctoidCollection
                        select m.ValidateAsync();
            var functoidsValidation = (await Task.WhenAll(fntTasks)).SelectMany(x => x.ToArray())
                .Select(x => new BuildError(x.ErrorLocation, x.Message));

            result.Errors.AddRange(functoidsValidation);


            var distintcs = result.Errors.Distinct().ToArray();
            result.Errors.ClearAndAddRange(distintcs);


            result.Result = !result.Errors.Any();
            return result;


        }


        public override Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            throw new Exception("Use ValidateBuildAsync");
        }
    }
}