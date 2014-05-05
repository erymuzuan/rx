using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace offline.generator
{
    public class PageBuilder
    {
        private readonly string m_entityName;
        public EntityDefinition Entity { get; set; }

        private readonly ObjectCollection<EntityForm> m_formCollection = new ObjectCollection<EntityForm>();
        private readonly ObjectCollection<EntityView> m_viewCollection = new ObjectCollection<EntityView>();

        public ObjectCollection<EntityView> ViewCollection
        {
            get { return m_viewCollection; }
        }
        public ObjectCollection<EntityForm> FormCollection
        {
            get { return m_formCollection; }
        }

        public PageBuilder(string entityName)
        {
            m_entityName = entityName;
        }

        public async Task LoadAsync()
        {
            var context = new SphDataContext();
            this.Entity = await context.LoadOneAsync<EntityDefinition>(e => e.Name == m_entityName);

            // forms
            var formQuery = context.EntityForms.Where(f => f.EntityDefinitionId == this.Entity.EntityDefinitionId);
            var formLo = await context.LoadAsync(formQuery);
            this.FormCollection.AddRange(formLo.ItemCollection);

            // views
            var viewQuery = context.EntityViews.Where(f => f.EntityDefinitionId == this.Entity.EntityDefinitionId);
            var viewLo = await context.LoadAsync(viewQuery);
            this.ViewCollection.AddRange(viewLo.ItemCollection);

        }

        public async Task BuildDasboard(string output)
        {
            var outputFolder = output;
            if (!Path.IsPathRooted(output))
                outputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, output);
            var item = this.Entity;
            var context = new SphDataContext();
            var form = await context.LoadOneAsync<EntityForm>(f => f.IsDefault == true
                && f.EntityDefinitionId == item.EntityDefinitionId);
            var vm = new
            {
                item.Name,
                item.Plural,
                item.EntityDefinitionId,
                ApplicationName = ConfigurationManager.ApplicationName.ToCamelCase(),
                Definition = item,
                Forms = this.FormCollection,
                DefaultForm = form,
                AllowedNewForms = this.FormCollection.Where(f => f.IsAllowedNewItem).ToList(),
                Views = this.ViewCollection
            };

            var html = Path.Combine(outputFolder, item.Name.ToLower() + ".html");
            using (var stream = new FileStream("entity.html", FileMode.Open))
            using (var reader = new StreamReader(stream))
            {
                var raw = reader.ReadToEnd();
                var markup = await ObjectBuilder.GetObject<ITemplateEngine>().GenerateAsync(raw, vm);
                File.WriteAllText(html, markup);

            }


            var js = Path.Combine(outputFolder, item.Name.ToLower() + ".js");
            using (var stream = new FileStream("entity.js", FileMode.Open))
            using (var reader = new StreamReader(stream))
            {
                var raw = reader.ReadToEnd();
                var script = await ObjectBuilder.GetObject<ITemplateEngine>().GenerateAsync(raw, vm);
                File.WriteAllText(js, script);
            }

            var appcache = Path.Combine(outputFolder, item.Name.ToLower() + ".appcache");
            using (var stream = new FileStream("entity.appcache", FileMode.Open))
            using (var reader = new StreamReader(stream))
            {
                var raw = reader.ReadToEnd();
                var manifest = await ObjectBuilder.GetObject<ITemplateEngine>().GenerateAsync(raw, vm);
                File.WriteAllText(appcache, manifest);

            }
        }

        public async Task BuildForms(string output)
        {
            var outputFolder = output;
            if (!Path.IsPathRooted(output))
                outputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, output);

            foreach (var form in this.FormCollection)
            {
                var formMarkup = "";
                using (var client = new HttpClient())
                {
                    var uri = ConfigurationManager.BaseUrl + "/Sph/EntityFormRenderer/Html/" + form.Route;
                    formMarkup = await client.GetStringAsync(uri);

                }
                var vm = new
                {
                    form.Name,
                    form.Route,
                    form.EntityFormId,
                    form.EntityDefinitionId,
                    Entity = this.Entity.Name,
                    Form = form,
                    ApplicationName = ConfigurationManager.ApplicationName.ToCamelCase(),
                    FormMarkup = formMarkup
                };
                var html = Path.Combine(outputFolder, form.Route + ".html");
                using (var stream = new FileStream("form.html", FileMode.Open))
                using (var reader = new StreamReader(stream))
                {
                    var raw = reader.ReadToEnd();
                    var markup = await ObjectBuilder.GetObject<ITemplateEngine>().GenerateAsync(raw, vm);
                    File.WriteAllText(html, markup);
                }


                var js = Path.Combine(outputFolder, form.Route + ".js");
                using (var stream = new FileStream("form.js", FileMode.Open))
                using (var reader = new StreamReader(stream))
                {
                    var raw = reader.ReadToEnd();
                    var script = await ObjectBuilder.GetObject<ITemplateEngine>().GenerateAsync(raw, vm);
                    File.WriteAllText(js, script);

                }
            }
        }
    }
}
