using System;
using System.Linq;

namespace NGitLab.Mock;

public sealed class LabelsCollection : Collection<Label>
{
    public LabelsCollection(GitLabObject parent)
        : base(parent)
    {
    }

    public Label GetById(long id)
    {
        return this.FirstOrDefault(l => l.Id == id);
    }

    public Label GetByName(string name)
    {
        return this.FirstOrDefault(l => l.Name.Equals(name, StringComparison.Ordinal));
    }

    public Label Add(string name = null, string color = null, string description = null)
    {
        if (string.IsNullOrEmpty(name))
        {
            name = Guid.NewGuid().ToString();
        }

        if (string.IsNullOrEmpty(color))
        {
            color = "#d9534f";
        }

        var label = new Label
        {
            Name = name,
            Color = color,
            Description = description,
        };

        Add(label);
        return label;
    }

    public override void Add(Label label)
    {
        if (label is null)
            throw new ArgumentNullException(nameof(label));

        if (label.Id == default)
        {
            label.Id = Server.GetNewLabelId();
        }
        else if (GetById(label.Id) != null)
        {
            throw new GitLabException("Label already exists");
        }

        if (GetByName(label.Name) != null)
        {
            throw new GitLabException("Label already exists");
        }

        base.Add(label);
    }
}
