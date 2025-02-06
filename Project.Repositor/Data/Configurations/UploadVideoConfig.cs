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
    public class UploadVideoConfig : IEntityTypeConfiguration<UserUploadVideo>
    {
        public void Configure(EntityTypeBuilder<UserUploadVideo> builder)
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
