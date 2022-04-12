using AutoMapper;
using MiniBanking.API.Models;
using MiniBanking.Core.Models;
using MiniBanking.Domain.Entities;

namespace MiniBanking.API.Application.Mappings;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<User, UserRequestModel>().ReverseMap();
        CreateMap<User, UserResponseModel>().ReverseMap();
        CreateMap<PayStackResponseModel, TransferResponse>().ReverseMap();
        CreateMap<PayStackRequestModel, FundTransferRequest>().ReverseMap();
        CreateMap<Account, AccountResponse>().ReverseMap();
    }
}