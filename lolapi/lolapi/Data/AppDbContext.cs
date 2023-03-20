using lolapi.Models;
using Microsoft.EntityFrameworkCore;
namespace lolapi.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options) {  }

    public DbSet<Summoner> Summoners { get; set; }
}
