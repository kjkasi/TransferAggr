using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TransferAggr.RequestApi.Models
{
    public class Request
    {
        [Key]
        public int RequestId { get; set; }
        public int FromId { get; set; }
        public Place From { get; set; }
        public int ToId { get; set; }
        public Place To { get; set; }
    }
}
