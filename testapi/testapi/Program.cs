using System.ComponentModel.DataAnnotations;
using testapi.Services.Summoners;
using testapi.Data;
using Microsoft.EntityFrameworkCore;


var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddScoped<ISummonerService, SummonerService>();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: MyAllowSpecificOrigins,
                        policy  =>
                        {
                            policy.WithOrigins("https://xdezex.github.io", "http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
                        });
    });
    builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
}

var app = builder.Build();
{
    app.UseCors(MyAllowSpecificOrigins);
    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
    app.UseExceptionHandler("/error");
}



