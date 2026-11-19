---
name: core
description: Top-level source map and project-wide invariants for NGitLab
metadata:
  type: project
---

# NGitLab

C# client library wrapping the GitLab REST API.

## Project layout

| Path | Purpose |
|---|---|
| `NGitLab/` | Main library (multi-target: net472, netstandard2.0, net8.0, net10.0) |
| `NGitLab.Mock/` | In-memory mock implementation for unit tests |
| `NGitLab.Tests/` | Integration tests against a real GitLab Docker container |
| `NGitLab.Mock.Tests/` | Unit tests using the mock |

Key files:
- `NGitLab/Impl/HttpRequestor.cs` — HTTP layer; `Enumerable<T>` inner class drives paginated `GetAllAsync`
- `NGitLab/Impl/MergeRequestClient.cs` — MR client (GetDiffsAsync, approvals, etc.)
- `NGitLab/DynamicEnum.cs` — wrapper for API enums with unknown future values
- `NGitLab/Models/DetailedMergeStatus.cs` — transient states: `Checking`, `Unchecked`, `Preparing`
- `NGitLab.Tests/Docker/GitLabTestContext.cs` — test fixture; `CreateMergeRequest` waits for MR to leave transient merge-status states before returning

See `mem:tech_stack`, `mem:conventions`, `mem:suggested_commands`, `mem:task_completion`.
