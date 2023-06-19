using AutoMapper;
using WDA.Api.Dto.User.Response;
using WDA.Domain.Models.User;

namespace WDA.Api.Configurations;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserInfoResponse>();
    }
}
