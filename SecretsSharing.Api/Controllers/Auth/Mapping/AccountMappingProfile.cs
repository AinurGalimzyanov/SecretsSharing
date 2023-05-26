using AutoMapper;
using Dal.User.Entity;
using SecretsSharing.Controllers.Auth.Dto.Request;

namespace SecretsSharing.Controllers.Auth.Mapping;

public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        CreateMap<RegisterAndSignInModelRequest, UserDal>()
            .ForMember(dst => dst.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email));
    }
};