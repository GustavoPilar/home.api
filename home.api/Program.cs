using home.api.Application.Entities;
using home.api.Application.Entities.DTOs;
using home.api.Application.Interfaces;
using home.api.Application.Mappers;
using home.api.Application.Services;
using home.api.Domain.Entities;
using home.api.Infra.Db;
using home.api.Infra.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// A aplicação não sobe sem a chave de assinatura: falhar aqui é melhor do que emitir token inválido
string secretKey = builder.Configuration["JWT:SecretKey"]
    ?? throw new InvalidOperationException("A configuração JWT:SecretKey não foi definida.");

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseOracle(builder.Configuration.GetConnectionString("Oracle"));
});

builder.Services.AddIdentityCore<User>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8;
})
    .AddSignInManager()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));

#region Dependency Injection

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<UnitOfWork>();

// Mapeadores: mantêm a conversão DTO/entidade fora dos serviços
builder.Services.AddScoped<IUserMapper, UserMapper>();
builder.Services.AddScoped<IMapperBase<Home, HomeRequest, HomeUpdate, HomeResponse>, HomeMapper>();
builder.Services.AddScoped<IFamilyMapper, FamilyMapper>();

// Serviços
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IFamilyService, FamilyService>();

#endregion

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
