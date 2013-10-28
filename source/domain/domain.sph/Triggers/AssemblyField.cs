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
            var dll = Assembly.LoadFile(this.Location);
            var type = dll.GetType(this.TypeName);
            if (null == type)
                throw new InvalidOperationException("Cannot load type " + this.TypeName);
            var obj = Activator.CreateInstance(type);
            var method = type.GetMethod(this.Method);
            if (null == method)
                throw new InvalidOperationException("Cannot load method " + this.Method);
            var args = method.GetParameters();
            if (args.Length != this.ParameterCollection.Count)
                throw new InvalidOperationException(string.Format("Expected {0} args but you had supplied {1} args instead", args.Length, ParameterCollection.Count));

            var temp = new List<object>();
            foreach (var g in args)
            {
                var g1 = g;
                if (this.ParameterCollection.Count(p => p.Name == g1.Name) != 1)
                    throw new InvalidOperationException("Cannot find the parameter " + g1);
            
                temp.Add(this.ParameterCollection.Single(p=> p.Name == g1.Name).Value);
            }

            if (this.IsAsync)
            {
                object result = null;
                var task = method.Invoke(obj, temp.ToArray()) as Task<object>;
                if(null == task)
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