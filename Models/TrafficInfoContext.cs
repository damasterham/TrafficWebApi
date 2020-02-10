using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafikWebAPI.Models
{
    // Our data context where we can define the collections od data we want to persist,
    // their references and the like
    public class TrafficInfoContext : DbContext
    {
        public DbSet<LineInfo> LineInfoEntries { get; set; }

        public TrafficInfoContext(DbContextOptions<TrafficInfoContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Initialze some seed data
            LineInfo[] seedData = new LineInfo[20];

            for (int i = 0; i < seedData.Length; i++)
            {
                seedData[i] = new LineInfo
                {
                    Id = i + 1,
                    Line = string.Format("{0}A", ((i+1) / 2) % 2 + 1 ),
                    Time = new DateTime(2020, 2, 7, 10, 26 + i, 56),
                    Message = string.Format("Message {0}", i)
                };
            }

            modelBuilder.Entity<LineInfo>().HasData(seedData);

            //base.OnModelCreating(modelBuilder);
        }
    }
}
