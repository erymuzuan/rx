using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Bespoke.Sph.WebApi
{
    public class CacheMetadata
    {
        public DateTime? LastModified { get; set; }
        public TimeSpan? MaxAge { get; set; }
        public bool Private { get; set; }
        public bool Public { get; set; }
        public bool NoCache { get; set; }
        public bool NoStore { get; set; }
        public string Etag { get; set; }

        public void SetMetadata(HttpResponseMessage response)
        {
            response.Headers.CacheControl = new CacheControlHeaderValue
            {
                MaxAge = this.MaxAge,
                Private = this.Private,
                Public = this.Public,
                NoCache = this.NoCache,
                NoStore = this.NoStore
            };
            if (!string.IsNullOrWhiteSpace(this.Etag))
                response.Headers.ETag = new EntityTagHeaderValue($"\"{this.Etag}\"", false);
            if (this.LastModified.HasValue)
                response.Content.Headers.LastModified = this.LastModified.Value;
            if (this.MaxAge.HasValue)
                response.Content.Headers.Expires = DateTime.Now.Add(this.MaxAge.Value);
        }
    }
}