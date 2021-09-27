# NGitLabMock ease the writing of unit tests

Import NGitLab mock and create fake depots without really
creating a depot on a GitLab instance. 

It allows to write simple unit tests on code that uses GitLab.

# How to use it?

## 1. With config (*RECOMMENDED*)

Build a mocked client from a `GitLabConfig` is the new recommended method because it's easy to setup with extension methods
and support YAML serialization.

First you need to instantiate and define your configuration

```csharp
var config = new GitLabConfig()
    // Add a user
    .WithUser("test")
    // Add a new project in the user namespace and add commits
    .WithProject("user-project", @namespace: "test", configure: project => project
        .WithCommit("init", user: "test", tags: new[] { "v1.0.0" }))
    // Add a project in a group
    .WithProject("SampleProject", @namespace: "testgroup");
```

Finally, you can instance the server and the client

```csharp
var server = config.BuildServer();
var client = server.CreateClient("test");
```

Don't forget to dispose the server after the test to clean up all resources such as git repositories

```csharp
server.Dispose();
```

## 2. With models

First you need to instantiate a server

```csharp
var server = new GitLabServer();
```

Then, you can initialize the test with some data on the server.

```csharp
// Add a user
var user = new User("test");
server.Users.Add(user);

// Create a new project in the user namespace and add commits
var project = user.Namespace.Projects.AddNew();
project.Repository.Commit(user, "init"); // It creates an actual git repository
project.Repository.CreateTag("v1.0.0");

// Create a group and add a project in it
var project2 = new Project("SampleProject");
var group = new Group();
group.Projects.Add(project);
server.Groups.Add(group);
```

Finally, you can create an instance of `IGitLabClient` using `server.CreateClient(user)`

```csharp
IGitLabClient client = server.CreateClient(user);
```

## 3. With YAML file

You can define your setup configuration like in paragraph `1.` but in a YAML file...

```yaml
gitlab:
  users:
    - username: test
  projects:
    - name: user-project
      namespace: test
      commits:
        - message: init
          user: test
          tags:
            - v1.0.0
    - name: SampleProject
      namespace: testgroup
```

...and load it in code.

```csharp
var content = File.ReadAllText(filePath);
var config = GitLabConfig.Deserialize(content);
```

We extended YAML deserialization to support environment or custom variables

```yaml
variables:
  repos_path: "%localappdata%\\ngitlab\\repos"
gitlab:
  users:
    - username: user
  projects:
    - name: SampleProject
      clonePath: "{repos_path}\\SampleProject"
      commits:
        - message: Initial commit
          user: user
```

A predefined variable `new_guid` generate a new GUID at format `xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx`
