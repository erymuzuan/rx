namespace Bespoke.Sph.Domain
{
    public interface IWorkOrderExport
    {
        string GenerateWorkOrder(Maintenance maintenance, string filename);
    }
}

