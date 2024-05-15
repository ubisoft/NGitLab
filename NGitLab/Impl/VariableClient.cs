using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl;

internal abstract class VariableClient
{
    private readonly string _urlPrefix;
    private readonly API _api;

    protected VariableClient(API api, string urlPrefix)
    {
        _urlPrefix = urlPrefix;
        _api = api;
    }

    public IEnumerable<Variable> All => _api.Get().GetAll<Variable>(_urlPrefix + "/variables");

    public Variable this[string key] => _api.Get().To<Variable>(_urlPrefix + "/variables/" + key);

    [Obsolete($"Use '{nameof(Create)}({nameof(Variable)} model)' instead")]
    public Variable Create(VariableCreate model) => _api.Post().With(model).To<Variable>(_urlPrefix + "/variables");

    public Variable Create(Variable model) => _api.Post().With(model).To<Variable>(_urlPrefix + "/variables");

    [Obsolete($"Use '{nameof(Update)}({nameof(Variable)} model)' instead")]
    public Variable Update(string key, VariableUpdate model) => _api.Put().With(model).To<Variable>($"{_urlPrefix}/variables/{key}");

#if (NET || NET48_OR_GREATER || NETSTANDARD2_1_OR_GREATER)
    public Variable Update(string key, Variable model, string environmentScope = null) => _api.Put().With(model).To<Variable>(string.IsNullOrWhiteSpace(environmentScope) ? $"{_urlPrefix}/variables/{key}" : $"{_urlPrefix}/variables/{key}?filter[environment_scope]={Uri.EscapeDataString(environmentScope!)}");
#else
    public Variable Update(string key, Variable model) => Update(key, model, null);

    public Variable Update(string key, Variable model, string environmentScope) => _api.Put().With(model).To<Variable>(string.IsNullOrWhiteSpace(environmentScope) ? $"{_urlPrefix}/variables/{key}" : $"{_urlPrefix}/variables/{key}?filter[environment_scope]={Uri.EscapeDataString(environmentScope!)}");
#endif

    public void Delete(string key, string environmentScope = null) => _api.Delete().Execute(string.IsNullOrWhiteSpace(environmentScope) ? $"{_urlPrefix}/variables/{key}" : $"{_urlPrefix}/variables/{key}?filter[environment_scope]={Uri.EscapeDataString(environmentScope!)}");
}
