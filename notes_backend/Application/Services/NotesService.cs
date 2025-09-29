using NotesBackend.Contracts;
using NotesBackend.Domain.Models;
using NotesBackend.Domain.Repositories;

namespace NotesBackend.Application.Services
{
    /// <summary>
    /// Handles business logic for notes and maps to DTOs.
    /// </summary>
    public class NotesService
    {
        private readonly INotesRepository _repo;

        public NotesService(INotesRepository repo)
        {
            _repo = repo;
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Lists all notes in descending updated order.
        /// </summary>
        public async Task<IReadOnlyList<NoteSummaryDto>> ListAsync(CancellationToken ct = default)
        {
            var items = await _repo.GetAllAsync(ct);
            return items.Select(ToSummaryDto).ToList();
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Gets a note by id or null when not found.
        /// </summary>
        public async Task<NoteDetailDto?> GetAsync(Guid id, CancellationToken ct = default)
        {
            var note = await _repo.GetByIdAsync(id, ct);
            return note is null ? null : ToDetailDto(note);
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Creates a new note and returns its details.
        /// </summary>
        public async Task<NoteDetailDto> CreateAsync(CreateNoteRequest req, CancellationToken ct = default)
        {
            var color = string.IsNullOrWhiteSpace(req.Color) ? "#2563EB" : req.Color!.Trim();
            var note = new Note
            {
                Title = req.Title.Trim(),
                Content = req.Content,
                Color = color
            };

            var created = await _repo.CreateAsync(note, ct);
            return ToDetailDto(created);
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Updates an existing note. Returns null if the note is not found.
        /// </summary>
        public async Task<NoteDetailDto?> UpdateAsync(Guid id, UpdateNoteRequest req, CancellationToken ct = default)
        {
            var existing = await _repo.GetByIdAsync(id, ct);
            if (existing is null) return null;

            existing.Title = req.Title.Trim();
            existing.Content = req.Content;
            existing.Color = string.IsNullOrWhiteSpace(req.Color) ? existing.Color : req.Color!.Trim();

            var ok = await _repo.UpdateAsync(existing, ct);
            return ok ? ToDetailDto(existing) : null;
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Deletes a note by id. Returns true if deleted, false if not found.
        /// </summary>
        public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default) => _repo.DeleteAsync(id, ct);

        private static NoteSummaryDto ToSummaryDto(Note n) => new()
        {
            Id = n.Id,
            Title = n.Title,
            UpdatedAtUtc = n.UpdatedAtUtc,
            Color = n.Color
        };

        private static NoteDetailDto ToDetailDto(Note n) => new()
        {
            Id = n.Id,
            Title = n.Title,
            Content = n.Content,
            Color = n.Color,
            CreatedAtUtc = n.CreatedAtUtc,
            UpdatedAtUtc = n.UpdatedAtUtc
        };
    }
}
