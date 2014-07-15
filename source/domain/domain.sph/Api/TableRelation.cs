namespace Bespoke.Sph.Domain.Api
{
    public class TableRelation
    {
        public string Table { get; set; }
        public string Constraint { get; set; }
        public string Column { get; set; }
        public string ForeignColumn { get; set; }
    }
}