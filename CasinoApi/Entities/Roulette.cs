using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.Api.Entities
{
    public class Roulette
    {
		[Key]
		[Required]
		public Guid Id { get; set; }

		[Required]
		public bool IsOpen { get; set; }

		[Required]
		public DateTime OpenAt { get; set; }

		[Required]
		public DateTime ClosedAt { get; set; }

		public Roulette()
		{
		}
	}
}
