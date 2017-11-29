using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bespoke.Sph.Domain
{
    [DebuggerDisplay("Name = {Name}")]
    [PersistenceOption(HasDerivedTypes = true, IsSource = true)]
    public partial class EntityDefinition : Entity, IProjectDefinitionWithMembers
    {
        // reserved names
        private readonly string[] m_reservedNames =
        {
            "JavascriptTest",
            "Management",
            "Image",
            "Home",
            "BaseSph",
            "Map",
            "Admin",
            "ActivityScreen",
            "BaseApp",
            "Config",
            "Nav",
            "RoleSettings",
            "ScreenEditor",
            "TriggerSetup",
            "Users",
            "WorkflowDraft",
            typeof(EntityDefinition).Name,
            typeof(AuditTrail).Name,
            typeof(BusinessRule).Name,
            typeof(BinaryStore).Name,
            typeof(SpatialEntity).Name,
            typeof(Entity).Name,
            typeof(Designation).Name,
            typeof(DocumentTemplate).Name,
            typeof(EmailAction).Name,
            typeof(EntityChart).Name,
            typeof(EntityDefinition).Name,
            typeof(EntityForm).Name,
            typeof(EntityView).Name,
            typeof(Message).Name,
            typeof(Organization).Name,
            typeof(ReportDefinition).Name,
            typeof(ReportDelivery).Name,
            typeof(SpatialStore).Name,
            typeof(Tracker).Name,
            typeof(Trigger).Name,
            typeof(UserProfile).Name,
            typeof(Watcher).Name,
            typeof(Workflow).Name,
            typeof(WorkflowDefinition).Name,
            typeof(EntityForm).Name,
            typeof(Message).Name
        };


        public override string ToString()
        {
            return this.Name;
        }

        public BuildValidationResult CanSave()
        {
            var result = new BuildValidationResult();
            var validName = new Regex(@"^[A-Za-z][A-Za-z0-9]*$");
            if (!validName.Match(this.Name).Success)
                result.Errors.Add(new BuildError(this.WebId)
                {
                    Message = "Name must start with letter.You cannot use symbol or number as first character"
                });
            if (string.IsNullOrWhiteSpace(this.Name))
                result.Errors.Add(new BuildError(this.WebId, "Name is missing"));
            if (m_reservedNames.Select(a => a.Trim().ToLowerInvariant()).Contains(this.Name.Trim().ToLowerInvariant()))
                result.Errors.Add(new BuildError(this.WebId, $"The name [{this.Name}] is reserved for the system"));


            result.Result = !result.Errors.Any();
            return result;
        }


        public static EntityDefinition operator +(EntityDefinition x, (string Name, Type Type) mb)
        {
            var simpleMember = new SimpleMember {Name = mb.Name, Type = mb.Type, WebId = Strings.GenerateId()};
            x.MemberCollection.Add(simpleMember);
            return x;
        }

        public static EntityDefinition operator -(EntityDefinition x, string name)
        {
            var member = x.MemberCollection.SingleOrDefault(v => v.Name == name);
            if (null != member)
                x.MemberCollection.Remove(member);
            return x;
        }

        public static EntityDefinition operator +(EntityDefinition x, SimpleMember mb)
        {
            x.MemberCollection.Add(mb);
            return x;
        }


        public Member GetMember(string path)
        {
            if (path == nameof(Id)) return new SimpleMember {Name = nameof(Id), Type = typeof(string)};
            if (path == nameof(CreatedDate))
                return new SimpleMember {Name = nameof(CreatedDate), Type = typeof(DateTime)};
            if (path == nameof(ChangedDate))
                return new SimpleMember {Name = nameof(ChangedDate), Type = typeof(DateTime)};
            if (path == nameof(CreatedBy)) return new SimpleMember {Name = nameof(CreatedBy), Type = typeof(string)};
            if (path == nameof(ChangedBy)) return new SimpleMember {Name = nameof(ChangedBy), Type = typeof(string)};

            if (!path.Contains("."))
            {
                var member = this.MemberCollection.SingleOrDefault(a => a.Name == path);
                if (null != member) return member;
                throw new InvalidOperationException($"Cannot find a member in \"{Name}\" with path :\"{path}\"");
            }

            var paths = path.Split(new[] {"."}, StringSplitOptions.RemoveEmptyEntries).ToList();
            var child = this.GetMember(paths.First());
            paths.RemoveAt(0);
            var nextPath = string.Join(".", paths);
            return this.GetMember(nextPath, child);
        }

        private Member GetMember(string path, Member member2)
        {
            if (!path.Contains("."))
            {
                if (member2 is ValueObjectMember vm)
                {
                    var members = vm.MemberCollection;
                    var rm1 = members.SingleOrDefault(a => a.Name == path);
                    if (null == rm1)
                        throw new ArgumentException($"Cannot find any {path} in {Name}.{member2.Name}", nameof(path));
                    return rm1;
                }
                var rm = member2.MemberCollection.SingleOrDefault(a => a.Name == path);
                if (null == rm)
                    throw new ArgumentException($"Cannot find any {path} in {Name}.{member2.Name}", nameof(path));
                return rm;
            }

            var paths = path.Split(new[] {"."}, StringSplitOptions.RemoveEmptyEntries).ToList();
            var child = this.GetMember(paths.First(), member2);
            paths.RemoveAt(0);
            var nextPath = string.Join(".", paths);
            return this.GetMember(nextPath, child);
        }

        public Member AddMember<T>(string name)
        {
            if (typeof(T).IsSubclassOf(typeof(Member)))
            {
                var ctor = typeof(T).GetConstructor(Array.Empty<Type>());
                if (null == ctor) throw new ArgumentException("No default ctor");
                if (ctor.Invoke(new object[] { }) is Member mbr)
                {
                    mbr.Name = name;
                    mbr.WebId = Strings.GenerateId();
                    this.MemberCollection.Add(mbr);
                    return mbr;
                }
            }
            var sm = new SimpleMember {Name = "Created", Type = typeof(T)};
            this.MemberCollection.Add(sm);
            return sm;
        }

    }
}