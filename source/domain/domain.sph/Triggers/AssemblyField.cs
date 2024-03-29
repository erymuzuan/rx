using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class AssemblyField : Field
    {
        public override object GetValue(RuleContext context)
        {
            if (this.LoadInCurrentAppDomain)
            {
                return ExecuteInNewAppDomain(context);
            }
            var type = Strings.GetType(this.TypeName + ", " + this.Location);
            if (null == type)
                throw new InvalidOperationException("Cannot load type " + this.TypeName);
            var obj = Activator.CreateInstance(type);

            // we can't use type.GetMethod(name, parameterTypes) because the arg collection might not be in order
            var methods = type.GetMethods();
            var count = methods.Count(m => m.Name == this.Method && m.GetParameters().Length == this.MethodArgCollection.Count);
            if (0 == count)
                throw new InvalidOperationException("Cannot load method " + this.Method + " with these parameters");

            if (count > 1)
                throw new InvalidOperationException("This is not yet possible to have overloaded method with same parameters length");


            var method = methods.Single(m => m.Name == this.Method && m.GetParameters().Length == this.MethodArgCollection.Count);
            var args = method.GetParameters();
            if (args.Length != this.MethodArgCollection.Count)
                throw new InvalidOperationException(
                    $"Expected {args.Length} args but you had supplied {MethodArgCollection.Count} args instead");

            var temp = new List<object>();
            foreach (var g in args)
            {
                var g1 = g;
                if (this.MethodArgCollection.Count(p => p.Name == g1.Name) != 1)
                    throw new InvalidOperationException("Cannot find the parameter " + g1);

                var arg = this.MethodArgCollection.Single(p => p.Name == g1.Name);
                var value = arg.GetValue(context);
                temp.Add(value);
            }

            if (this.IsAsync)
            {
                if (method.Invoke(obj, temp.ToArray()) is Task<object> taskObject) return taskObject.Result;

                if (method.Invoke(obj, temp.ToArray()) is Task<string> taskString) return taskString.Result;

                if (method.Invoke(obj, temp.ToArray()) is Task<int> taskInt32) return taskInt32.Result;

                if (method.Invoke(obj, temp.ToArray()) is Task<bool> taskBoolean) return taskBoolean.Result;

                if (method.Invoke(obj, temp.ToArray()) is Task<DateTime> taskDateTime) return taskDateTime.Result;

                if (method.Invoke(obj, temp.ToArray()) is Task<decimal> taskDecimal) return taskDecimal.Result;

                dynamic taskDynamic = method.Invoke(obj, temp.ToArray());
                if (null != taskDynamic) return taskDynamic.Result;

                throw new InvalidOperationException("Only Task<object> is supported for now");

            }

            return method.Invoke(obj, temp.ToArray());
        }

        private object ExecuteInNewAppDomain(RuleContext context)
        {
            throw new NotImplementedException();
        }

        public override string GenerateAsyncCode()
        {
            if (!this.IsAsync)
                throw new InvalidOperationException("Cannot generate async code for non asynchoronous method, method should return Task or Task<T>");

            var code = new StringBuilder();
            code.Append($"var @ = new ");
            return code.ToString();
        }
    }
}