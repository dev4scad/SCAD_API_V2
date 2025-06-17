using Microsoft.OpenApi.Models;
using SCAD_API_V2.Application.Http;
using SCAD_API_V2.Application.Interfaces;
using SCAD_API_V2.Application.Mapping;
using SCAD_API_V2.Application.Services;
using SCAD_API_V2.Domain.Interfaces;
using SCAD_API_V2.Infrastructure;
using SCAD_API_V2.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "SCAD API", Version = "v2" });
});

builder.Services.Configure<ConnectionString>(
    builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddAutoMapper(typeof(ClienteProfile).Assembly);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentDatabase, CurrentDatabase>();

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ILicencaRepository, LicencaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IVinculoRepository, VinculoRepository>();

builder.Services.AddScoped<IClienteServices, ClienteServices>();
builder.Services.AddScoped<ILicencaServices, LicencaServices>();
builder.Services.AddScoped<IUsuarioServices, UsuarioServices>();
builder.Services.AddScoped<IVinculoServices, VinculoServices>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "SCAD API v2");
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
