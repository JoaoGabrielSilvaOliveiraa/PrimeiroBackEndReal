using beyondthegame.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar a leitura da configura��o do appsettings.json
//builder.Configuration.AddJsonFile("appsettings.json");

var connectionString = builder.Configuration.GetConnectionString("DataBase");

builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));

// Configurar autentica��o JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = key
    };
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

// Habilitar a autentica��o e autoriza��o
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
