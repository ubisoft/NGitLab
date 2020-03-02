## What is NGitLab?

*NGitLab* is a .NET REST client implementation of GitLab API with no external dependencies.

## Usage

It's a wrapper of REST api. Read the [GitLab docs](https://github.com/gitlabhq/gitlabhq/tree/master/doc/api) and start using by creating a GitLabClient instance:

```csharp
var client =  new GitLabClient("https://mygitlab.example.com", "your_private_token");
```

or

```cs
var client =  new GitLabClient("https://mygitlab.example.com", "username", "password");
```

_Username and Password authentication is disabled for users with two-factor authentication turned on [see GitLab Documentation](https://docs.gitlab.com/ce/api/session.html)._

Then use its properties. You can obtain the private token in your account page. You may want to create a custom user for the API usage.

## Where can I get it?

Get it from NuGet. You can simply install it with the Package Manager console:

    PM> Install-Package NGitLab
    
## Unit-Test

Unit tests are running against a GitLab Server. The easiest way to host a GitLab Server quickly use docker:

```
docker run --detach --hostname gitlab.example.com --publish 443:443 --publish 80:80 --publish 2222:22 --name gitlab --restart always --volume /srv/gitlab/config:/etc/gitlab --volume /srv/gitlab/logs:/var/log/gitlab --volume /srv/gitlab/data:/var/opt/gitlab gitlab/gitlab-ce:latest
```

## Maintainer

NGitLab is a fork of [github project](https://github.com/Franklin89/NGitLab) 
