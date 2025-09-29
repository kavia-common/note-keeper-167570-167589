using System.Collections.Concurrent;
using NotesBackend.Domain.Models;
using NotesBackend.Domain.Repositories;

namespace NotesBackend.Infrastructure.Repositories
{
    /// <summary>
    /// Thread-safe in-memory implementation of INotesRepository.
    /// Not for production use. Replace with a database-backed implementation later.
    /// </summary>
    public class InMemoryNotesRepository : INotesRepository
    {
        private readonly ConcurrentDictionary<Guid, Note> _store = new();

        public Task<Note> CreateAsync(Note note, CancellationToken ct = default)
        {
            note.CreatedAtUtc = DateTimeOffset.UtcNow;
            note.UpdatedAtUtc = note.CreatedAtUtc;
            _store[note.Id] = note;
            return Task.FromResult(note);
        }

        public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            return Task.FromResult(_store.TryRemove(id, out _));
        }

        public Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        {
            return Task.FromResult(_store.ContainsKey(id));
        }

        public Task<IReadOnlyList<Note>> GetAllAsync(CancellationToken ct = default)
        {
            var all = _store.Values.OrderByDescending(n => n.UpdatedAtUtc).ToList();
            return Task.FromResult((IReadOnlyList<Note>)all);
        }

        public Task<Note?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            _store.TryGetValue(id, out var note);
            return Task.FromResult(note);
        }

        public Task<bool> UpdateAsync(Note note, CancellationToken ct = default)
        {
            if (!_store.ContainsKey(note.Id)) return Task.FromResult(false);
            note.UpdatedAtUtc = DateTimeOffset.UtcNow;
            _store[note.Id] = note;
            return Task.FromResult(true);
        }
    }
}
