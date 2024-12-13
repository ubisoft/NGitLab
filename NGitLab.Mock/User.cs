using System;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock;

public sealed class User : GitLabObject
{
    public User(string userName)
    {
        UserName = userName;
        Name = userName;
        Email = userName + "@example.com";
        State = UserState.active;
    }

    public long Id { get; set; }

    public string UserName { get; }

    public string Name { get; set; }

    public string Email { get; set; }

    public bool IsAdmin { get; set; }

    public string AvatarUrl { get; set; }

    public UserState State { get; set; }

    public DateTime CreatedAt { get; set; }

    public Identity[] Identities { get; set; } = Array.Empty<Identity>();

    public string WebUrl => Server.MakeUrl(UserName);

    public bool Bot
    {
        get
        {
            if (string.IsNullOrEmpty(UserName))
                return false;

            var nameParts = UserName.Split('_');

            if (nameParts.Length != 4)
                return false;

            if (!string.Equals(nameParts[0], "project", StringComparison.Ordinal) && !string.Equals(nameParts[0], "group", StringComparison.Ordinal))
                return false;

            return int.TryParse(nameParts[1], out var _) && string.Equals("bot", nameParts[2], StringComparison.Ordinal);
        }
    }

    public Models.User ToClientUser()
    {
        var user = new Models.User();
        CopyTo(user);
        return user;
    }

    public Session ToClientSession()
    {
        var user = new Session();
        CopyTo(user);
        return user;
    }

    private void CopyTo<T>(T instance)
        where T : Models.User
    {
        instance.Id = Id;
        instance.Username = UserName;
        instance.Name = Name;
        instance.Email = Email;
        instance.State = State.ToString();
        instance.AvatarURL = AvatarUrl;
        instance.CreatedAt = CreatedAt;
        instance.Identities = Identities;
        instance.Bot = Bot;

        if (IsAdmin)
        {
            instance.IsAdmin = true;
        }
    }

    public Group Namespace => Server.Groups.FirstOrDefault(group => string.Equals(group.PathWithNameSpace, UserName, StringComparison.Ordinal));

    public override string ToString() => $"{Id}: {UserName}";
}
