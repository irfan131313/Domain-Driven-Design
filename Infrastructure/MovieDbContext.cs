using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Movie.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Infrastructure
{
    public class MovieDbContext : IdentityDbContext
    {
            public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
            {
            }
            public DbSet<TMovie> Movies { get; set; }
            public DbSet<AppUser> AppUsers { get; set; }
            public DbSet<AppRole> AppRoles { get; set; }


    }
}
