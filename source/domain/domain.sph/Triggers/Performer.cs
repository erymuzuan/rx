using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    partial class Performer : DomainObject
    {

        public async Task<string[]> GetUsersAsync<T>(T item)
        {
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
                    var list3 = await ad.GetUserInRolesAsync(unwrapValue);
                    users.AddRange(list3);
                    break;
                default:
                    throw new Exception("Whoaaa we cannot send invitation to " + this.UserProperty);
            }
            return users.ToArray();
        }

    }
}
