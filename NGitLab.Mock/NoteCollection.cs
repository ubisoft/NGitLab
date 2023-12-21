using System;
using System.Linq;

namespace NGitLab.Mock;

public sealed class NoteCollection<T> : Collection<T>
    where T : Note
{
    public NoteCollection(GitLabObject parent)
        : base(parent)
    {
    }

    public T GetById(long id) => this.FirstOrDefault(item => item.Id == id);

    public override void Add(T item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        if (item.Id == default)
        {
            item.Id = GetNewId();
        }
        else if (GetById(item.Id) != null)
        {
            throw new GitLabException("Note already exists");
        }

        base.Add(item);
    }

    private long GetNewId()
    {
        return this.Select(note => note.Id).DefaultIfEmpty().Max() + 1;
    }
}
