using Casino.Api.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Casino.Api.Models
{
    public class RouletteBetForCreation
    {
		[Required]
		public Guid RouletteId { get; set; }

		public RouletteBetTypes BetType { get; set; }

		public string Bet { get; set; }

		public decimal Money { get; set; }

		public RouletteBetForCreation()
		{
		}
	}
}
