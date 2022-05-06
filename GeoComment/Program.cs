global using GeoComment.Models;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using GeoComment.AutoMapperProfiles;
using GeoComment.Services;
using GeoComment.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<GeoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

#region Own services

builder.Services.AddScoped<Database>();
builder.Services.AddScoped<GeoCommentService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<JwtManager>();
builder.Services.AddAutoMapper(typeof(CommentProfile));

#endregion

builder.Services.AddIdentityCore<GeoUser>()
    .AddEntityFrameworkStores<GeoDbContext>();


builder.Services.AddApiVersioning(options =>
{
    options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
});

#region Swagger

builder.Services.AddVersionedApiExplorer(options => { options.GroupNameFormat = "'v'VVV"; });

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("JwtAuth", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "Bearer Authorization header with JWT."
    });
    // skapa ett OpenAPI JSON objekt per versionsgrupp
    options.SwaggerDoc("v0.1", new OpenApiInfo()
    {
        Title = "Geo Comment API",
        Version = "ver 0.1"
    });
    options.SwaggerDoc("v0.2", new OpenApiInfo()
    {
        Title = "Geo Comment API",
        Version = "ver 0.2"
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
    options.OperationFilter<AddApiVersionExampleValueOperationFilter>();

    #region Enabling XML comments to get included in SwaggerUI

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    #endregion
});

#endregion


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = false,
            ValidateLifetime = true,
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var db = services.GetService<Database>();
    await db.RecreateDb();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v0.1/swagger.json", "v0.1");
        options.SwaggerEndpoint("/swagger/v0.2/swagger.json", "v0.2");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();