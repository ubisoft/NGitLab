Contact me if you want to be the new maintainer of this?
=============
I can't maintain this project. Contact me if you want to have it keep it alive.

What is NGitLab?
=============

*NGitLab* is a .NET REST client implementation of GitLab API with no external dependencies.

How can I learn it?
=============

It's a wrap of REST api. Read the [GitLab docs](https://github.com/gitlabhq/gitlabhq/tree/master/doc/api) and start using by creating a GitLabClient instance:

```csharp

var client =  GitLabClient.Connect("https://mygitlab.example.com", "your_private_token");
```

Then use its properties. You can obtain the private token in your account page. You may want to create a custom user for the API usage.

Where can I get it?
=============

Get it from NuGet. You can simply install it with the Package Manager console:
    
    PM> Install-Package NGitLab
