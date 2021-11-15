using AutoMapper;
using Casino.Api.Entities;
using Casino.Api.Models;

namespace Casino.Api.Profiles
{
    public class CasinoProfile : Profile
    {
        public CasinoProfile()
        {
            CreateMap<Roulette, RouletteDto>();
            CreateMap<RouletteDto, Roulette>();

            CreateMap<RouletteBet, RouletteBetForCreation>();
            CreateMap<RouletteBetForCreation, RouletteBet>();

            CreateMap<RouletteBet, RouletteResultDto>();
            CreateMap<RouletteResultDto, RouletteBet>();
        }
    }
}
