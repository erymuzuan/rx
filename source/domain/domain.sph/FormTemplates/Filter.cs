﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class Filter : DomainObject
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
                Value = $"{value}",
                Type = value?.GetType()
            };
        }

        public Filter(string term, Operator op, object value)
        {
            this.Term = term;
            this.Operator = op;
            this.Field = new ConstantField
            {
                Value = $"{value}",
                Type = value?.GetType()
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


        public override string ToString()
        {
            object value = null;
            try
            {
                value = Field.GetValue(default);
                if (value is string sv)
                    value = $@"""{sv}""";
            }
            catch (Exception)
            {
                //ignore
            }
            return $"{Term} {Operator} {value}";
        }
    }
}