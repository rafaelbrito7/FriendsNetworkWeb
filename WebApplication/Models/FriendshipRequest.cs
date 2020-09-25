using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class FriendshipRequest
    {
        public Guid PersonId { get; set; }
        public Guid FriendId { get; set; }

        public FriendshipRequest(Guid _PersonId, Guid _FriendId)
        {
            PersonId = _PersonId;
            FriendId = _FriendId;
        }
    }
}
