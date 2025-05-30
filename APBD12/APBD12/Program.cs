using APBD12.Data;
using APBD12.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<ITripService,TripService>();
builder.Services.AddScoped<IClientService, ClientService>();

builder.Services.AddDbContext<Apbd12Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();
