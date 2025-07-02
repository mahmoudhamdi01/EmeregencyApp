using Project.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project.Repositor.Data
{
    public static class EmergencySeed
    {
        public static async Task SeedAsync(EmergencyContext DbContext)
        {
       
            // Seeding ServicesData
            if (!DbContext.emergencyServices.Any())
            {
                var ServicesData = File.ReadAllText("../Project.Repositor/Data/DataSeed/EmergencyService.json");
                var Services = JsonSerializer.Deserialize<List<EmergencyServices>>(ServicesData);
                if (Services?.Count > 0)
                {
                    foreach (var service in Services)
                    {
                        await DbContext.Set<EmergencyServices>().AddAsync(service);
                    }
                    await DbContext.SaveChangesAsync();
                }
            }

            // Seeding VideosData
            if (!DbContext.videos.Any())
            {
                var VideosData = File.ReadAllText("../Project.Repositor/Data/DataSeed/Video.json");
                var Videos = JsonSerializer.Deserialize<List<Video>>(VideosData);
                if (Videos?.Count > 0)
                {
                    foreach (var video in Videos)
                    {
                        await DbContext.Set<Video>().AddAsync(video);
                    }
                    await DbContext.SaveChangesAsync();
                }
            }


            // Seeding UploadVideosData
            if (!DbContext.uploadVideos.Any())
            {
                var UploadVideosData = File.ReadAllText("../Project.Repositor/Data/DataSeed/UserUploadVideo.json");
                var UploadVideos = JsonSerializer.Deserialize<List<UserUploadVideo>>(UploadVideosData);
                if (UploadVideos?.Count > 0)
                {
                    foreach (var video in UploadVideos)
                    {
                        await DbContext.Set<UserUploadVideo>().AddAsync(video);
                    }
                    await DbContext.SaveChangesAsync();
                }
            }
        }
    }
}
