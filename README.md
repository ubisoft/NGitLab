[![NuGet](https://img.shields.io/nuget/v/NGitLab.svg)](https://www.nuget.org/packages/NGitLab/)

## What is NGitLab?

`NGitLab` is a .NET REST client implementation for the GitLab API.

## Usage

Start by creating a `GitLabClient` instance:

```csharp
var client = new GitLabClient("https://mygitlab.example.com", "your_private_token");
```

Then use its properties. You can obtain the private token in your account page. You may want to create a custom user for the API usage.

For further info about the GitLab API, refer to [the official documentation](https://docs.gitlab.com/ee/api/rest/)

## Where can I get it?

Get it from [NuGet](https://www.nuget.org/packages/NGitLab). You can simply install it with the Package Manager console:

> PM> Install-Package NGitLab

## Running Unit Tests locally

- Install Docker on your machine
- It's recommended to use WSL version 2: https://docs.microsoft.com/en-us/windows/wsl/install-win10
- Executing tests under Linux requires PowerShell to be installed. (https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-core-on-linux)

## Thanks

Thanks to [Scooletz](https://github.com/Scooletz) for initiating the original project.
