using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace subscriber.entities
{
    public class EntityFormSubscriber : Subscriber<EntityForm>
    {
        public override string QueueName
        {
            get { return "ed_form_gen"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { typeof(EntityForm).Name + ".changed.Publish" }; }

        }

        [ImportMany(FormCompilerMetadataAttribute.FORM_COMPILER_CONTRACT, typeof(FormCompiler), AllowRecomposition = true)]
        public Lazy<FormCompiler, IFormCompilerMetadata>[] Compilers { get; set; }


        protected async override Task ProcessMessage(EntityForm item, MessageHeaders header)
        {
            if (null == this.Compilers)
                ObjectBuilder.ComposeMefCatalog(this);
            if (null == this.Compilers)
                throw new InvalidOperationException("Cannot initiate MEF");

            foreach (var name in item.CompilerCollection)
            {

                var name1 = name;
                var lazy = this.Compilers.SingleOrDefault(x => x.Metadata.Name == name1);
                if (null == lazy)
                    throw new InvalidOperationException("Cannot find compiler " + name);
                var compiler = lazy.Value;
                var result = await compiler.CompileAsync(item);
                var errors = string.Join("\r\n", result.Errors.ToString());
                this.WriteMessage("{2} to compile {0} with {1}\r\n{3}", item.Name, name, result.Result ? "Successfully" : "Failed", errors);

            }

        }



    }
}