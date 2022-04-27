global using GeoComment.Models;
using GeoComment.AutoMapperProfiles;
using GeoComment.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<GeoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<Database>();
builder.Services.AddScoped<GeoCommentService>();
builder.Services.AddScoped<UserService>();


builder.Services.AddAutoMapper(typeof(CommentProfile));

builder.Services.AddApiVersioning(options =>
{
    options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
});
builder.Services.AddVersionedApiExplorer(options => { options.GroupNameFormat = "'v'VVV"; });

builder.Services.AddSwaggerGen(options =>
{
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
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var db = services.GetService<Database>();
    await db.RecreateDb();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // skapar en UI flik per OpenAPI JSON objekt
        options.SwaggerEndpoint("/swagger/v0.1/swagger.json", "v0.1");
        options.SwaggerEndpoint("/swagger/v0.2/swagger.json", "v0.2");
    });
}

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();