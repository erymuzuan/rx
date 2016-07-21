namespace Bespoke.Sph.Domain
{
    public partial class SetterActionChild : DomainObject
    {
        public string GenerateCode(EntityDefinition ed, string itemName, int count)
        {
            var member = ed.GetMember(this.Path);
            var sc = this.Field.GenerateCode();
            if (string.IsNullOrWhiteSpace(sc) || sc.Contains("NOT IMPLEMENTED"))
            {
                return $@"
            var setter{count} = endpoint.SetterActionChildCollection.Single(a => a.WebId == ""{this.WebId}"");
            item.{Path} = ({member.GetMemberTypeName()})setter{count}.Field.GetValue(rc);";

            }

            return member.GenerateInitializeValueCode(this.Field, itemName);
        }

    }
}