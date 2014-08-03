namespace Bespoke.Sph.Domain
{
    public class SourceFunctoid : Functoid
    {
        private string m_field;

        public string Field
        {
            get { return m_field; }
            set
            {
                m_field = value;
                RaisePropertyChanged();
            }
        }
        public override string GenerateCode()
        {
            return "item." + this.Field;
        }
    }
}