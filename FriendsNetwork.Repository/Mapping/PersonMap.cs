using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FriendsNetwork.Repository.Models;

namespace FriendsNetwork.Repository.Mapping
{
    public class PersonMap : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("person");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Firstname).IsRequired();
            builder.Property(x => x.Lastname).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Birthday).IsRequired();
            builder.Property(x => x.PhotoUrl).IsRequired();
            builder.Property(x => x.CountryId).IsRequired();
            builder.Property(x => x.StateId).IsRequired();

            builder.HasOne(x => x.State).WithMany().HasForeignKey(x => x.StateId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Country).WithMany().HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.Friendships).WithOne(x => x.PersonOrFriend).HasForeignKey(x => x.PersonId);
            builder.HasMany(x => x.Friendships).WithOne(x => x.PersonOrFriend).HasForeignKey(x => x.FriendId);
        }
    }
}
