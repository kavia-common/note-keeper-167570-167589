using NotesBackend.Domain.Models;

namespace NotesBackend.Domain.Repositories
{
    /// <summary>
    /// Abstraction for persisting and retrieving notes.
    /// </summary>
    public interface INotesRepository
    {
        Task<IReadOnlyList<Note>> GetAllAsync(CancellationToken ct = default);
        Task<Note?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Note> CreateAsync(Note note, CancellationToken ct = default);
        Task<bool> UpdateAsync(Note note, CancellationToken ct = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
    }
}
