using Asp.Versioning;
using CitiesManager.Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers(options =>
{
  options.Filters.Add(new ProducesAttribute("application/json"));
  options.Filters.Add(new ConsumesAttribute("application/json"));
})
  .AddXmlSerializerFormatters();


builder.Services.AddApiVersioning(config =>
{
  config.DefaultApiVersion = new ApiVersion(1, 0);
  config.AssumeDefaultVersionWhenUnspecified = true;
  config.ReportApiVersions = true;

  config.ApiVersionReader = new UrlSegmentApiVersionReader();
  //Read version from request url at "apiVersion" constraint

  //config.ApiVersionReader = new QueryStringApiVersionReader();
  //Read version form request query string called "api-version"  eg: api-version=1.0

  //config.ApiVersionReader = new HeaderApiVersionReader("api-version");
  //Read version number from request header called "api-version". eg: api-version: 1.0
});

//swagger
builder.Services.AddEndpointsApiExplorer(); //Generates description for all endpoints
builder.Services.AddSwaggerGen(options =>
{
  options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));

  options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo() { Title = "Custom API Name", Version = "1.0" });

  options.SwaggerDoc("v2", new Microsoft.OpenApi.OpenApiInfo() { Title = "Custom API Name", Version = "2.0" });
});

builder.Services.AddApiVersioning()
  .AddApiExplorer(options =>
  {
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
  });

builder.Services.AddCors(options =>
{
  options.AddDefaultPolicy(policyBuilder =>
  {
    //policyBuilder.WithOrigins("http://localhost:4200");

    var allowedOrigins = builder.Configuration
            .GetSection("AllowedOrigins")
            .Get<string[]>();

    policyBuilder.WithOrigins(allowedOrigins);

    policyBuilder.WithHeaders("Authorization", "origin", "accept", "content-type")
    .WithMethods("GET", "POST", "PUT", "DELETE");
  });

  options.AddPolicy("4100Client", policyBuilder =>
  {
    var allowedOrigins = builder.Configuration
            .GetSection("AllowedOrigins2")
            .Get<string[]>();

    policyBuilder.WithOrigins(allowedOrigins);

    policyBuilder.WithHeaders("Authorization", "origin", "accept")
    .WithMethods("GET");
  });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
  options.SwaggerEndpoint("/swagger/v1/swagger.json", "1.0");
  options.SwaggerEndpoint("/swagger/v2/swagger.json", "2.0");
});

app.UseRouting();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
