﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable InconsistentNaming

namespace Bespoke.Sph.Web.Hubs
{
    public class SolutionConnection : PersistentConnection, IDisposable
    {
        public class SolutionItem
        {

            public string id { get; set; }
            public string changedType { get; set; }
            public SolutionItem item { get; set; }
            public string text { get; set; }
            public string icon { get; set; }
            public string type { get; set; }
            public string url { get; set; }
            public string dialog { get; set; }
            public string createDialog { get; set; }
            public string createdUrl { get; set; }
            public string codeEditor { get; set; }

            public ObjectCollection<SolutionItem> itemCollection { get; } = new ObjectCollection<SolutionItem>();
        }
        public class Solution
        {
            public ObjectCollection<SolutionItem> itemCollection { get; } = new ObjectCollection<SolutionItem>();
        }

        private FileSystemWatcher m_watcher;
        private FileSystemWatcher m_appDataWatcher;
        public override void Initialize(IDependencyResolver resolver)
        {
            m_watcher = new FileSystemWatcher(ConfigurationManager.SphSourceDirectory, "*.json")
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = true
            };

            m_watcher.Changed += SourceChanged;
            m_watcher.Deleted += SourceChanged;
            m_watcher.Created += SourceChanged;
            m_watcher.Renamed += SourceChanged;

