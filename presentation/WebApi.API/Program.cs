
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.Chats.Commands;
using WebApi.Application.Interfaces;
using WebApi.Persistence.Data;
using WebApi.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173") 
            .AllowAnyMethod()   // GET, POST, PUT, DELETE kimi bütün sorğulara icazə ver
            .AllowAnyHeader()  // Bütün HTTP başlıqlarına (Headers) icazə ver
            .AllowCredentials(); // SignalR (Websocket) bağlantıları üçün bu mütləq lazımdır!
    });
});

builder.Services.AddSwaggerGen();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAppDbContext, AppDbContext>();

// Repository qeydiyyatı
builder.Services.AddScoped<IChatRepository, ChatRepository>();

// MediatR qeydiyyatı (Application layihəsindəki sinifləri tapmaq üçün)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SendMessageCommand).Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.MapHub<WebApi.Application.Hubs.ChatHub>("/chathub");

app.Run();