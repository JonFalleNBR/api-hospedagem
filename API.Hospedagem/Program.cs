using API.Hospedagem.Data;
using Microsoft.EntityFrameworkCore;
using API.Hospedagem.Models;
using API.Hospedagem.Services.Implementations;
using API.Hospedagem.Services.Interfaces;
using AutoMapper;
using API.Hospedagem.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IHospedeService, HospedeService>();
builder.Services.AddScoped<IQuartoService, QuartoService>();
builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddScoped<IFuncionarioService, FuncionarioService>();
builder.Services.AddScoped<ICargoService, CargoService>();
builder.Services.AddCors(
        o => {
            o.AddPolicy("AllowAngular",
                        p => p.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader());



                });
        
    
  

    

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configura��o do DbContext com retry logic - SQL Server - relevante para conex�es inst�veis e docker / compose
builder.Services.AddDbContext<ApplicationDbContext>(opts =>
{
    var conn = Environment.GetEnvironmentVariable("DB_CONNECTION")
               ?? builder.Configuration.GetConnectionString("DefaultConnection");
    opts.UseSqlServer(conn, sql =>
    {
        sql.EnableRetryOnFailure(
            maxRetryCount: 10,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null);
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAngular");


app.Run();






/*

 * Microsoft.EntityFrameworkCore.SqlServer / Tools

AutoMapper.Extensions.Microsoft.DependencyInjection

Swashbuckle.AspNetCore (Swagger)
 * 
 */