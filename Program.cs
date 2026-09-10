using System;
using Microsoft.EntityFrameworkCore;
using Test_Task_URL_shortener_Backend.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<URLShortenerContext>(options => {
    options.UseNpgsql(connectionString);
    options.LogTo(Console.WriteLine);
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
