using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NuGet.Versioning;
using NUnit.Framework;

namespace NGitLab.Tests;

public class ProjectVariableClientTests
{
    [Test]
    [NGitLabRetry]
    public async Task Test_project_variables()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var projectVariableClient = context.Client.GetProjectVariableClient(project.Id);

        // Create
        var variable = projectVariableClient.Create(new VariableCreate
        {
            Key = "My_Key", Value = "My value", Protected = true,
        });

        Assert.That(variable.Key, Is.EqualTo("My_Key"));
        Assert.That(variable.Value, Is.EqualTo("My value"));
        Assert.That(variable.Protected, Is.EqualTo(true));

        // Update
        variable = projectVariableClient.Update(variable.Key,
            new VariableUpdate { Value = "My value edited", Protected = false });

        Assert.That(variable.Key, Is.EqualTo("My_Key"));
        Assert.That(variable.Value, Is.EqualTo("My value edited"));
        Assert.That(variable.Protected, Is.EqualTo(false));

        // Delete
        projectVariableClient.Delete(variable.Key);

        var variables = projectVariableClient.All.ToList();
        Assert.That(variables, Is.Empty);

        // All
        projectVariableClient.Create(new VariableCreate { Key = "Variable1", Value = "test" });
        projectVariableClient.Create(new VariableCreate { Key = "Variable2", Value = "test" });
        projectVariableClient.Create(new VariableCreate { Key = "Variable3", Value = "test" });
        variables = projectVariableClient.All.ToList();
        Assert.That(variables, Has.Count.EqualTo(3));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_project_variables_with_scope()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var projectVariableClient = context.Client.GetProjectVariableClient(project.Id);

        // Create
        var variable = projectVariableClient.Create(new Variable
        {
            Key = "My_Key",
            Value = "My value",
            Description = "Some important variable",
            Protected = true,
            Type = VariableType.Variable,
            Masked = false,
            Raw = false,
            Scope = "test/*",
        });

        Assert.That(variable.Key, Is.EqualTo("My_Key"));
        Assert.That(variable.Value, Is.EqualTo("My value"));

        if (context.IsGitLabVersionInRange(VersionRange.Parse("[16.2,)"), out _))
        {
            Assert.That(variable.Description, Is.EqualTo("Some important variable"));
        }

        Assert.That(variable.Protected, Is.EqualTo(true));
        Assert.That(variable.Type, Is.EqualTo(VariableType.Variable));
        Assert.That(variable.Masked, Is.EqualTo(false));
        Assert.That(variable.Raw, Is.EqualTo(false));
        Assert.That(variable.Scope, Is.EqualTo("test/*"));

        // Update
        var newScope = "integration/*";
        variable = projectVariableClient.Update(variable.Key,
            new Variable { Value = "My value edited", Protected = false, Scope = newScope},
            variable.Scope);

        Assert.That(variable.Key, Is.EqualTo("My_Key"));
        Assert.That(variable.Value, Is.EqualTo("My value edited"));
        Assert.That(variable.Protected, Is.EqualTo(false));
        Assert.That(variable.Scope, Is.EqualTo(newScope));

        // Delete
        var ex = Assert.Throws<GitLabException>(() => projectVariableClient.Delete(variable.Key, "wrongScope"));
        Assert.That(ex!.StatusCode == HttpStatusCode.NotFound);

        projectVariableClient.Delete(variable.Key, newScope);

        var variables = projectVariableClient.All.ToList();
        Assert.That(variables, Is.Empty);

        // All
        projectVariableClient.Create(new Variable { Key = "Variable1", Value = "test", Scope = "test/*" });
        projectVariableClient.Create(new Variable { Key = "Variable2", Value = "test", Scope = "integration" });
        projectVariableClient.Create(new Variable { Key = "Variable3", Value = "test", Scope = "*" });
        variables = projectVariableClient.All.ToList();
        Assert.That(variables, Has.Count.EqualTo(3));
    }
}
