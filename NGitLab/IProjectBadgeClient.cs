using System.Collections.Generic;
using System.ComponentModel;
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

    [EditorBrowsable(EditorBrowsableState.Never)]
    Variable Create(VariableCreate model);

    Variable Create(Variable model);

    [EditorBrowsable(EditorBrowsableState.Never)]
    Variable Update(string key, VariableUpdate model);

    Variable Update(string key, Variable model);

    Variable Update(string key, Variable model, string environmentScope);

    [EditorBrowsable(EditorBrowsableState.Never)]
    void Delete(string key);

    void Delete(string key, string environmentScope);
}

public interface IGroupVariableClient
{
    IEnumerable<Variable> All { get; }

    Variable this[string key] { get; }

    Variable Create(VariableCreate model);

    Variable Create(Variable model);

    [EditorBrowsable(EditorBrowsableState.Never)]
    Variable Update(string key, VariableUpdate model);

    Variable Update(string key, Variable model);

    Variable Update(string key, Variable model, string environmentScope);

    [EditorBrowsable(EditorBrowsableState.Never)]
    void Delete(string key);

    void Delete(string key, string environmentScope);
}
