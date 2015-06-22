using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    partial class Performer : DomainObject
    {
        public override bool Validate()
        {
            if (this.IsPublic) return true;
            if (string.IsNullOrWhiteSpace(this.UserProperty)) return false;
            if (string.IsNullOrWhiteSpace(this.Value)) return false;

            return true;
        }

        public async Task<string[]> GetUsersAsync<T>(T item)
        {
            if (!this.Validate())
                throw new InvalidOperationException("This performer is not validated");

            if (this.IsPublic)
                return new[] { "*" };

            var script = ObjectBuilder.GetObject<IScriptEngine>();
            var context = new SphDataContext();
            var ad = ObjectBuilder.GetObject<IDirectoryService>();

            var unwrapValue = this.Value;
            if (!string.IsNullOrWhiteSpace(unwrapValue) && unwrapValue.StartsWith("="))
                unwrapValue = script.Evaluate<string, T>(unwrapValue.Remove(0, 1), item);

            var users = new List<string>();
            switch (this.UserProperty)
            {
                case "UserName":
                    users.Add(unwrapValue);
                    break;
                case "Department":
                    var list = await context.GetListAsync<UserProfile, string>(
                        u => u.Department == unwrapValue,
                        u => u.UserName);
                    users.AddRange(list);
                    break;
                case "Designation":
                    var list2 = await context.GetListAsync<UserProfile, string>(
                        u => u.Designation == unwrapValue,
                        u => u.UserName);
                    users.AddRange(list2);
                    break;
                case "Roles":
                    try
                    {
                        var list3 = await ad.GetUserInRolesAsync(unwrapValue);
                        users.AddRange(list3);
                    }
                    catch (InvalidOperationException e)
                    {
                        ObjectBuilder.GetObject<ILogger>().Log(new LogEntry(e));
                    }
                    break;
                default:
                    throw new Exception("Whoaaa we cannot user for " + this.UserProperty + " : " + this.Value);
            }
            return users.ToArray();
        }

    }
}
