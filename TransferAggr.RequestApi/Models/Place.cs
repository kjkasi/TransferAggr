using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TransferAggr.RequestApi.Models
{
    public class Place
    {
        [Key]
        public string GUID { get; set; }
        public string Address { get; set; }
        public string Raion { get; set; }
        public string Housenumber { get; set; }
        public string Unit { get; set; }
        public string Building { get; set; }
        public string CityParsed { get; set; }
        public string StreetParsed { get; set; }
        public string lon { get; set; }
        public string lat { get; set; }
    }
}
