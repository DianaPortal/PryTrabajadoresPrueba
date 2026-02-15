using Microsoft.EntityFrameworkCore;
using PryTrabajadoresPrueba.Application.Interfaces;
using PryTrabajadoresPrueba.Application.Services;
using PryTrabajadoresPrueba.Infrastructure.Data;

using PryTrabajadoresPrueba.Domain.Entities;
using PryTrabajadoresPrueba.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// *****************************************************************************************
//1. CONEXION A LA BASE DE DATOS - SQL SERVER
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//2. INYECCIÓN DE DEPENDENCIAS
//Repositrio
builder.Services.AddScoped<ITrabajadorRepository, TrabajadorRepository>();
//Servicio
builder.Services.AddScoped<ITrabajadorService, TrabajadorService>();

builder.Services.AddScoped<IFotoService, FotoService>();
// *****************************************************************************************

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Trabajadores}/{action=Index}/{id?}");

app.Run();
