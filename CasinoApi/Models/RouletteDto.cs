using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.Api.Models
{
    public class RouletteDto
    {
		public Guid Id { get; set; }

		public bool IsOpen { get; set; }

		public DateTime? OpenAt { get; set; }

		public DateTime? ClosedAt { get; set; }

		public RouletteDto()
		{
		}
	}
}
