using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Casino.Api.Entities;

namespace Casino.Api.Context
{
    public class CasinoContext : DbContext
    {
        private readonly IConfiguration Config;

        public CasinoContext(DbContextOptions<CasinoContext> options, IConfiguration config) : base(options)
        {
            Config = config;
        }

        public async Task CommitAsync()
        {
            await SaveChangesAsync().ConfigureAwait(false);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Config.GetValue<string>("SchemaName"));

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Roulette> Roulettes { get; set; }
        public DbSet<RouletteBet> RouletteBets { get; set; }
    }
}
