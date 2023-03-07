using System.ComponentModel.DataAnnotations;
using testapi.Services.Summoners;
using testapi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddScoped<ISummonerService, SummonerService>();
}

var app = builder.Build();
{
    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
    app.UseExceptionHandler("/error");
}



