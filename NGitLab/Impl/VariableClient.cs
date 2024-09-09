using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl;

internal abstract class VariableClient
{
    private readonly string _urlPrefix;
    private readonly API _api;

    private static string EnvironmentScopeFilter(string environmentScope = null) => !string.IsNullOrWhiteSpace(environmentScope) ? $"?filter[environment_scope]={Uri.EscapeDataString(environmentScope)}" : string.Empty;

    protected VariableClient(API api, string urlPrefix)
    {
        _urlPrefix = urlPrefix;
        _api = api;
    }

    public IEnumerable<Variable> All => _api.Get().GetAll<Variable>(_urlPrefix + "/variables");

    public Variable this[string key] => this[key, null];

    public Variable this[string key, string environmentScope] => _api.Get().To<Variable>($"{_urlPrefix}/variables/{key}{EnvironmentScopeFilter(environmentScope)}");

    public Variable Create(VariableCreate model) => _api.Post().With(model).To<Variable>(_urlPrefix + "/variables");

    public Variable Update(string key, VariableUpdate model) => Update(key, null, model);

    public Variable Update(string key, string environmentScope, VariableUpdate model) => _api.Put().With(model).To<Variable>($"{_urlPrefix}/variables/{key}{EnvironmentScopeFilter(environmentScope)}");

    public void Delete(string key) => Delete(key, null);

    public void Delete(string key, string environmentScope) => _api.Delete().Execute($"{_urlPrefix}/variables/{key}{EnvironmentScopeFilter(environmentScope)}");
}
