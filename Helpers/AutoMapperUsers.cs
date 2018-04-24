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

            CreateMap<Invitation, InvitationDto>();
            CreateMap<InvitationDto, Invitation>();

            CreateMap<RespondDto, Respond>();
            CreateMap<Respond, RespondDto>();
        }
    }
}