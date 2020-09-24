using System;
using System.Collections.Generic;

namespace FriendsNetwork.Domain
{
    public class Friend
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        public Country Country { get; set; }
        public State State { get; set; }
        public virtual List<Friend> Friends { get; set; }
    }
}
