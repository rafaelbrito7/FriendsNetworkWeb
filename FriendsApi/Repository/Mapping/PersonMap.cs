using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FriendsApi.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FriendsApi.Repository.Mapping
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
            builder.Property(x => x.CountryId).IsRequired();
            builder.Property(x => x.StateId).IsRequired();

            builder.HasOne(x => x.State);
            builder.HasOne(x => x.Country);

            builder.HasMany(x => x.Friendships).WithOne(x => x.PersonOrFriend).HasForeignKey(x => x.PersonId);
            builder.HasMany(x => x.Friendships).WithOne(x => x.PersonOrFriend).HasForeignKey(x => x.FriendId);
        }
    }
}
