using Calories.APP.Domain;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register the application's DbContext dependency injection
// by sending CaloriesDb instances to the injected service class
// constructors' DbContext or CaloriesDb parameter, using SQLite
// with the connection string from appsettings.json.
builder.Services.AddDbContext<DbContext, CaloriesDb>(
    options => options.UseSqlite(
        builder.Configuration.GetConnectionString(nameof(CaloriesDb))));
// nameof(CaloriesDb) = "CaloriesDb" which is the class name.

// Way 1:
// Registers MediatR services with the dependency injection
// container.
// MediatR is a popular .NET library that implements the
// mediator pattern, enabling decoupled communication
// between components by sending requests (commands, queries,
// events) to handlers without direct dependencies.
// The configuration below scans the assembly containing the
// 'CaloriesDb' type for any classes that implement MediatR handler
// interfaces (such as IRequestHandler, INotificationHandler,
// etc.).
// This allows automatic discovery and registration of all
// MediatR handlers in the specified assembly.
// As a result, you can inject the IMediator interface into
// controllers and use it to send requests or publish notifications,
// which will be routed to the appropriate handlers.
//builder.Services.AddMediatR(
// config => config.RegisterServicesFromAssembly(
//  typeof(CaloriesDb).Assembly));
// Way 2:
// Iterates through all assemblies currently loaded in the
// application's app domain.
// For each assembly, registers all MediatR handlers (such as
// IRequestHandler, INotificationHandler, etc.) found within
// that assembly with the dependency injection container.
// This enables MediatR to automatically discover and wire up
// handlers from any loaded assembly, allowing for modular handler
// organization and dynamic handler loading (e.g., from plugins
// or feature assemblies).
// Note: Registering handlers from all assemblies can be useful
// in large or modular applications, but may introduce duplicate
// registrations or performance overhead if not managed carefully.
foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
{
    builder.Services.AddMediatR(
        config => config.RegisterServicesFromAssemblies(assembly));
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
