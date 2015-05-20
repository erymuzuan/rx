
namespace Bespoke.Sph.Domain
{
    public partial class Change : DomainObject
    {

        public Change()
        {
            if (string.IsNullOrEmpty(this.Action))
                this.Action = "Change";
        }

        public override string ToString()
        {
            return
                $"Action : {this.Action}\r\nField : {this.PropertyName}\r\nOldValue : {this.OldValue}\r\nNewValue: {this.NewValue}";
        }
    }
}
