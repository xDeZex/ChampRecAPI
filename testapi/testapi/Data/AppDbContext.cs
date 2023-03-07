using testapi.Models;
using Microsoft.EntityFrameworkCore;
namespace testapi.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options) {  }

    public DbSet<Summoner> Summoners { get; set; }
}
