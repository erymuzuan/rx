﻿using System;
using System.Collections.Concurrent;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Extensions;
using Bespoke.Sph.Web.Solutions;
using Microsoft.AspNet.SignalR;

namespace Bespoke.Sph.Web.Hubs
{
    public class SolutionConnection : PersistentConnection, IDisposable
    {

        [ImportMany(typeof(IItemsProvider), AllowRecomposition = true)]
        public IItemsProvider[] ItemsProviders { get; set; }

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
        private async void SourceChanged(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath.Contains("BinaryStores")) return;
            try
            {
                var directory = Directory.GetParent(e.FullPath).Name;
                var item = new SolutionItem
                {
                    changedType = e.ChangeType.ToString(),
                    text = e.Name,
                    id = $"__{directory}__{Path.GetFileNameWithoutExtension(e.FullPath)}"
                };

                var hasItem = e.ChangeType == WatcherChangeTypes.Created ||
                    e.ChangeType == WatcherChangeTypes.Changed;

                if (hasItem && null != this.ItemsProviders)
                {
                    await WaitReadyAsync(e.FullPath);
                    var tasks = this.ItemsProviders.Select(x => x.GetItemAsync(e.FullPath));
                    var items = await Task.WhenAll(tasks);
                    item = items.FirstOrDefault(x => null != x);
                    if (null != item)
                    {
                        item.changedType = e.ChangeType.ToString();
                    }

                }

                var conns = m_connections.ToArray();
                var send = this.Connection?.Send(conns.ToList(), item);
                if (send != null)
                    await send;
            }
            catch (Exception exception)
            {
                var broadcast = this.Connection?.Broadcast(new LogEntry(exception));
                if (broadcast != null)
                    await broadcast;
            }

        }

        public async Task WaitReadyAsync(string fileName)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            while (true)
            {
                try
                {
                    using (Stream stream = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        logger.WriteVerbose($"Output file {fileName} ready. " + stream);
                        break;
                    }
                }
                catch (FileNotFoundException ex)
                {
                    logger.WriteVerbose($"Output file {fileName} not yet ready ({ex.Message})");
                }
                catch (IOException ex)
                {
                    logger.WriteVerbose($"Output file {fileName} not yet ready ({ex.Message})");
                }
                catch (UnauthorizedAccessException ex)
                {
                    logger.WriteVerbose($"Output file {fileName} not yet ready ({ex.Message})");
                }
                await Task.Delay(500);
            }
        }

        private SolutionItem GetSolution()
        {
            if (null == this.ItemsProviders)
                ObjectBuilder.ComposeMefCatalog(this);
            if (null == this.ItemsProviders)
                throw new InvalidOperationException("Fail to initialize MEF");

            var solution = new SolutionItem();
            var providers = this.ItemsProviders.Distinct().ToArray();


            foreach (var provider in providers)
            {
                var items = provider.GetItemsAsync(null).Result;
                solution.AddItems(items.ToArray());
            }

            foreach (var provider in providers)
            {
                var item = provider.GetItemAsync().Result;
                solution.AddItems(item);
            }

            foreach (var node in solution.itemCollection)
            {
                foreach (var provider in this.ItemsProviders)
                {
                    var children = provider.GetItemsAsync(node).Result;
                    node.AddItems(children.ToArray());
                }
            }

            return solution;
        }

        protected override Task OnConnected(IRequest request, string connectionId)
        {
            var developer = request?.User?.IsInRole("developers");
            if (developer ?? false)
            {
                try
                {
                    m_connections.Add(connectionId);
                    return Connection.Send(connectionId, GetSolution());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Connection.Send(connectionId, new LogEntry(e));
                }
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

    }
}