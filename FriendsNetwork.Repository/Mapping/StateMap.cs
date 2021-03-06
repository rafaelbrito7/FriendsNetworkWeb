﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FriendsNetwork.Repository.Models;

namespace FriendsNetwork.Repository.Mapping
{
    public class StateMap : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.ToTable("state");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.PhotoUrl).IsRequired();
            builder.Property(x => x.CountryId).IsRequired();
            builder.HasOne(x => x.Country);
        }
    }
}
