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
    }
}
