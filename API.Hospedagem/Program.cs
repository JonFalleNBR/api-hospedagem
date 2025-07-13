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
//builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<ApplicationDbContext>(opts =>
    opts.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")

    )
);


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

app.Run();






/*

 * Microsoft.EntityFrameworkCore.SqlServer / Tools

AutoMapper.Extensions.Microsoft.DependencyInjection

Swashbuckle.AspNetCore (Swagger)
 * 
 */