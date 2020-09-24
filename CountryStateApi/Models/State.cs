using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryStateApi.Models
{
    public class State
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public Guid CountryId { get; set; }
        public Country Country { get; set; }
    }
}
