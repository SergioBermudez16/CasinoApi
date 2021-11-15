using Casino.Api.Models;
using Casino.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Casino.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouletteBetController : ControllerBase
    {
        private readonly ICasinoRepository _repo;

        public RouletteBetController(ICasinoRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> AddRouletteBet([FromHeader] Guid userId, [FromBody] RouletteBetForCreation newRouletteBet)
        {
            return await _repo.AddRouletteBet(userId,newRouletteBet);
        }
    }
}
