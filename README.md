[![NuGet](https://img.shields.io/nuget/v/NGitLab.svg)](https://www.nuget.org/packages/NGitLab/)

## What is NGitLab?

`NGitLab` is a .NET client implementation for the GitLab REST API.

## Usage

Start by creating a `GitLabClient` instance:

```csharp
var client = new GitLabClient("https://mygitlab.example.com", "your_private_token");
```

Then use its properties. You can obtain the private token in your account page. You may want to create a custom user for the API usage.

For further info about the GitLab API, refer to [the official documentation](https://docs.gitlab.com/api/rest/)

## Where can I get it?

Get it from [nuget.org](https://www.nuget.org/packages/NGitLab). You can simply install it using the `dotnet` CLI:

```PowerShell
dotnet add package NGitLab
```

## Running Unit Tests locally

- Install Docker on your machine
- It's recommended to use WSL version 2 _(see [here](https://learn.microsoft.com/en-us/windows/wsl/install))_
- Running the Docker-based integration tests (`NGitLab.Tests`) under Linux requires PowerShell (`pwsh`) to be installed, because they start a GitLab runner that uses the PowerShell shell _(see [here](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-linux))_

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md). In particular, changes to the public API are checked by snapshot tests: the page explains how to check them and how to update the snapshots.

## Thanks

Thanks to [Scooletz](https://github.com/Scooletz) for creating the original project.
