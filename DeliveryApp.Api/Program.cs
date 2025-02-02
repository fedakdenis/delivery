using System.Reflection;
using DeliveryApp.Api;
using DeliveryApp.Core.Domain.Services;
using DeliveryApp.Core.Ports;
using DeliveryApp.Infrastructure.Adapters.Grpc.GeoService;
using DeliveryApp.Infrastructure.Adapters.Postgres;
using DeliveryApp.Infrastructure.Adapters.Postgres.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenApi.Filters;
using OpenApi.OpenApi;
using Primitives;

var builder = WebApplication.CreateBuilder(args);

// Health Checks
builder.Services.AddHealthChecks();

// Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin(); // Не делайте так в проде!
        });
});

// Configuration
builder.Services.ConfigureOptions<SettingsSetup>();
var connectionString = builder.Configuration["CONNECTION_STRING"];

builder.Services.AddSingleton<IDispatchService, DispatchService>();
builder.Services.AddDbContext<ApplicationDbContext>(builder => {
    builder.UseNpgsql(connectionString);
});
builder.Services.AddMediatR(configuration => {
    configuration.RegisterServicesFromAssemblyContaining<DeliveryApp.Core.AssemblyAnchor>();
});

builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<ICourierRepository, CourierRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

// Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("1.0.0", new OpenApiInfo
    {
        Title = "Basket Service",
        Description = "Отвечает за формирование корзины и оформление заказа",
        Contact = new OpenApiContact
        {
            Name = "Fedak Denis",
            Url = new Uri("https://www.linkedin.com/in/denisfedak/"),
            Email = "fedakdenis@gmail.com"
        }
    });
    options.CustomSchemaIds(type => type.FriendlyId(true));
    options.IncludeXmlComments(
        $"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{Assembly.GetEntryAssembly()?.GetName().Name}.xml");
    options.DocumentFilter<BasePathFilter>("");
    options.OperationFilter<GeneratePathParamsValidationFilter>();
});
builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();



var app = builder.Build();

// -----------------------------------
// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseHsts();

app.UseHealthChecks("/health");
app.UseRouting();

// Apply Migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseHsts();

app.UseHealthChecks("/health");
app.UseRouting();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseSwagger(c => { c.RouteTemplate = "openapi/{documentName}/openapi.json"; })
    .UseSwaggerUI(options =>
    {
        options.RoutePrefix = "openapi";
        options.SwaggerEndpoint("/openapi/1.0.0/openapi.json", "Swagger Basket Service");
        options.RoutePrefix = string.Empty;
        options.SwaggerEndpoint("/openapi-original.json", "Swagger Basket Service");
    });

// gRPC
builder.Services.AddScoped<IGeoClient, GeoClient>();


app.UseCors();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();