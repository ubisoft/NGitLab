namespace NGitLab.Mock;

public sealed class Label : GitLabObject
{
    public Project Project => Parent as Project;

    public Group Group => Parent as Group;

    public long Id { get; set; }

    public string Name { get; set; }

    public string Color { get; set; }

    public string Description { get; set; }

    public Models.Label ToClientLabel()
    {
        return new Models.Label
        {
            Name = Name,
            Color = Color,
            Description = Description,
        };
    }
}
