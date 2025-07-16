using Microsoft.EntityFrameworkCore;
using TesteConciliadora.Infrastructure.Data;
using TesteConciliadora.Infrastructure.Repositories;

namespace TesteConciliadora;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<EstacionamentoDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // repositorios 
        builder.Services.AddScoped<ClienteRepository>();
        builder.Services.AddScoped<VeiculoRepository>();
        builder.Services.AddScoped<MensalistaRepository>();

        // // referencia circular 
        // builder.Services.AddControllers()
        //     .AddJsonOptions(options =>
        //     {
        //         options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        //     });
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (true) // ativado para facilitar o teste  => app.Environment.IsDevelopment()
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }


        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        
        app.Run();
    }
}