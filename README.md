![License](https://img.shields.io/github/license/franklin89/NGitLab.svg)
[![Build status](https://ci.appveyor.com/api/projects/status/4sufsyhxh9m7ga6g?svg=true)](https://ci.appveyor.com/project/Franklin89/ngitlab)
[![Gitter](https://badges.gitter.im/JoinChat.svg)](https://gitter.im/ML-Software/NGitLab?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[![Nuget Version](https://img.shields.io/nuget/v/NGitLab.svg)](https://www.nuget.org/packages/NGitLab/)
[![Nuget Version](https://img.shields.io/nuget/vpre/NGitLab.svg)](https://www.nuget.org/packages/NGitLab/)

# NGitLab

## What is NGitLab?

*NGitLab* is a .NET REST client implementation of GitLab API with no external dependencies.

## How can I learn it?

It's a wrap of REST api. Read the [GitLab docs](https://github.com/gitlabhq/gitlabhq/tree/master/doc/api) and start using by creating a GitLabClient instance:

```csharp
var client =  GitLabClient.Connect("https://mygitlab.example.com", "your_private_token");
```

Then use its properties. You can obtain the private token in your account page. You may want to create a custom user for the API usage.

## Where can I get it?

Get it from NuGet. You can simply install it with the Package Manager console:

    PM> Install-Package NGitLab

## Maintainer

Big thanks to @Scooletz to bringing this project to life. I am happy to continue his work.
