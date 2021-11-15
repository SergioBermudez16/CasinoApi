using System;
using System.ComponentModel.DataAnnotations;

namespace Casino.Api.Entities
{
    public class RouletteBet
    {
		[Key]
		[Required]
		public Guid Id { get; set; }

		[Required]
		public Guid UserId { get; set; }

		[Required]
		public Guid RouletteId { get; set; }

		[Required]
		public DateTime CreatedAt { get; set; }

		[Required]
		public int BetType { get; set; }

		[Required]
		public string Bet { get; set; }

		[Required]
		public decimal Money { get; set; }

		[Required]
		public decimal MoneyRetrieved { get; set; }

		public RouletteBet()
		{
		}
	}
}
