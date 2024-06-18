using System;
using System.Collections.Generic;
using System.ComponentModel;
using NGitLab.Models;

namespace NGitLab.Impl;

internal abstract class VariableClient
{
    private readonly string _urlPrefix;
    private readonly API _api;

    private string EnvironmentScopeFilter(string environmentScope = null) => !string.IsNullOrWhiteSpace(environmentScope) ? $"?filter[environment_scope]={Uri.EscapeDataString(environmentScope!)}" : string.Empty;

    protected VariableClient(API api, string urlPrefix)
    {
        _urlPrefix = urlPrefix;
        _api = api;
    }

    public IEnumerable<Variable> All => _api.Get().GetAll<Variable>(_urlPrefix + "/variables");

    public Variable this[string key] => _api.Get().To<Variable>(_urlPrefix + "/variables/" + key);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public Variable Create(VariableCreate model) => _api.Post().With(model).To<Variable>(_urlPrefix + "/variables");

    public Variable Create(Variable model) => _api.Post().With(model).To<Variable>(_urlPrefix + "/variables");

    [EditorBrowsable(EditorBrowsableState.Never)]
    public Variable Update(string key, VariableUpdate model) => _api.Put().With(model).To<Variable>($"{_urlPrefix}/variables/{key}");

    public Variable Update(string key, Variable model) => Update(key, model, null);

    public Variable Update(string key, Variable model, string environmentScope) => _api.Put().With(model).To<Variable>($"{_urlPrefix}/variables/{key}{EnvironmentScopeFilter(environmentScope)}");

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Delete(string key) => Delete(key, null);

    public void Delete(string key, string environmentScope) => _api.Delete().Execute($"{_urlPrefix}/variables/{key}{EnvironmentScopeFilter(environmentScope)}");
}
