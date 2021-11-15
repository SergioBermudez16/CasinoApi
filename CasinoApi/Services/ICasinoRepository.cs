using Casino.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Casino.Api.Services
{
    public interface ICasinoRepository
    {
        Task<Guid> AddRoulette();
        Task<ActionResult> AddRouletteBet(Guid userId, RouletteBetForCreation newRouletteBetCreation);
        Task<ActionResult<List<RouletteDto>>> GetAllRoulettes();
        Task<ActionResult<List<RouletteResultDto>>> GetRouletteResult(Guid rouletteId);
        Task<bool> InvalidRouletteBet(RouletteBetForCreation newRouletteBet);
        Task<ActionResult> OpenRoulette(Guid id);
        Task<bool> Save();
    }
}