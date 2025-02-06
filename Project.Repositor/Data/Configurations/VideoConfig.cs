using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Repositor.Data.Configurations
{
    public class VideoConfig : IEntityTypeConfiguration<Video>
    {
        public void Configure(EntityTypeBuilder<Video> builder)
        {
            builder.HasOne(V => V.EmergencyService)
                  .WithMany()
                  .HasForeignKey(V => V.EmergencyServiceId);

            builder.HasOne(U => U.User)
                   .WithMany()
                   .HasForeignKey(U => U.UserId);
        }
    }
}
