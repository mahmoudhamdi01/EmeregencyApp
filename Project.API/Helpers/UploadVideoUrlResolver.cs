using AutoMapper;
using Project.API.DTOs;
using Project.Core.Entities;

namespace Project.API.Helpers
{
    public class UploadVideoUrlResolver : IValueResolver<UserUploadVideo, UploadVideoDTO, string>
    {
        private readonly IConfiguration _configuration;

        public UploadVideoUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(UserUploadVideo source, UploadVideoDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.VideoUrl))
            {
                return $"{_configuration["APIVideoUrl"]}{source.VideoUrl}";
            }
            return string.Empty;
        }
    }
}
