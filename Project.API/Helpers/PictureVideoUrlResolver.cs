using AutoMapper;
using AutoMapper.Execution;
using Project.API.DTOs;
using Project.Core.Entities;

namespace Project.API.Helpers
{
    public class PictureVideoUrlResolver : IValueResolver<Video, VideoDTO, string>
    {
        private readonly IConfiguration _configuration;

        public PictureVideoUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Video source, VideoDTO destination, string destMember, ResolutionContext context)
        {
            
            if (!string.IsNullOrEmpty(source.VideoUrl))
            {
                return $"{_configuration["APIVideoUrl"]}{source.VideoUrl}";
            }
            return string.Empty;
        }
    }
}
