using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransferAggr.RequestApi.Models
{
    public class Request
    {
        public int RequestId { get; set; }
        public int FromId { get; set; }
        public Place From { get; set; }
        public int ToId { get; set; }
        public Place To { get; set; }
    }
}
