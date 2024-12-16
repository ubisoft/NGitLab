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

    Badge this[long id] { get; }

    Badge Create(BadgeCreate badge);

    Badge Update(long id, BadgeUpdate badge);

    void Delete(long id);
}

public interface IProjectVariableClient
{
    IEnumerable<Variable> All { get; }

    Variable this[string key] { get; }

    Variable this[string key, string environmentScope] { get; }

    Variable Create(VariableCreate model);

    Variable Update(string key, VariableUpdate model);

    Variable Update(string key, string environmentScope, VariableUpdate model);

    void Delete(string key);

    void Delete(string key, string environmentScope);
}

public interface IGroupVariableClient
{
    IEnumerable<Variable> All { get; }

    Variable this[string key] { get; }

    Variable this[string key, string environmentScope] { get; }

    Variable Create(VariableCreate model);

    Variable Update(string key, VariableUpdate model);

    Variable Update(string key, string environmentScope, VariableUpdate model);

    void Delete(string key);

    void Delete(string key, string environmentScope);
}
