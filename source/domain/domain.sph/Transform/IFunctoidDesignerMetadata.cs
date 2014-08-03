namespace Bespoke.Sph.Domain
{
    public interface IFunctoidDesignerMetadata
    {
        FunctoidCategory Category { get; }
        string Name { get; }
        string FontAwesomeIcon { get; }
        string BootstrapIcon { get; }
        string PngIcon { get;  }
    }

    public enum FunctoidCategory
    {
        Common,
        Date,
        String,
        Math,
        Database
    }
}