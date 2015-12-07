using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SourceBuilders
{
    internal class DesignationBuilder : Builder<Designation>
    {
        public override async Task RestoreAsync(Designation item)
        {

            var starts = from r in item.RoleCollection.Distinct()
                         select new ProcessStartInfo
                         {
                             CreateNoWindow = true,
                             UseShellExecute = true,
                             WindowStyle = ProcessWindowStyle.Hidden,
                             FileName = $"{ConfigurationManager.Home}\\utils\\mru.exe",
                             Arguments = $"-r \"{r}\" -c \"{ConfigurationManager.WebPath}\\web.config\""
                         };
            starts.ToList().ForEach(i => Process.Start(i));

            //update user roles
            var context = new SphDataContext();
            var lo = await context.LoadAsync(context.UserProfiles.Where(x => x.Designation == item.Name));
            var users = lo.ItemCollection;
            while (lo.HasNextPage)
            {
                lo = await context.LoadAsync(context.UserProfiles.Where(x => x.Designation == item.Name), lo.CurrentPage + 1);
                users.AddRange(lo.ItemCollection);
            }
            foreach (var u in users)
            {
                var starts2 = from r in item.RoleCollection.Distinct()
                              select new ProcessStartInfo
                              {
                                  CreateNoWindow = true,
                                  UseShellExecute = true,
                                  WindowStyle = ProcessWindowStyle.Hidden,
                                  FileName = $"{ConfigurationManager.Home}\\utils\\mru.exe",
                                  Arguments = $"-r \"{r}\" -u {u.UserName} -c \"{ConfigurationManager.WebPath}\\web.config\""
                              };
                starts2.ToList().ForEach(i => Process.Start(i));

            }

        }
    }
}