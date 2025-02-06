using Microsoft.EntityFrameworkCore;
using Project.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Project.Repositor.Data
{
    public class EmergencyContext : DbContext 
    {
        public EmergencyContext(DbContextOptions<EmergencyContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmergencyServices>()
                        .HasKey(e => e.ServiceId);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<User> users { get; set; }
        public DbSet<EmergencyServices> emergencyServices {  get; set; }    
        public DbSet<Video> videos { get; set; }
        public DbSet<UserUploadVideo> uploadVideos { get; set; }
    }
}
