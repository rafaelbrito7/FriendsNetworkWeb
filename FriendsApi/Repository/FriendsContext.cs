using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FriendsApi.Models;
using FriendsApi.Repository.Mapping;

namespace FriendsApi.Repository
{
    public class FriendsContext : DbContext
    {
        public DbSet<Person> People { get; set; }

        public FriendsContext(DbContextOptions<FriendsContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonMap());
            modelBuilder.ApplyConfiguration(new FriendshipMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
