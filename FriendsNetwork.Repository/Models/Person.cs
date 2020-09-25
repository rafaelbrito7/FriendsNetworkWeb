using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendsNetwork.Repository.Models
{
    public class Person
    {
        public Guid Id { get; set; }
        public Guid StateId { get; set; }
        public State State { get; set; }
        public Guid CountryId { get; set; }
        public Country Country { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime Birthday { get; set; }
        public List<Friendship> Friendships { get; set; }
    }
}
