using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FriendsNetwork.Repository.Mapping;
using FriendsNetwork.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace FriendsNetwork.Repository
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<Friendship> Friendship { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FriendsNetworkAT;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonMap());
            modelBuilder.ApplyConfiguration(new CountryMap());
            modelBuilder.ApplyConfiguration(new StateMap());
            modelBuilder.ApplyConfiguration(new FriendshipMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
