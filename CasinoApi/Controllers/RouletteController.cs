using Casino.Api.Models;
using Casino.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Casino.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouletteController : ControllerBase
    {
        private readonly ICasinoRepository _repo;

        public RouletteController(ICasinoRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<RouletteDto>>> GetAllRoulettes()
        {
            return await _repo.GetAllRoulettes();
        }

        [HttpGet("{rouletteId}")]
        public async Task<ActionResult<List<RouletteResultDto>>> GetRouletteResult(Guid rouletteId)
        {
            return await _repo.GetRouletteResult(rouletteId);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoulette()
        {
            return Ok(await _repo.AddRoulette());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> OpenRoulette(Guid id)
        {
            return await _repo.OpenRoulette(id);
        }
    }
}
