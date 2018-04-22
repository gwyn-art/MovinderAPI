using AutoMapper;
using MovinderAPI.Dtos;
using MovinderAPI.Models;
 
namespace MovinderAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Invitaiton, InvitationDto>();
            CreateMap<InvitationDto, Invitaiton>();
        }
    }
}