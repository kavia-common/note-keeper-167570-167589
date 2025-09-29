using System.ComponentModel.DataAnnotations;

namespace NotesBackend.Contracts
{
    /// <summary>
    /// A compact representation of a note used in list responses.
    /// </summary>
    public class NoteSummaryDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTimeOffset UpdatedAtUtc { get; set; }
        public string? Color { get; set; }
    }

    /// <summary>
    /// Representation of a note used in detail responses.
    /// </summary>
    public class NoteDetailDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? Color { get; set; }
        public DateTimeOffset CreatedAtUtc { get; set; }
        public DateTimeOffset UpdatedAtUtc { get; set; }
    }

    /// <summary>
    /// Request payload for creating a new note.
    /// </summary>
    public class CreateNoteRequest
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [MaxLength(32)]
        public string? Color { get; set; }
    }

    /// <summary>
    /// Request payload for updating an existing note.
    /// </summary>
    public class UpdateNoteRequest
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [MaxLength(32)]
        public string? Color { get; set; }
    }
}
