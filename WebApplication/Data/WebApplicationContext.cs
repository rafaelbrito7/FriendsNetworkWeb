using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;
using FriendsNetwork.Repository.Models;

namespace WebApplication.Data
{
    public class WebApplicationContext : DbContext
    {
        public WebApplicationContext (DbContextOptions<WebApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Country> Country { get; set; }

        public DbSet<State> State { get; set; }

        public DbSet<Person> Person { get; set; }
    }
}
