using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FriendsApi.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FriendsApi.Repository.Mapping
{
    public class FriendshipMap : IEntityTypeConfiguration<Friendship>
    {
        public void Configure(EntityTypeBuilder<Friendship> builder)
        {
            builder.ToTable("friendship");
            builder.HasKey(x => new { x.PersonId, x.FriendId });

            builder.HasOne(x => x.PersonOrFriend)
            .WithMany(x => x.Friendships)
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.PersonOrFriend)
            .WithMany(x => x.Friendships)
            .HasForeignKey(x => x.FriendId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
