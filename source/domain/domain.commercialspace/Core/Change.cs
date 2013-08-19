
namespace Bespoke.SphCommercialSpaces.Domain
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
            return string.Format("Action : {0}\r\nField : {1}\r\nOldValue : {2}\r\nNewValue: {3}"
                , this.Action, this.PropertyName, this.OldValue, this.NewValue);
        }
    }
}
