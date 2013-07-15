namespace Bespoke.SphCommercialSpaces.Domain
{
    public interface IWorkOrderExport
    {
        string GenerateWorkOrder(Maintenance maintenance, string filename);
    }
}

