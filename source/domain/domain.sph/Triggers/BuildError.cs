namespace Bespoke.Sph.Domain
{
    public class BuildError
    {
        public string Message { get; set; }
        public int Line { get; set; }

        public override string ToString()
        {
            return this.Message;
        }
    }
}