using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendsNetwork.Repository.Models
{
    public class Friendship
    {
        public Guid PersonId { get; set; }
        public Person PersonOrFriend { get; set; }
        public Guid FriendId { get; set; }
    }
}
