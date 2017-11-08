using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public sealed partial class Filter : DomainObject
    {
        public Filter()
        {
        }

        public Filter(string term, object value)
        {
            this.Term = term;
            this.Operator = Operator.Eq;
            this.Field = new ConstantField
            {
                Value = value as string,
                Type = typeof(string)
            };
        }

        public Filter(string term, Operator op, object value)
        {
            this.Term = term;
            this.Operator = op;
            this.Field = new ConstantField
            {
                Value = $"{value}",
                Type = value.GetType()
            };
        }


        public async Task<IEnumerable<BuildError>> ValidateErrorsAsync()
        {
            var errors = new List<BuildError>();
            if (string.IsNullOrWhiteSpace(this.Term))
                errors.Add(new BuildError(this.WebId, "Term cannot be empty"));
            if (null == this.Field)
                errors.Add(new BuildError(this.WebId, $"Filed cannot be null for {Term} filter"));

            if (null != this.Field)
            {
                var fieldErrors = await this.Field.ValidateErrorsAsync(this);
                errors.AddRange(fieldErrors);
            }

            return errors;
        }

        public Task<IEnumerable<BuildError>> ValidateWarningsAsync()
        {
            return Task.FromResult(Array.Empty<BuildError>().AsEnumerable());
        }

        public static Filter[] Parse(string query)
        {
            var queries = query.Split(new[] { "and", "or" }, StringSplitOptions.RemoveEmptyEntries);
            var filters = from q in queries
                let words = q.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                let term = words[0]
                let op = (Operator)Enum.Parse(typeof(Operator), words[1], true)
                let value = words[2] // TODO:  create aprroriate constant field for value
                select new Filter(term, op, value);

            return filters.ToArray();
        }
    }
}