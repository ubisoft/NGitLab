---
name: task_completion
description: Steps to verify a coding task is done in NGitLab
metadata:
  type: project
---

1. `dotnet build` — must succeed with no errors
2. `dotnet test NGitLab.Mock.Tests` — fast unit tests; should always pass without Docker
3. `dotnet test NGitLab.Tests` — integration tests; requires GitLab Docker container
4. Check `NGitLab/PublicAPI/*/PublicAPI.Unshipped.txt` if you added/changed any public API
