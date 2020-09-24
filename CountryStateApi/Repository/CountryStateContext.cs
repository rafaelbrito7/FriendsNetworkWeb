using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CountryStateApi.Models;
using CountryStateApi.Repository.Mapping;
using Microsoft.EntityFrameworkCore;

namespace CountryStateApi.Repository
{
    public class CountryStateContext : DbContext
    {
        public DbSet<State> State { get; set; }
        public DbSet<Country> Country { get; set; }

        public CountryStateContext(DbContextOptions<CountryStateContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CountryMap());
            modelBuilder.ApplyConfiguration(new StateMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
}
