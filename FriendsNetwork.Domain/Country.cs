using System;
using System.Collections.Generic;
using System.Text;

namespace FriendsNetwork.Domain
{
    public class Country
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual List<State> States { get; set; }
    }
}
