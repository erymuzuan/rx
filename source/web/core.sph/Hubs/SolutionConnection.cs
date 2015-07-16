using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Microsoft.AspNet.SignalR;
// ReSharper disable InconsistentNaming

namespace Bespoke.Sph.Web.Hubs
{
    public class SolutionConnection : PersistentConnection//, IDisposable
    {
        public class SolutionItem
        {
            
            public string id { get; set; }
            public string changedType { get; set; }
            public SolutionItem item { get; set; }
            public string text { get; set; }
            public string icon { get; set; }
            public string type { get; set; }
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

        private  SolutionItem GetSolution()
        {
            var solution = new SolutionItem();

            foreach (var f in Directory.GetFiles($"{ConfigurationManager.SphSourceDirectory}\\EntityDefinition", "*.json"))
            {
                var ed = f.DeserializeFromJsonFile<EntityDefinition>();
                var entity = new SolutionItem
                {
                    id = ed.Id,
                    text = ed.Name,
                    icon = "fa fa-database"
                };
                var ops = ed.EntityOperationCollection.Select(x => new SolutionItem { id = x.Name, text = x.Name, icon="fa fa-keyboard-o"});
                entity.itemCollection.AddRange(ops);

                var rules = ed.BusinessRuleCollection.Select(x => new SolutionItem { id = x.Name, text = x.Name, icon="fa fa-graduation-cap"});
                entity.itemCollection.AddRange(rules);
                solution.itemCollection.Add(entity);

            }
            foreach (var f in Directory.GetFiles($"{ConfigurationManager.SphSourceDirectory}\\EntityForm", "*.json"))
            {
                var form = f.DeserializeFromJsonFile<EntityForm>();
                var parent = solution.itemCollection.SingleOrDefault(x => x.id.Equals(form.EntityDefinitionId, StringComparison.InvariantCultureIgnoreCase));

                parent?.itemCollection.Add(new SolutionItem
                {
                    id = Path.GetFileNameWithoutExtension(f),
                    text = Path.GetFileNameWithoutExtension(f),
                    icon = "fa fa-pencil-square-o",
                });
            }
            foreach (var f in Directory.GetFiles($"{ConfigurationManager.SphSourceDirectory}\\EntityView", "*.json"))
            {
                var view = f.DeserializeFromJsonFile<EntityView>();
                var parent = solution.itemCollection.SingleOrDefault(x => x.id.Equals(view.EntityDefinitionId, StringComparison.InvariantCultureIgnoreCase));

                parent?.itemCollection.Add(new SolutionItem
                {
                    id = Path.GetFileNameWithoutExtension(f),
                    text = Path.GetFileNameWithoutExtension(f),
                    icon = "fa fa-list-ul",
                });
            }
            foreach (var f in Directory.GetFiles($"{ConfigurationManager.SphSourceDirectory}\\Trigger", "*.json"))
            {
                var trigger = f.DeserializeFromJsonFile<Trigger>();
                var parent = solution.itemCollection.SingleOrDefault(x => x.id.Equals(trigger.Entity, StringComparison.InvariantCultureIgnoreCase));

                parent?.itemCollection.Add(new SolutionItem
                {
                    id = Path.GetFileNameWithoutExtension(f),
                    text = Path.GetFileNameWithoutExtension(f),
                    icon = "fa fa-bolt",
                });
            }


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
                        icon = GetIcon(Path.GetFileName(folder))
                    });
                }
            }

            
            return solution;
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
                case nameof(DocumentTemplate): return "fa fa-word-o";
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