using System;
using System.Globalization;
using System.Reflection;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Mangements
{
    public class MemberChange
    {
        public MemberChange()
        {
            
        }
        public MemberChange(Member mbr, Member existing, string parent, string oldParent)
        {
            this.WebId = mbr.WebId;
            this.Name = mbr.Name;
            NewPath = $"{parent}.{mbr.Name}";
            if (null == existing)
            {
                Action = "Added";
                OldPath = null;
                IsEmpty = false;
                return;
            }

            var oldType = existing.GetMemberTypeName();
            var type = mbr.GetMemberTypeName();
            if (existing.Name != mbr.Name && null != oldType && null != type && oldType == type)
            {
                Action = "NameChanged";
                OldPath = $"{oldParent}.{existing.Name}";
                return;
            }

            // TODO : complex member , named changed, now all it's children has got to change
            if (existing.Name != mbr.Name && null == oldType && null == type)
            {
                Action = "NameChanged";
                OldPath = $"{oldParent}.{existing.Name}";
                return;
            }
            if ((null != oldType || null != type) && oldType != type)
            {
                Action = "TypeChanged";
                return;
            }
            IsEmpty = true;
        }
        public string WebId { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public string NewPath { get; set; }
        public string OldPath { get; set; }
        public string OldType { get; set; }
        public string NewType { get; set; }
        public bool IsEmpty { get; }
        public MemberMigrationStrategies MigrationStrategy { get; set; }

        public void Migrate(dynamic item, JObject json)
        {
            if (string.IsNullOrWhiteSpace(this.OldPath)) return;
            var oldValue = json.SelectToken(this.OldPath).Value<dynamic>();
            var names = this.NewPath.Replace("$.", "").Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            Type type = item.GetType();
            PropertyInfo prop = null;
            var target = item;

            foreach (var name in names)
            {
                prop = type.GetProperty(name);
                if (null == prop)
                    throw new InvalidOperationException("Cannot read the property type for " + name);
                type = prop.PropertyType;

                if (type.Namespace.StartsWith("Bespoke"))
                {
                    target = prop.GetValue(target);
                }
            }
            try
            {
                if (null != prop && typeof(JValue) == oldValue.GetType())
                    prop.SetValue(target, oldValue.Value);

            }
            catch (Exception e)
            {
                Console.WriteLine("oldValue " + oldValue.ToString(CultureInfo.InvariantCulture));
                Console.WriteLine(e.Message);
            }
        }


        public override string ToString()
        {
            if (IsEmpty) return string.Empty;
            return $@"
{Action}: {WebId}
------------------------
{OldPath} -> {NewPath}
{OldType} -> {NewType}";
        }
    }

    public enum MemberMigrationStrategies
    {
        Direct,
        Script,
        Ignore
    }
}