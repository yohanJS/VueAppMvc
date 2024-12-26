using Microsoft.EntityFrameworkCore;
using VueAppMvc.Server.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//CONNECTION_STRING_VUEJS

string? localDbConnectionString = builder.Configuration.GetConnectionString("defaultConnectionString");
string? prdDbConnectionString = Environment.GetEnvironmentVariable("BOOKING_CONNECTION_STRING");

string? connectionString = string.IsNullOrEmpty(prdDbConnectionString) ? localDbConnectionString : prdDbConnectionString;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();

// Add authentication and authorization services
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "your-issuer",
            ValidAudience = "your-audience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key"))
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("https://localhost:54554", "http://engfuel.com")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Create roles when the app starts
/*Creates a scope to access services (e.g., RoleManager) for dependency injection.
Defines a list of roles (Admin, User).
Checks if each role exists in the database.
If a role doesn’t exist, it creates it using RoleManager.
*/
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors("AllowSpecificOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();  // Enable authentication middleware
app.UseAuthorization();   // Enable authorization middleware

app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
