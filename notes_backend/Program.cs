using NotesBackend.Application.Services;
using NotesBackend.Domain.Repositories;
using NotesBackend.Infrastructure.Repositories;
using NotesBackend.Presentation.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Configure OpenAPI/Swagger with metadata and tags
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(settings =>
{
    settings.Title = "Notes API";
    settings.Description = "A modern, Ocean Professional themed Notes API. Use the /api/notes endpoints to create, read, update, and delete notes.";
    settings.Version = "1.0.0";
    settings.DocumentName = "v1";
    // Configure tags via PostProcess for NSwag
    settings.PostProcess = document =>
    {
        document.Tags = new List<NSwag.OpenApiTag>
        {
            new NSwag.OpenApiTag
            {
                Name = "Notes",
                Description = "CRUD endpoints for notes"
            }
        };
    };
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowCredentials()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Dependency Injection
builder.Services.AddSingleton<INotesRepository, InMemoryNotesRepository>();
builder.Services.AddScoped<NotesService>();

var app = builder.Build();

// Middleware
app.UseCors("AllowAll");
app.UseOpenApi();
app.UseSwaggerUi(options =>
{
    options.Path = "/docs";
    options.DocumentTitle = "Notes API Docs";
});

// Health check endpoint
// PUBLIC_INTERFACE
app.MapGet("/", () => Results.Ok(new
{
    status = "Healthy",
    name = "Notes Backend",
    theme = new
    {
        name = "Ocean Professional",
        primary = "#2563EB",
        secondary = "#F59E0B",
        background = "#f9fafb",
        surface = "#ffffff",
        text = "#111827"
    }
}))
.WithName("HealthCheck")
.WithSummary("Service health check")
.WithDescription("Returns health and theme information for the Notes backend.")
.Produces(StatusCodes.Status200OK);

// Map Notes endpoints
app.MapNotesEndpoints();

app.Run();