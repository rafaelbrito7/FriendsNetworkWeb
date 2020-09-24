using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryStateApi.Models
{
    public class Country
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public List<State> States { get; set; }
    }
}

