using AutoMapper;
using Project.API.DTOs;
using Project.Core.Entities;

namespace Project.API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Video, VideoDTO>()
                  .ForMember(D => D.EmergencyService, O => O.MapFrom(S => S.EmergencyService.ServiceName))
                  .ForMember(D => D.User, O => O.MapFrom(S => S.User.USerName))
                  .ForMember(D => D.VideoUrl, O => O.MapFrom<PictureVideoUrlResolver>());

            CreateMap<UserUploadVideo, UploadVideoDTO>()
                   .ForMember(D => D.EmergencyService, O => O.MapFrom(S => S.EmergencyService.ServiceName))
                   .ForMember(D => D.User, O => O.MapFrom(S => S.User.USerName))
                   .ForMember(D => D.VideoUrl, O => O.MapFrom<UploadVideoUrlResolver>());
        }
    }
}
