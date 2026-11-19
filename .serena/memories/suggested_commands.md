---
name: suggested_commands
description: Commands to build, test, and work with NGitLab on Windows
metadata:
  type: project
---

```powershell
# Build
dotnet build

# Run all tests (integration tests need Docker)
dotnet test

# Run only mock (unit) tests — no Docker needed
dotnet test NGitLab.Mock.Tests

# Run integration tests (requires GitLab Docker)
dotnet test NGitLab.Tests
```
