using Microsoft.AspNetCore.Mvc;
using NotesBackend.Application.Services;
using NotesBackend.Contracts;

namespace NotesBackend.Presentation.Endpoints
{
    /// <summary>
    /// Minimal API endpoint mappings for the Notes resource.
    /// </summary>
    public static class NotesEndpoints
    {
        // PUBLIC_INTERFACE
        /// <summary>
        /// Maps all CRUD endpoints for notes under /api/notes.
        /// </summary>
        public static IEndpointRouteBuilder MapNotesEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/notes")
                           .WithTags("Notes")
                           .WithDescription("CRUD operations for notes");

            // List notes
            group.MapGet("/", async ([FromServices] NotesService svc, CancellationToken ct) =>
                {
                    var list = await svc.ListAsync(ct);
                    return Results.Ok(list);
                })
                .WithName("ListNotes")
                .WithSummary("List notes")
                .WithDescription("Returns a collection of notes ordered by last updated time (desc).")
                .Produces<List<NoteSummaryDto>>(StatusCodes.Status200OK);

            // Get note by id
            group.MapGet("/{id:guid}", async ([FromRoute] Guid id, [FromServices] NotesService svc, CancellationToken ct) =>
                {
                    var note = await svc.GetAsync(id, ct);
                    return note is null ? Results.NotFound() : Results.Ok(note);
                })
                .WithName("GetNote")
                .WithSummary("Get a note")
                .WithDescription("Returns a single note by its unique identifier.")
                .Produces<NoteDetailDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            // Create note
            group.MapPost("/", async ([FromBody] CreateNoteRequest request, [FromServices] NotesService svc, CancellationToken ct) =>
                {
                    if (request is null) return Results.BadRequest(new { error = "Invalid payload." });

                    var created = await svc.CreateAsync(request, ct);
                    return Results.Created($"/api/notes/{created.Id}", created);
                })
                .WithName("CreateNote")
                .WithSummary("Create a note")
                .WithDescription("Creates a new note. Title and content are required. Color is optional (defaults to theme primary).")
                .Produces<NoteDetailDto>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            // Update note
            group.MapPut("/{id:guid}", async ([FromRoute] Guid id, [FromBody] UpdateNoteRequest request, [FromServices] NotesService svc, CancellationToken ct) =>
                {
                    if (request is null) return Results.BadRequest(new { error = "Invalid payload." });

                    var updated = await svc.UpdateAsync(id, request, ct);
                    return updated is null ? Results.NotFound() : Results.Ok(updated);
                })
                .WithName("UpdateNote")
                .WithSummary("Update a note")
                .WithDescription("Updates title, content, and optionally color of an existing note.")
                .Produces<NoteDetailDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            // Delete note
            group.MapDelete("/{id:guid}", async ([FromRoute] Guid id, [FromServices] NotesService svc, CancellationToken ct) =>
                {
                    var deleted = await svc.DeleteAsync(id, ct);
                    return deleted ? Results.NoContent() : Results.NotFound();
                })
                .WithName("DeleteNote")
                .WithSummary("Delete a note")
                .WithDescription("Deletes a note by its identifier.")
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound);

            return app;
        }
    }
}
