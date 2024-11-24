using Microsoft.EntityFrameworkCore;
using VueAppMvc.Server.Data;

var builder = WebApplication.CreateBuilder(args);

/******IMPORTANT*********/
//If I deploye with the SQL servers added it I break the app beacuse is looking for the connection string
//that does not exist
// Add services to the container.
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING_VUEJS") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//This is used to update the PRD Db using Entity Framework
//var connectionString = Environment.GetEnvironmentVariable("PRD_CONNECTION_STRING") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();