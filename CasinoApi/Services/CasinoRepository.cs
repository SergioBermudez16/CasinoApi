using System.Linq;
using System;
using AutoMapper;
using System.Threading.Tasks;
using Casino.Api.Context;
using Microsoft.AspNetCore.Mvc;
using Casino.Api.Entities;
using Casino.Api.Models;
using Casino.Api.Consts;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Casino.Api.Enums;

namespace Casino.Api.Services
{
    public class CasinoRepository : ICasinoRepository
    {
        private readonly CasinoContext _context;
        private readonly IMapper _mapper;
        private readonly Random _random = new Random();

        public CasinoRepository(CasinoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> AddRoulette()
        {
            var newRoulette = new Roulette();
            await _context.Roulettes.AddAsync(newRoulette);
            if (!(await Save()))
                throw new Exception($"Adding new roullete failed on save.");

            return newRoulette.Id;
        }

        public async Task<ActionResult> AddRouletteBet(Guid userId, RouletteBetForCreation newRouletteBetCreation)
        {
            if (await InvalidRouletteBet(newRouletteBetCreation))
                return new BadRequestObjectResult("Invalid data for new roulette bet");
            var newRouletteBet = _mapper.Map<RouletteBet>(newRouletteBetCreation);
            newRouletteBet.UserId = userId;
            newRouletteBet.CreatedAt = DateTime.Now;
            await _context.RouletteBets.AddAsync(newRouletteBet);
            if (!(await Save()))
                throw new Exception($"Adding new roulette bet failed on save.");

            return new OkObjectResult(newRouletteBet.Id);
        }

        public decimal CalculateMoneyPrize(RouletteBet rouletteBet, int winnerNumber)
        {
            decimal moneyPrize = 0;
            switch ((RouletteBetTypes)rouletteBet.BetType)
            {
                case RouletteBetTypes.Number:
                    moneyPrize = int.Parse(rouletteBet.Bet) == winnerNumber ? rouletteBet.Money * (decimal)5.0 : 0;
                    break;

                case RouletteBetTypes.Colour:
                    var winnerColour = winnerNumber % 2 == 0 ? RouletteConsts.COLOUR_RED : RouletteConsts.COLOUR_BLACK;
                    moneyPrize = rouletteBet.Bet == winnerColour ? rouletteBet.Money * (decimal)1.8 : 0;
                    break;
            }

            return moneyPrize;
        }

        public async Task<ActionResult<List<RouletteDto>>> GetAllRoulettes()
        {
            var roulettes = new List<RouletteDto>();
            roulettes.AddRange(await _context.Roulettes.Select(r => _mapper.Map<RouletteDto>(r)).ToListAsync());

            return new OkObjectResult(roulettes);
        }

        public async Task<ActionResult<List<RouletteResultDto>>> GetRouletteResult(Guid rouletteId)
        {
            var updateRes = await UpdateRouletteBets(rouletteId);
            var objUpdateRes = updateRes as ObjectResult;
            if (objUpdateRes.StatusCode == 400)
                return updateRes;
            var rouletteResults = new List<RouletteResultDto>();
            rouletteResults.AddRange(await _context.RouletteBets.Select(rb => _mapper.Map<RouletteResultDto>(rb)).ToListAsync());

            return new OkObjectResult(rouletteResults);
        }

        public async Task<bool> InvalidRouletteBet(RouletteBetForCreation newRouletteBet)
        {
            var roulette = await _context.Roulettes.FindAsync(newRouletteBet.RouletteId);
            if (roulette == null)
                return true;
            if (!roulette.IsOpen)
                return true;
            if (!Enum.IsDefined(typeof(RouletteBetTypes), newRouletteBet.BetType))
                return true;
            switch (newRouletteBet.BetType)
            {
                case RouletteBetTypes.Number:
                    if (!int.TryParse(newRouletteBet.Bet, out var betNumber))
                        return true;
                    if (betNumber < RouletteConsts.MIN_ROULETTE_NUMBER || betNumber > RouletteConsts.MAX_ROULETTE_NUMBER)
                        return true;
                    break;

                case RouletteBetTypes.Colour:
                    newRouletteBet.Bet = newRouletteBet.Bet.ToUpper();
                    if (newRouletteBet.Bet != RouletteConsts.COLOUR_BLACK && newRouletteBet.Bet != RouletteConsts.COLOUR_RED)
                        return true;
                    break;
            }
            if (newRouletteBet.Money > RouletteConsts.MAX_BET_MONEY)
                return true;

            return false;
        }

        public async Task<ActionResult> OpenRoulette(Guid id)
        {
            var roulette = await _context.Roulettes.FindAsync(id);
            if (roulette == null)
                return new NotFoundResult();
            if (roulette.IsOpen)
                return new BadRequestResult();
            roulette.IsOpen = true;
            roulette.OpenAt = DateTime.Now;
            if (!(await Save()))
                throw new Exception($"Updating roulette {id} failed on save.");

            return new OkResult();
        }

        public async Task<bool> Save()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<ActionResult> UpdateRouletteBets(Guid rouletteId)
        {
            var winnerNumber = _random.Next(RouletteConsts.MIN_ROULETTE_NUMBER, RouletteConsts.MAX_ROULETTE_NUMBER);
            var roulette = await _context.Roulettes.FindAsync(rouletteId);
            if (roulette == null)
                return new BadRequestObjectResult("Roulette Id does not exist");
            roulette.IsOpen = false;
            roulette.ClosedAt = DateTime.Now;
            var rouletteBets = await _context.RouletteBets.Where(rb => rb.RouletteId == roulette.Id &&
                rb.CreatedAt > roulette.OpenAt && rb.CreatedAt < roulette.ClosedAt)
                .ToListAsync();
            rouletteBets.ForEach(rb => rb.MoneyRetrieved = CalculateMoneyPrize(rb, winnerNumber));
            if (!(await Save()))
                throw new Exception($"Updating roulette bets failed on save.");

            return new OkObjectResult("Roulette bets updated");
        }
    }
}