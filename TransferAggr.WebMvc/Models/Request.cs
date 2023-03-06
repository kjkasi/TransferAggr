using System.ComponentModel.DataAnnotations;

namespace TransferAggr.WebMvc.Models
{
    public class Request
    {
        [Key]
        public int RequestId { get; set; }
        public string? FromId { get; set; }
        public Place From { get; set; }
        public string? ToId { get; set; }
        public Place To { get; set; }
    }
}
