using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.WebApi
{
    /// <summary>
    /// An attribute that get the Entity from a source file and put it in a ICacheManager
    /// </summary>
    [AttributeUsage( AttributeTargets.Parameter)]
    public sealed class SourceEntityAttribute : ParameterBindingAttribute
    {
        private readonly string m_id;

        public SourceEntityAttribute(string id)
        {
            m_id = id;
        }

        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            if (parameter.ParameterType.IsAssignableFrom(typeof(Entity)))
            {
                return new SourceEntityParameterBinding(parameter, parameter.ParameterType, m_id);
            }
            return parameter.BindAsError("Wrong parameter type, only string or byte[]");
        }
    }

    public class SourceEntityParameterBinding : HttpParameterBinding
    {
        private readonly Type m_type;
        private readonly string m_id;

        public SourceEntityParameterBinding(HttpParameterDescriptor descriptor, Type type, string id)
            : base(descriptor)
        {
            m_type = type;
            m_id = id;
        }


        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider,
                                                    HttpActionContext actionContext,
                                                    CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<AsyncVoid>();
            tcs.SetResult(default(AsyncVoid));
            var item = new object();
            var type = m_type.Name;
            switch (type)
            {
                case nameof(EntityDefinition):item = this.GetFromCache<EntityDefinition>(m_id);break;
                case nameof(OperationEndpoint):item = this.GetFromCache<OperationEndpoint>(m_id);break;
                case nameof(QueryEndpoint):item = this.GetFromCache<QueryEndpoint>(m_id);break;
                case nameof(Trigger):item = this.GetFromCache<Trigger>(m_id);break;
                case nameof(WorkflowDefinition):item = this.GetFromCache<WorkflowDefinition>(m_id);break;
                case nameof(EntityForm):item = this.GetFromCache<EntityForm>(m_id);break;
                case nameof(EntityView):item = this.GetFromCache<EntityView>(m_id);break;
                case nameof(EntityChart):item = this.GetFromCache<EntityChart>(m_id);break;
                case nameof(PartialView):item = this.GetFromCache<PartialView>(m_id);break;
                case nameof(FormDialog):item = this.GetFromCache<FormDialog>(m_id);break;
                case nameof(ReportDefinition):item = this.GetFromCache<ReportDefinition>(m_id);break;
                case nameof(TransformDefinition):item = this.GetFromCache<TransformDefinition>(m_id);break;
                case nameof(Adapter):item = this.GetFromCache<Adapter>(m_id);break;
            }
      

            actionContext.ActionArguments[Descriptor.ParameterName] = item;
            return tcs.Task;
        }

        private T GetFromCache<T>(string id)
        {
            var cache = ObjectBuilder.GetObject<ICacheManager>();
            var item = cache.Get<T>(id);
            if (null != item) return item;


            var file = $"{ConfigurationManager.SphSourceDirectory}\\{m_type.Name}\\{m_id}.json";
            if (!System.IO.File.Exists(file)) throw new InvalidOperationException("Cannot find the source file for " + file);

            var json = System.IO.File.ReadAllText(file);
            item = json.DeserializeFromJson<T>();
            cache.Insert(id, item, file);

            return item;
        } 

        private struct AsyncVoid
        {
        }




    }

}
