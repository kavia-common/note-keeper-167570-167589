using System.ComponentModel.DataAnnotations;

namespace NotesBackend.Domain.Models
{
    /// <summary>
    /// Represents a note entity in the system.
    /// </summary>
    public class Note
    {
        /// <summary>
        /// Unique identifier of the note.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Title of the note.
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Body content of the note (Markdown or plain text).
        /// </summary>
        [Required]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Optional color tag for the note. Use Ocean Professional theme hints.
        /// </summary>
        [MaxLength(32)]
        public string? Color { get; set; } = "#2563EB"; // Ocean Professional primary as default

        /// <summary>
        /// When the note was created (UTC).
        /// </summary>
        public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// When the note was last updated (UTC).
        /// </summary>
        public DateTimeOffset UpdatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// Soft-delete flag (not used by in-memory repo but kept for future persistence upgrades).
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}