            m_appDataWatcher = new FileSystemWatcher($"{ConfigurationManager.WebPath}\\App_Data", " *.json")
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = false
            };
            m_appDataWatcher.Changed += SourceChanged;
            base.Initialize(resolver);
        }


        private readonly ConcurrentBag<string> m_connections = new ConcurrentBag<string>();
        private void SourceChanged(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath.Contains("BinaryStores")) return;
            try
            {
                var item = new SolutionItem
                {
                    changedType = e.ChangeType.ToString(),
                    text = e.Name,
                    id = e.FullPath
                };

                var conns = m_connections.ToArray();
                this.Connection?.Send(conns.ToList(), item);
            }
            catch (Exception exception)
            {
                this.Connection?.Broadcast(exception);
            }

        }

        private SolutionItem GetSolution()
        {
            var solution = new SolutionItem();

            var entities = GetEntityDefinition();
            solution.itemCollection.AddRange(entities.ToArray());

            ExtractSolutionItems<QueryEndpoint>(solution);
            ExtractSolutionItems<OperationEndpoint>(solution);
            ExtractSolutionItems<EntityForm>(solution);
            ExtractSolutionItems<EntityView>(solution);
            ExtractSolutionItems<Trigger>(solution);
            ExtractSolutionItems<FormDialog>(solution);
            ExtractSolutionItems<PartialView>(solution);


            foreach (var folder in Directory.GetDirectories(ConfigurationManager.SphSourceDirectory))
            {
                if (folder == ConfigurationManager.GeneratedSourceDirectory) continue;
                if (folder.Contains("BinaryStore")) continue;
                if (folder.Contains("_generated")) continue;
                if (folder.Contains("Setting")) continue;
                if (folder.Contains("EntityDefinition")) continue;
                if (folder.Contains("EntityForm")) continue;
                if (folder.Contains(nameof(OperationEndpoint))) continue;
                if (folder.Contains(nameof(QueryEndpoint))) continue;
                if (folder.Contains("EntityView")) continue;
                if (folder.Contains("EntityChart")) continue;
                if (folder.Contains("FormDialog")) continue;
                if (folder.Contains("PartialView")) continue;
                if (folder.Contains("Trigger")) continue;
                if (folder.Contains("Page")) continue;

                var parent = new SolutionItem
                {
                    text = Path.GetFileName(folder),
                    id = Path.GetFileName(folder),
                    icon = GetIcon(Path.GetFileName(folder), null)
                };
                foreach (var f in Directory.GetFiles(folder, "*.json"))
                {
                    var name = Path.GetFileNameWithoutExtension(f);

                    var jo = JObject.Parse(File.ReadAllText(f));
                    var node = (jo.SelectToken("$.Name") ?? jo.SelectToken("$.Title")).ToEmptyString();
                    if (!string.IsNullOrWhiteSpace(node))
                        name = node;
                    var item = new SolutionItem
                    {
                        id = "__" + parent.text + "__" + Path.GetFileNameWithoutExtension(f),
                        text = name,
                        url = GetEditUrl(Path.GetFileName(folder), f),
                        icon = GetIcon(Path.GetFileName(folder), jo)
                    };
                    if (!string.IsNullOrWhiteSpace(item.url))
                        parent.itemCollection.Add(item);
                }
                // TODO : do not include source EntityDefinition
                if (parent.itemCollection.Count > 0)
                    solution.itemCollection.Add(parent);
            }

            var customRoutes = GetCustomRoutes();
            solution.itemCollection.AddRange(customRoutes);

            string scriptConfig;
            var scriptNode = GetScripts(out scriptConfig);
            solution.itemCollection.AddRange(scriptNode);

            var dialogNode = GetCustomDialogs();
            solution.itemCollection.AddRange(dialogNode);

            var partialViewNode = GetPartialViewNode();
            solution.itemCollection.AddRange(partialViewNode);


            return solution;
        }

        private static SolutionItem GetCustomRoutes()
        {
            var routes = new SolutionItem { id = "custom.forms", text = "Custom Forms", icon = "fa fa-edit" };
            var config = $"{ConfigurationManager.WebPath}\\App_Data\\routes.config.json";
            if (!File.Exists(config)) return routes;
            var scripts = from r in JsonConvert.DeserializeObject<JsRoute[]>(File.ReadAllText(config))
                          where !string.IsNullOrWhiteSpace(r.ModuleId)
                          let name = r.ModuleId.Replace("viewmodels/", "")
                          select new SolutionItem
                          {
                              id = $"{r.ModuleId}.js",
                              text = $"{name}.js",
                              icon = "fa fa-file-text-o",
                              codeEditor = $"/sphapp/{r.ModuleId}.js"
                          };
            var views = from r in JsonConvert.DeserializeObject<JsRoute[]>(File.ReadAllText(config))
                        where !string.IsNullOrWhiteSpace(r.ModuleId)
                        let name = r.ModuleId.Replace("viewmodels/", "")
                        select new SolutionItem
                        {
                            id = $"{r.ModuleId}.html",
                            text = $"{name}.html",
                            icon = "fa fa-file-code-o",
                            codeEditor = $"/sphapp/views/{name}.html"
                        };
            var forms = views.Concat(scripts).OrderBy(x => x.text);
            routes.itemCollection.AddRange(forms);
            return routes;
        }

        private static SolutionItem GetScripts(out string scriptConfig)
        {
            var scriptNode = new SolutionItem { id = "custom.scrpts", text = "Custom Scripts", icon = "fa fa-file-text-o" };
            scriptConfig = $"{ConfigurationManager.WebPath}\\App_Data\\custom-script.json";
            if (File.Exists(scriptConfig))
            {
                var scripts = JArray.Parse(File.ReadAllText(scriptConfig))
                    .Select(a => a.SelectToken("name").Value<string>())
                    .Select(x => new SolutionItem
                    {
                        icon = "fa fa-file-text-o",
                        text = x.ToString(),
                        id = x.ToString(),
                        codeEditor = $"/sphapp/services/{x}.js"
                    });
                scriptNode.itemCollection.AddRange(scripts);
            }
            return scriptNode;
        }

        private static SolutionItem GetCustomDialogs()
        {
            var dialogNode = new SolutionItem
            {
                id = "custom.dialogs",
                text = "Custom Dialogs",
                icon = "fa fa-files-o",
                createDialog = "custom.form.dialog.dialog"
            };
            var dialogConfig = $"{ConfigurationManager.WebPath}\\App_Data\\custom-dialog.json";
            if (File.Exists(dialogConfig))
            {
                var scripts = JArray.Parse(File.ReadAllText(dialogConfig))
                    .Select(a => a.SelectToken("name").Value<string>())
                    .Select(x => new SolutionItem
                    {
                        icon = "fa fa-files-o",
                        text = $"{x}.js",
                        id = $"{x}.js",
                        codeEditor = $"/sphapp/viewmodels/{x}.js"
                    });

                var views = JArray.Parse(File.ReadAllText(dialogConfig))
                    .Select(a => a.SelectToken("name").Value<string>())
                    .Select(x => new SolutionItem
                    {
                        icon = "fa fa-file-code-o",
                        text = $"{x}.html",
                        id = $"{x}.html",
                        codeEditor = $"/sphapp/views/{x}.html"
                    });
                var dialogs = scripts.Concat(views).OrderBy(x => x.text);
                dialogNode.itemCollection.AddRange(dialogs);
            }
            return dialogNode;
        }

        private static SolutionItem GetPartialViewNode()
        {
            var partialViewNode = new SolutionItem
            {
                id = "partial.views",
                text = "Partial Views",
                icon = "fa fa-file-o",
                createDialog = "custom.form.dialog.dialog",
                createdUrl = ""
            };
            var partialViewConfig = $"{ConfigurationManager.WebPath}\\App_Data\\custom-partial-view.json";
            if (File.Exists(partialViewConfig))
            {
                var scripts = JArray.Parse(File.ReadAllText(partialViewConfig))
                    .Select(a => a.SelectToken("name").Value<string>())
                    .Select(x => new SolutionItem
                    {
                        icon = "fa fa-file-code-o",
                        text = x.ToString(),
                        id = x.ToString(),
                        codeEditor = $"/sphapp/views/{x}.html"
                    });
                partialViewNode.itemCollection.AddRange(scripts);
            }
            return partialViewNode;
        }

        private static void ExtractSolutionItems<T>(SolutionItem solution) where T : Entity
        {
            var folder = $"{ConfigurationManager.SphSourceDirectory}\\{typeof(T).Name}";
            if (!Directory.Exists(folder)) return;

            foreach (var f in Directory.GetFiles(folder, "*.json"))
            {
                var form = (IEntityDefinitionAsset)f.DeserializeFromJsonFile<T>();
                var parent =
                    solution.itemCollection.SingleOrDefault(
                        x => x.id.Equals(form.Entity, StringComparison.InvariantCultureIgnoreCase));

                parent?.itemCollection.Add(new SolutionItem
                {
                    id = form.Id,
                    text = form.Name,
                    icon = form.Icon,
                    url = form.Url
                });
            }
        }

        private IList<SolutionItem> GetEntityDefinition()
        {
            var list = new List<SolutionItem>();
            var folder = $"{ConfigurationManager.SphSourceDirectory}\\EntityDefinition";
            if (!Directory.Exists(folder)) return list;

            foreach (var f in Directory.GetFiles(folder, "*.json"))
            {
                var ed = f.DeserializeFromJsonFile<EntityDefinition>();
                var entity = new SolutionItem
                {
                    id = ed.Id,
                    text = ed.Name,
                    icon = ed.IconClass ?? "fa fa-database",
                    url = "entity.details/" + ed.Id
                };

                var rules =
                    ed.BusinessRuleCollection.Select(
                        x => new SolutionItem { id = x.Name, text = x.Name, icon = "fa fa-graduation-cap" });
                entity.itemCollection.AddRange(rules);
                list.Add(entity);
            }

            return list;
        }

        private string GetEditUrl(string folder, string file)
        {
            if (folder == nameof(DocumentTemplate))
            {
                var template = file.DeserializeFromJsonFile<DocumentTemplate>();
                return $"document.template.details/{template.Id}";
            }
            if (folder == nameof(EmailTemplate))
            {
                var template = file.DeserializeFromJsonFile<EmailTemplate>();
                return $"email.template.details/{template.Id}";
            }
            if (folder == nameof(ReportDefinition))
            {
                var rdl = file.DeserializeFromJsonFile<ReportDefinition>();
                return $"reportdefinition.edit/{rdl.Id}";
            }
            if (folder == nameof(WorkflowDefinition))
            {
                var d = file.DeserializeFromJsonFile<WorkflowDefinition>();
                return $"workflow.definition.visual/{d.Id}";
            }
            if (folder == nameof(ValueObjectDefinition))
            {
                var d = file.DeserializeFromJsonFile<ValueObjectDefinition>();
                return $"value.object.details/{d.Id}";
            }
            if (folder == nameof(WorkflowForm))
            {
                var d = file.DeserializeFromJsonFile<WorkflowForm>();
                return $"workflow.form.designer/{d.WorkflowDefinitionId}/{d.Id}";
            }
            if (folder == nameof(Designation))
            {
                var d = file.DeserializeFromJsonFile<Designation>();
                return $"role.settings/{d.Id}";
            }
            if (folder == nameof(TransformDefinition))
            {
                var map = file.DeserializeFromJsonFile<TransformDefinition>();
                return $"transform.definition.edit/{map.Id}";
            }
            if (folder == nameof(Adapter))
            {
                var jo = JObject.Parse(File.ReadAllText(file));
                var id = jo.SelectToken("$.Id").ToEmptyString();
                var type = jo.SelectToken("$.$type").ToEmptyString();
                if (type == "Bespoke.Sph.Integrations.Adapters.MySqlAdapter, mysql.adapter")
                    return $"adapter.mysql/{id}";
                if (type == "Bespoke.Sph.Integrations.Adapters.SqlServerAdapter, sqlserver.adapter")
                    return $"adapter.sqlserver/{id}";
                if (type == "Bespoke.Sph.Integrations.Adapters.HttpAdapter, http.adapter")
                    return $"adapter.http/{id}";
                if (type == "Bespoke.Sph.Integrations.Adapters.OrcaleAdapter, oracle.adapter")
                    return $"adapter.oracle/{id}";
            }
            return string.Empty;
        }


        protected override Task OnConnected(IRequest request, string connectionId)
        {
            var developer = request?.User?.IsInRole("developers");
            if (developer ?? false)
            {
                m_connections.Add(connectionId);
                return Connection.Send(connectionId, GetSolution());
            }
            return Task.FromResult(0);
        }


        public void Dispose()
        {
            if (null != m_watcher)
            {
                m_watcher.Changed -= SourceChanged;
                m_watcher.Deleted -= SourceChanged;
                m_watcher.Renamed -= SourceChanged;
                m_watcher.Created -= SourceChanged;
            }
            if (null != m_appDataWatcher)
            {
                m_appDataWatcher.Changed -= SourceChanged;
                m_appDataWatcher.Dispose();
                m_appDataWatcher = null;
            }
            m_watcher?.Dispose();
            m_watcher = null;
        }

        private string GetIcon(string name, JToken jo)
        {
            switch (name)
            {
                case nameof(ValueObjectDefinition): return "fa fa-object-ungroup";
                case nameof(WorkflowDefinition): return "fa fa-code-fork";
                case nameof(TransformDefinition): return "fa fa-random";
                case nameof(EntityDefinition): return "fa fa-file-o";
                case nameof(Adapter):
                    if (null != jo)
                    {
                        var type = jo.SelectToken("$.$type").ToEmptyString();
                        if (type == "Bespoke.Sph.Integrations.Adapters.MySqlAdapter, mysql.adapter")
                            return "fa fa-database";
                        if (type == "Bespoke.Sph.Integrations.Adapters.SqlServerAdapter, sqlserver.adapter")
                            return "fa fa-windows";
                        if (type == "Bespoke.Sph.Integrations.Adapters.HttpAdapter, http.adapter")
                            return "fa fa-html5";
                        if (type == "Bespoke.Sph.Integrations.Adapters.OrcaleAdapter, oracle.adapter")
                            return "fa fa-database";
                    }
                    return "fa fa-puzzle-piece";
                case nameof(Designation): return "fa fa-users";
                case nameof(EmailTemplate): return "fa fa-envelope-o";
                case nameof(DocumentTemplate): return "fa fa-file-word-o";
                case nameof(ReportDefinition): return "fa fa-bar-chart-o";
                case "CustomForm": return "fa fa-edit";
                case "PartialView": return "fa fa-code-o";
                case "CustomScript": return "fa fa-file-o";
                case "CustomDialog": return "fa fa-files-o";
            }
            return "fa fa-file-o";
        }
    }
}