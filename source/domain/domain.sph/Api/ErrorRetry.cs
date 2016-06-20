namespace Bespoke.Sph.Domain.Api
{
    public partial class ErrorRetry : DomainObject
    {
        public string GenerateWaitCode()
        {
            var wait = this.Wait > 200 ? this.Wait.ToString() : "500";
            if (this.Algorithm == WaitAlgorithm.Exponential)
                wait = $"{wait} * Math.Pow(2, c)";
            if (this.Algorithm == WaitAlgorithm.Linear)
                wait = $"{wait} * c";
            return $"{this.Attempt}, c => TimeSpan.FromMilliseconds({wait})";
        }
    }
    
}