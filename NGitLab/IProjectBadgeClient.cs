using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab;

public interface IProjectBadgeClient
{
    IEnumerable<Badge> All { get; }

    /// <summary>
    /// Gets the project's badges only (not group badges)
    /// </summary>
    /// <remarks>Project Badge API returns both Group and Project badges</remarks>
    IEnumerable<Badge> ProjectsOnly { get; }

    Badge this[int id] { get; }

    Badge Create(BadgeCreate badge);

    Badge Update(int id, BadgeUpdate badge);

    void Delete(int id);
}

public interface IProjectVariableClient
{
    IEnumerable<Variable> All { get; }

    Variable this[string key] { get; }

    [Obsolete($"Use '{nameof(Create)}({nameof(Variable)} model)' instead")]
    Variable Create(VariableCreate model);

    Variable Create(Variable model);

    Variable Update(string key, VariableUpdate model);

    void Delete(string key);
}

public interface IGroupVariableClient
{
    IEnumerable<Variable> All { get; }

    Variable this[string key] { get; }

    Variable Create(VariableCreate model);

    Variable Create(Variable model);

    Variable Update(string key, VariableUpdate model);

    void Delete(string key);
}
