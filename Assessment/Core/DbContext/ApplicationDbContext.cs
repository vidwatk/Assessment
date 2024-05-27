using Assessment.Core.Entities;
using Assessment.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Core.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Drawing> Drawings { get; set; }
        public DbSet<Manual> Manuals { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Report> Reports { get; set; }
    }
}
