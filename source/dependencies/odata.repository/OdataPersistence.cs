using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bespoke.CommercialSpace.Domain;

namespace Bespoke.Sph.OdataRepository
{
    public class OdataPersistence : IPersistence
    {
        private readonly string m_url;

        public OdataPersistence()
        {
            m_url = ConfigurationManager.AppSettings["DataServiceUrl"] + "/Repository/Save";
        }
        public OdataPersistence(string url)
        {
            m_url = url;
        }

        public async Task<SubmitOperation> SubmitChanges(IEnumerable<Entity> addedOrUpdatedItems, IEnumerable<Entity> deletedItems, PersistenceSession session)
        {
            var items = addedOrUpdatedItems.ToList();
            var uri = new Uri(m_url, UriKind.Absolute);
            var changes = new ChangeSubmission
                              {
                                  ChangedCollection = new ObjectCollection<Entity>(items),
                                  DeletedCollection = new ObjectCollection<Entity>(deletedItems)
                              };

            var request = (HttpWebRequest)WebRequest
                .Create(uri)
                .SetCredential();
            request.Method = "POST";
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            var data = changes.ToXmlString().Replace("utf-16", "utf-8");
            var postBuffer = Encoding.GetEncoding(1252).GetBytes(data);
            request.ContentLength = postBuffer.Length;

            var postData = await request.GetRequestStreamAsync();
            postData.Write(postBuffer, 0, postBuffer.Length);
            postData.Close();

            var response = await request.GetResponseAsync();
            if (null == response)
                throw new InvalidOperationException("Cannot get the server data for " + uri);

            var responseStream = response.GetResponseStream();
            var result = await responseStream.DeserializeJsonAsync<RepositoryJsonData>();
            var so = new SubmitOperation { RowsAffected = result.__count };
            foreach (var tuple in result.__results)
            {
                
                var et = items.SingleOrDefault(t => t.WebId == tuple.__webid);
                if (null != et)
                {
                    var entityType = this.GetEntityType(et);
                    var p = entityType.GetProperties().Single(a => a.Name == entityType.Name + "Id");
                    p.SetValue(et, tuple.__id);
                    so.Add(et.WebId, tuple.__id);
                }
            }
            return so;
        }

        private Type GetEntityType(Entity item)
        {
            var type = item.GetType();
            var attr = type.GetCustomAttribute<EntityTypeAttribute>();
            if (null != attr) return attr.Type;
            return type;
        }
        public async Task<SubmitOperation> SubmitChanges(Entity item)
        {
            var items = new List<Entity> { item };
            return await this.SubmitChanges(items, new List<Entity>(), null);
        }
    }
}
