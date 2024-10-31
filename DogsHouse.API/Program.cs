using DogsHouse.API.Extensions;
using DogsHouse.API.Middlewares;
using DogsHouse.Application.Filters;
using DogsHouse.Domain.Models;
using DogsHouse.Persistence.Data;
using Microsoft.EntityFrameworkCore;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddLogging();

        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ControllerExceptionFilter>();
        });
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddCustomServices();

        builder.Services.AddCors();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.Configure<RequestLimiterOptions>(builder.Configuration.GetSection("RequestLimiter"));
        builder.Services.AddDbContext<AppDbContext>(b =>
        {
            b.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        builder.Services.AddSingleton<RequestLimiterMiddleware>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors(x => x
             .AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader());

        app.UseRouting();

        app.UseMiddleware<RequestLimiterMiddleware>();

        app.MapControllers();
        app.Run();
    }
}