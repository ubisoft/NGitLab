﻿using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Impl;

public class FilesClient : IFilesClient
{
    private readonly API _api;
    private readonly string _repoPath;

    public FilesClient(API api, string repoPath)
    {
        _api = api;
        _repoPath = repoPath;
    }

    public void Create(FileUpsert file)
    {
        _api.Post().With(file).Execute($"{_repoPath}/files/{EncodeFilePath(file.Path)}");
    }

    public Task CreateAsync(FileUpsert file, CancellationToken cancellationToken = default)
    {
        return _api.Post().With(file).ExecuteAsync($"{_repoPath}/files/{EncodeFilePath(file.Path)}", cancellationToken);
    }

    public void Update(FileUpsert file)
    {
        _api.Put().With(file).Execute($"{_repoPath}/files/{EncodeFilePath(file.Path)}");
    }

    public Task UpdateAsync(FileUpsert file, CancellationToken cancellationToken = default)
    {
        return _api.Put().With(file).ExecuteAsync($"{_repoPath}/files/{EncodeFilePath(file.Path)}", cancellationToken);
    }

    public void Delete(FileDelete file)
    {
        _api.Delete().With(file).Execute($"{_repoPath}/files/{EncodeFilePath(file.Path)}");
    }

    public Task DeleteAsync(FileDelete file, CancellationToken cancellationToken = default)
    {
        return _api.Delete().With(file).ExecuteAsync($"{_repoPath}/files/{EncodeFilePath(file.Path)}", cancellationToken);
    }

    public FileData Get(string filePath, string @ref)
    {
        return _api.Get().To<FileData>(_repoPath + $"/files/{EncodeFilePath(filePath)}?ref={Uri.EscapeDataString(@ref)}");
    }

    public Task<FileData> GetAsync(string filePath, string @ref, CancellationToken cancellationToken = default)
    {
        return _api.Get().ToAsync<FileData>(_repoPath + $"/files/{EncodeFilePath(filePath)}?ref={Uri.EscapeDataString(@ref)}", cancellationToken);
    }

    public Task GetRawAsync(string filePath, Func<Stream, Task> parser, GetRawFileRequest request = null, CancellationToken cancellationToken = default)
    {
        var url = _repoPath + $"/files/{EncodeFilePath(filePath)}/raw";

        if (!string.IsNullOrWhiteSpace(request?.Ref))
        {
            url = Utils.AddParameter(url, "ref", Uri.EscapeDataString(request.Ref));
        }

        if (request is not null && request.Lfs.HasValue)
        {
            url = Utils.AddParameter(url, "lfs", request.Lfs.Value);
        }

        return _api.Get().StreamAsync(url, parser, cancellationToken);
    }

    public bool FileExists(string filePath, string @ref)
    {
        try
        {
            _api.Head().Execute(_repoPath + $"/files/{EncodeFilePath(filePath)}?ref={@ref}");
            return true;
        }
        catch (GitLabException e) when (e.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    public async Task<bool> FileExistsAsync(string filePath, string @ref, CancellationToken cancellationToken = default)
    {
        try
        {
            await _api.Head().ExecuteAsync(_repoPath + $"/files/{EncodeFilePath(filePath)}?ref={@ref}", cancellationToken).ConfigureAwait(false);
            return true;
        }
        catch (GitLabException e) when (e.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    public Blame[] Blame(string filePath, string @ref)
    {
        return _api.Get().To<Blame[]>(_repoPath + $"/files/{EncodeFilePath(filePath)}/blame?ref={@ref}");
    }

    private static string EncodeFilePath(string path)
    {
        return Uri.EscapeDataString(path);
    }
}
