using System;
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
            base.Initialize(resolver);
        }

        private readonly ConcurrentBag<string> m_connections = new ConcurrentBag<string>();
        private void SourceChanged(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath.Contains("BinaryStores")) return;
            try
            {
                var solution = GetSolution();
                solution.changedType = e.ChangeType.ToString();
                solution.item = new SolutionItem { text = e.Name, id = e.FullPath, changedType = e.ChangeType.ToString() };

                var conns = m_connections.ToArray();
                this.Connection?.Send(conns.ToList(), solution);
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

            ExtractEntityForms(solution);
            ExtractEntityView(solution);
            ExtractTrigger(solution);


            foreach (var folder in Directory.GetDirectories(ConfigurationManager.SphSourceDirectory))
            {
                if (folder == ConfigurationManager.GeneratedSourceDirectory) continue;
                if (folder.Contains("BinaryStore")) continue;
                if (folder.Contains("_generated")) continue;
                if (folder.Contains("Setting")) continue;
                if (folder.Contains("EntityDefinition")) continue;
                if (folder.Contains("EntityForm")) continue;
                if (folder.Contains("EntityView")) continue;
                if (folder.Contains("EntityChart")) continue;
                if (folder.Contains("Trigger")) continue;
                if (folder.Contains("Page")) continue;

                var parent = new SolutionItem
                {
                    text = Path.GetFileName(folder),
                    id = Path.GetFileName(folder),
                    icon = GetIcon(Path.GetFileName(folder))
                };
                solution.itemCollection.Add(parent);
                foreach (var f in Directory.GetFiles(folder, "*.json"))
                {
                    parent.itemCollection.Add(new SolutionItem
                    {
                        id = Path.GetFileNameWithoutExtension(f),
                        text = Path.GetFileNameWithoutExtension(f),
                        url = GetEditUrl(Path.GetFileName(folder), f),
                        icon = GetIcon(Path.GetFileName(folder))
                    });
                }
            }

            var customRoutes = new SolutionItem { id = "custom.forms", text = "Custom Forms", icon = "fa fa-file-o" };
            var crFile = $"{ConfigurationManager.WebPath}\\App_Data\\routes.config.json";
            if (File.Exists(crFile))
            {
                var routes = from r in
                    JsonConvert.DeserializeObject<JsRoute[]>(File.ReadAllText(crFile))
                             select new SolutionItem
                             {
                                 id = r.ModuleId,
                                 text = r.Title,
                                 icon = "fa fa-code",
                                 codeEditor = $"/sphapp/{r.ModuleId}.js"
                             };
                customRoutes.itemCollection.AddRange(routes);

            }
            solution.itemCollection.AddRange(customRoutes);

            var scriptNode = new SolutionItem { id = "custom.scrpts", text = "Custom Scripts", icon = "fa fa-file-o" };
            var scriptConfig = $"{ConfigurationManager.WebPath}\\App_Data\\custom-script.json";
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
            solution.itemCollection.AddRange(scriptNode);

            var dialogNode = new SolutionItem
            {
                id = "custom.dialogs",
                text = "Custom Dialogs",
                icon = "fa fa-file-o",
                createDialog = "custom.form.dialog.dialog"

            };
            var dialogConfig = $"{ConfigurationManager.WebPath}\\App_Data\\custom-dialog.json";
            if (File.Exists(dialogConfig))
            {
                var scripts = JArray.Parse(File.ReadAllText(scriptConfig))
                    .Select(a => a.SelectToken("name").Value<string>())
                        .Select(x => new SolutionItem
                        {
                            icon = "fa fa-file-text-o",
                            text = x.ToString(),
                            id = x.ToString(),
                            codeEditor = $"/sphapp/viewmodels/{x}.js"
                        });
                dialogNode.itemCollection.AddRange(scripts);

            }
            solution.itemCollection.AddRange(dialogNode);

            var partialViews = new SolutionItem
            {
                id = "partial.views",
                text = "Partial Views",
                icon = "fa fa-file-o",
                createDialog = "custom.form.dialog.dialog",
                createdUrl = ""
            };
            solution.itemCollection.AddRange(partialViews);


            return solution;
        }

        private static void ExtractTrigger(SolutionItem solution)
        {
            foreach (var f in Directory.GetFiles($"{ConfigurationManager.SphSourceDirectory}\\Trigger", "*.json"))
            {
                var trigger = f.DeserializeFromJsonFile<Trigger>();
                var parent =
                    solution.itemCollection.SingleOrDefault(
                        x => x.id.Equals(trigger.Entity, StringComparison.InvariantCultureIgnoreCase));

                parent?.itemCollection.Add(new SolutionItem
                {
                    id = Path.GetFileNameWithoutExtension(f),
                    text = Path.GetFileNameWithoutExtension(f),
                    icon = "fa fa-bolt",
                    url = $"trigger.setup/{trigger.Id}"
                });
            }
        }

        private static void ExtractEntityView(SolutionItem solution)
        {
            foreach (var f in Directory.GetFiles($"{ConfigurationManager.SphSourceDirectory}\\EntityView", "*.json"))
            {
                var view = f.DeserializeFromJsonFile<EntityView>();
                var parent =
                    solution.itemCollection.SingleOrDefault(
                        x => x.id.Equals(view.EntityDefinitionId, StringComparison.InvariantCultureIgnoreCase));

                parent?.itemCollection.Add(new SolutionItem
                {
                    id = Path.GetFileNameWithoutExtension(f),
                    text = Path.GetFileNameWithoutExtension(f),
                    icon = "fa fa-list-ul",
                    url = $"entity.view.designer/{parent.id}/{view.Id}"
                });
            }
        }

        private static void ExtractEntityForms(SolutionItem solution)
        {
            foreach (var f in Directory.GetFiles($"{ConfigurationManager.SphSourceDirectory}\\EntityForm", "*.json"))
            {
                var form = f.DeserializeFromJsonFile<EntityForm>();
                var parent =
                    solution.itemCollection.SingleOrDefault(
                        x => x.id.Equals(form.EntityDefinitionId, StringComparison.InvariantCultureIgnoreCase));

                parent?.itemCollection.Add(new SolutionItem
                {
                    id = Path.GetFileNameWithoutExtension(f),
                    text = Path.GetFileNameWithoutExtension(f),
                    icon = "fa fa-pencil-square-o",
                    url = $"entity.form.designer/{parent.id}/{form.Id}"
                });
            }
        }

        private IList<SolutionItem> GetEntityDefinition()
        {
            var list = new List<SolutionItem>();
            foreach (var f in Directory.GetFiles($"{ConfigurationManager.SphSourceDirectory}\\EntityDefinition", "*.json"))
            {
                var ed = f.DeserializeFromJsonFile<EntityDefinition>();
                var entity = new SolutionItem
                {
                    id = ed.Id,
                    text = ed.Name,
                    icon = "fa fa-database",
                    url = "entity.details/" + ed.Id
                };
                var ops =
                    ed.EntityOperationCollection.Select(
                        x =>
                            new SolutionItem
                            {
                                id = x.Name,
                                text = x.Name,
                                icon = "fa fa-keyboard-o",
                                url = $"entity.operation.details/{ed.Id}/{x.Name}"
                            });
                entity.itemCollection.AddRange(ops);

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
            if (folder == nameof(Designation))
            {
                var d = file.DeserializeFromJsonFile<Designation>();
                return $"role.settings/{d.Id}";
            }
            if (folder == nameof(TransformDefinition))
            {
                var map = file.DeserializeFromJsonFile<Designation>();
                return $"transform.definition.edit/{map.Id}";
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
            m_watcher?.Dispose();
            m_watcher = null;
        }

        private string GetIcon(string name)
        {
            switch (name)
            {
                case nameof(WorkflowDefinition): return "fa fa-code-fork";
                case nameof(TransformDefinition): return "fa fa-random";
                case nameof(EntityDefinition): return "fa fa-file-o";
                case nameof(Adapter): return "fa fa-puzzle-piece";
                case nameof(Designation): return "fa fa-users";
                case nameof(EmailTemplate): return "fa fa-envelope-o";
                case nameof(DocumentTemplate): return "fa fa-file-word-o";
                case nameof(ReportDefinition): return "fa fa-bar-chart-o";
                case "CustomForm": return "fa fa-file-o";
                case "PartialView": return "fa fa-file-o";
                case "CustomScript": return "fa fa-file-o";
                case "CustomDialog": return "fa fa-file-o";
            }
            return "fa fa-file-o";
        }
    }
}