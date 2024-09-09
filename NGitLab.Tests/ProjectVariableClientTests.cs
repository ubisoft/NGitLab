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
            Key = "My_Key",
            Value = "My value",
            Protected = true,
        });

        Assert.That(variable.Key, Is.EqualTo("My_Key"));
        Assert.That(variable.Value, Is.EqualTo("My value"));
        Assert.That(variable.Protected, Is.EqualTo(true));

        // Update
        variable = projectVariableClient.Update(variable.Key, new VariableUpdate
        {
            Value = "My value edited",
            Protected = false,
        });

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
        var firstScope = "test/*";
        var changingScopeVariable = projectVariableClient.Create(new VariableCreate
        {
            Key = "My_Key",
            Value = "My value",
            Description = "Some important variable",
            Protected = true,
            Type = VariableType.Variable,
            Masked = false,
            Raw = false,
            EnvironmentScope = firstScope,
        });

        var integrationScope = "integration/*";
        var integrationVariable = projectVariableClient.Create(new VariableCreate
        {
            Key = "My_Key",
            Value = "My value",
            Description = "Some important variable",
            Protected = true,
            Type = VariableType.Variable,
            Masked = false,
            Raw = false,
            EnvironmentScope = integrationScope,
        });

        Assert.That(changingScopeVariable.Key, Is.EqualTo("My_Key"));
        Assert.That(changingScopeVariable.Value, Is.EqualTo("My value"));

        if (context.IsGitLabVersionInRange(VersionRange.Parse("[16.2,)"), out _))
        {
            Assert.That(changingScopeVariable.Description, Is.EqualTo("Some important variable"));
        }

        Assert.That(changingScopeVariable.Protected, Is.EqualTo(true));
        Assert.That(changingScopeVariable.Type, Is.EqualTo(VariableType.Variable));
        Assert.That(changingScopeVariable.Masked, Is.EqualTo(false));
        Assert.That(changingScopeVariable.Raw, Is.EqualTo(false));
        Assert.That(changingScopeVariable.EnvironmentScope, Is.EqualTo(firstScope));

        // Check single access with scoped variables
        Assert.That(projectVariableClient[changingScopeVariable.Key, changingScopeVariable.EnvironmentScope], Is.Not.Null);

        var exMultipleVariables = Assert.Throws<GitLabException>(() =>
        {
            var dummy = projectVariableClient[changingScopeVariable.Key];
        });
        Assert.That(exMultipleVariables?.ErrorMessage, Is.EqualTo("There are multiple variables with provided parameters. Please use 'filter[environment_scope]'"));

        // Update
        var newScope = "production/*";
        changingScopeVariable = projectVariableClient.Update(changingScopeVariable.Key, changingScopeVariable.EnvironmentScope, new VariableUpdate
        {
            Value = "My value edited",
            Protected = false,
            EnvironmentScope = newScope,
        });

        Assert.That(changingScopeVariable.Key, Is.EqualTo("My_Key"));
        Assert.That(changingScopeVariable.Value, Is.EqualTo("My value edited"));
        Assert.That(changingScopeVariable.Protected, Is.EqualTo(false));
        Assert.That(changingScopeVariable.EnvironmentScope, Is.EqualTo(newScope));

        // Delete
        var ex = Assert.Throws<GitLabException>(() => projectVariableClient.Delete(changingScopeVariable.Key, "wrongScope"));
        Assert.That(ex!.StatusCode == HttpStatusCode.NotFound);

        projectVariableClient.Delete(changingScopeVariable.Key, newScope);
        projectVariableClient.Delete(integrationVariable.Key, integrationScope);

        var variables = projectVariableClient.All.ToList();
        Assert.That(variables, Is.Empty);

        // All
        projectVariableClient.Create(new VariableCreate { Key = "Variable1", Value = "test", EnvironmentScope = "test/*" });
        projectVariableClient.Create(new VariableCreate { Key = "Variable2", Value = "test", EnvironmentScope = "integration" });
        projectVariableClient.Create(new VariableCreate { Key = "Variable3", Value = "test", EnvironmentScope = "*" });
        variables = projectVariableClient.All.ToList();
        Assert.That(variables, Has.Count.EqualTo(3));
    }
}
