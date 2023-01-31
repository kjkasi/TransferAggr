using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferAggr.RequestApi.Models;

namespace TransferAggr.RequestApi.Dtos
{
    public class RequestReadDto
    {
        public int RequestId { get; set; }
        public int FromId { get; set; }
        public Place From { get; set; }
        public int ToId { get; set; }
        public Place To { get; set; }
    }
}
