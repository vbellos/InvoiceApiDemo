using Api.Middleware;
using Application.Extensions;
using Infrastructure;
using Infrastructure.Extensions;
using SoftOneAssessment.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register infrastructure (database, auth service)
builder.Services.RegisterInfrastructureModule(builder.Configuration);

// Register application layer (MediatR, commands, queries)
builder.Services.RegisterApplicationModule();

// Register the rest (Controllers, Swagger)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//Add nswag Documentation
builder.Services.AddCustomOpenApiDocument();

var app = builder.Build();

app.UseMiddleware<AuthMiddleware>();

// Migrate and seed the database
DatabaseInitializer.InitializeDatabase(app.Services);

//add nswag openapi document
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.MapControllers();
app.Run();