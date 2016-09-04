using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    public class LocationEndpointDiagnostics : BuilDiagnostic
    {
        public override async Task<BuildError[]> ValidateErrorsAsync(ReceiveLocation location)
        {
            var errors = new List<BuildError>(await base.ValidateErrorsAsync(location));
            if (string.IsNullOrWhiteSpace(location.SubmitEndpoint))
                errors.Add(new BuildError(location.Id, $"No submit endpoint is registered for {location.Name}"));
            if (string.IsNullOrWhiteSpace(location.SubmitMethod))
                errors.Add(new BuildError(location.Id, $"No submit METHOD is registered for {location.Name}"));
            return errors.ToArray();
        }
    }
    [Export(typeof(IBuildDiagnostics))]
    public class FileDropLocationDiagnostics : BuilDiagnostic
    {
        public override async Task<BuildError[]> ValidateErrorsAsync(ReceiveLocation location)
        {
           
            var errors = new List<BuildError>(await base.ValidateErrorsAsync(location));
            var folder = location as FolderReceiveLocation;
            if (null == folder) return errors.ToArray();

            if (string.IsNullOrWhiteSpace(folder.Filter))
                errors.Add(new BuildError(location.Id, $"No file filter is registered for {folder.Name}"));
            if (!Directory.Exists(folder.Path))
                errors.Add(new BuildError(location.Id, $"Cannot verify path '{folder.Path}'"));
            return errors.ToArray();
        }
    }
}