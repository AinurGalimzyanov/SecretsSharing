using AutoMapper;
using Dal.Files.Entity;
using SecretsSharing.Controllers.Files.Dto.Response;

namespace SecretsSharing.Controllers.Files.Mapping;

public class FileResponseProfile : Profile
{
    public FileResponseProfile()
    {
        CreateMap<FilesDal, FileModelResponse>()
            .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
            ;
    }
}