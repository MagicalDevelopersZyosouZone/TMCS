using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TMCS.Server.Models
{
    public class TMCSServerContext : DbContext
    {
        public TMCSServerContext (DbContextOptions<TMCSServerContext> options)
            : base(options)
        {
        }

        public DbSet<TMCS.Server.Models.User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>()
                .HasKey(c => new { c.SubjectId, c.ObjectId });
            modelBuilder.Entity<Contact>()
                .HasOne(c => c.Subject)
                .WithOne();
            modelBuilder.Entity<Contact>()
                .HasOne(c => c.Object)
                .WithMany(u => u.Contacts)
                .HasForeignKey(c => c.ObjectId);
        }
    }
}
