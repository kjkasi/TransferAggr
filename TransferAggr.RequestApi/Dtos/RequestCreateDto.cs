using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransferAggr.RequestApi.Dtos
{
    public class RequestCreateDto
    {
        public int FromId { get; set; }
        public int ToId { get; set; }
    }
}
