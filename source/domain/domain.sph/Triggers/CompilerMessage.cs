namespace Bespoke.Sph.Domain
{
    public class CompilerMessage
    {
        public bool IsError { get; set; }
        public object Error { get; set; }
        public string Text{ get; set; }
        public Activity Activity { get; set; }
    }
}