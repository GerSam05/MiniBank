using AutoMapper;
using MiniBank_API.Models;
using MiniBank_API.Models.Dtos;
using System.Runtime;

namespace MiniBank_API
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {

            CreateMap<Client, ClientDto>().ReverseMap();
            CreateMap<Client, ClientCreateDto>().ReverseMap();
            CreateMap<Client, ClientUpdateDto>().ReverseMap();

            CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<Account, AccountCreateDto>().ReverseMap();
            CreateMap<Account, AccountUpdateDto>().ReverseMap();
        }
    }
}
