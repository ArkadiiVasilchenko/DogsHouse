namespace DogsHouse.Domain.Models
{
    public class RequestLimiterOptions
    {
        public int MaxRequestsPerSecond { get; set; }
        public int ResetIntervalInSeconds { get; set; }
    }
}
