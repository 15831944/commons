namespace System.Web.Mvc
{
    public interface IClaimInfo
    {
    }

    public class ClaimInfo<T> : IClaimInfo
    {
        public T ID { get; set; }
    }
}