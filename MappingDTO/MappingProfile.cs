using AutoMapper;
using WalletApi.Models;

namespace WalletApi.MappingDTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        { 
            CreateMap<Users, UsersDTO>().ReverseMap();
        }
    }
}
