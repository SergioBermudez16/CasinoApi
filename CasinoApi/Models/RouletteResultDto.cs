using System;
using System.Text.Json.Serialization;

namespace Casino.Api.Models
{
    public class RouletteResultDto
    {
		[JsonPropertyName("userId")]
		public Guid UserId { get; set; }

		[JsonPropertyName("rouletteId")]
		public Guid RouletteId { get; set; }

		[JsonPropertyName("bet")]
		public string Bet { get; set; }

		[JsonPropertyName("money")]
		public decimal Money { get; set; }

		[JsonPropertyName("moneyRetrieved")]
		public decimal MoneyRetrieved { get; set; }

		public RouletteResultDto()
		{
		}
	}
}
