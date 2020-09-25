using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FriendsNetwork.Repository.Models;

namespace FriendsNetwork.Repository.Mapping
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
            .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.PersonOrFriend)
            .WithMany(x => x.Friendships)
            .HasForeignKey(x => x.FriendId)
            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
