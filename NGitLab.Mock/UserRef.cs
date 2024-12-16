using System;
using NGitLab.Models;

namespace NGitLab.Mock;

public sealed class UserRef
{
    private readonly User _user;

    public UserRef(User user)
    {
        _user = user ?? throw new ArgumentNullException(nameof(user));
    }

    public long Id => _user.Id;

    public string Name => _user.Name;

    public string UserName => _user.UserName;

    public string Email => _user.Email;

    public Models.User ToUserClient()
    {
        return new Models.User
        {
            Id = Id,
            Name = Name,
            Email = Email,
            Username = UserName,
        };
    }

    public static implicit operator UserRef(User user)
    {
        if (user == null)
            return null;

        return new UserRef(user);
    }

    public Author ToClientAuthor()
    {
        return new Author
        {
            Id = Id,
            Username = UserName,
            Email = Email,
            Name = Name,
            State = _user.State.ToString(),
            AvatarUrl = _user.AvatarUrl,
            WebUrl = _user.WebUrl,
        };
    }

    public Assignee ToClientAssignee()
    {
        return new Assignee
        {
            Id = Id,
            Username = UserName,
            Email = Email,
            Name = Name,
            State = _user.State.ToString(),
            AvatarURL = _user.AvatarUrl,
        };
    }
}
