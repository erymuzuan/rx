using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class AssemblyField : Field
    {
        public override object GetValue(RuleContext context)
        {
            var dll = Assembly.LoadFrom(this.Location);
            var type = dll.GetType(this.TypeName);
            if (null == type)
                throw new InvalidOperationException("Cannot load type " + this.TypeName);
            var obj = Activator.CreateInstance(type);

            // we can't use type.GetMethod(name, parameterTypes) because the arg collection might not be in order
            var methods = type.GetMethods();
            var count = methods.Count(m => m.Name == this.Method && m.GetParameters().Length == this.MethodArgCollection.Count);
            if (0 == count)
                throw new InvalidOperationException("Cannot load method " + this.Method + " with these parameters");

            if (count > 1) 
                throw new InvalidOperationException("This is not yet possible to have overloaded method with same parameters length ");


            var method = methods.Single(m => m.Name == this.Method && m.GetParameters().Length == this.MethodArgCollection.Count);
            var args = method.GetParameters();
            if (args.Length != this.MethodArgCollection.Count)
                throw new InvalidOperationException(string.Format("Expected {0} args but you had supplied {1} args instead", args.Length, MethodArgCollection.Count));

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
                object result = null;
                var task = method.Invoke(obj, temp.ToArray()) as Task<object>;
                if (null == task)
                    throw new InvalidOperationException("Only Task<object> is supported for now");
                task.ContinueWith(_ =>
                {
                    result = _.Result;
                }).Wait(this.AsyncTimeout);

                return result;

            }

            return method.Invoke(obj, temp.ToArray());
        }
    }
}