namespace TransferAggr.WebMvc.Models
{
    public class RequestViewModel
    {
        public int RequestId { get; set; }
        public Place From { get; set; }
        public Place To { get; set; }
    }
}
