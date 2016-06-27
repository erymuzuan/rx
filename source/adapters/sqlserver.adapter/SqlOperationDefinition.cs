using System;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Integrations.Adapters
{
    public abstract class SqlOperationDefinition : OperationDefinition
    {
        public string ObjectType { get; set; }
        public abstract Task InitializeRequestMembersAsync(SqlServerAdapter adapter);
        protected override string GenerateAdapterActionBody(Adapter adapter)
        {
            throw new Exception("Should be implemented by one of the function or sprocs");
        }


        public override async Task RefreshMetadataAsync(Adapter adapter)
        {
            var clone = this.Clone();
            clone.ResponseMemberCollection.Clear();
            clone.RequestMemberCollection.Clear();
            await clone.InitializeRequestMembersAsync((SqlServerAdapter)adapter);
            //
            foreach (var m in clone.RequestMemberCollection.OfType<SqlColumn>())
            {
                var old = this.RequestMemberCollection.OfType<SqlColumn>().SingleOrDefault(x => x.Name == m.Name);
                if (null == old) continue;
                m.Merge(old);
            }
            this.RequestMemberCollection.ClearAndAddRange(clone.RequestMemberCollection.OfType<SqlColumn>().OrderBy(x => x.Order));


            // root level response
            foreach (var m in clone.ResponseMemberCollection.OfType<SqlColumn>())
            {
                var old = this.ResponseMemberCollection.OfType<SqlColumn>().SingleOrDefault(x => x.Name == m.Name);
                if (null == old) continue;
                m.Merge(old);
            }

            // reader
            var oldReader = this.ResponseMemberCollection.OfType<ComplexMember>().SingleOrDefault();
            var reader = clone.ResponseMemberCollection.OfType<ComplexMember>().SingleOrDefault();
            if (null == reader || null == oldReader) return;
            // 
            foreach (var m in reader.MemberCollection.OfType<SqlColumn>())
            {
                var old = oldReader.MemberCollection.OfType<SqlColumn>().SingleOrDefault(x => x.Name == m.Name);
                if (null == old) continue;
                m.Merge(old);
           
            }
            oldReader.MemberCollection.ClearAndAddRange(reader.MemberCollection.OfType<SqlColumn>().OrderBy(x => x.Order));
            this.ResponseMemberCollection.ClearAndAddRange(clone.ResponseMemberCollection.OfType<SqlColumn>().OrderBy(x => x.Order));
            this.ResponseMemberCollection.Add(oldReader);

        }
        public override string ToString()
        {
            return $"[{Schema}].[{Name}]";
        }
    }
}