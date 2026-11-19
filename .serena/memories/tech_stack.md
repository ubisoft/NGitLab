---
name: tech_stack
description: Language, frameworks, build tools, and key dependencies for NGitLab
metadata:
  type: project
---

- **Language**: C# (.NET multi-target: net472, netstandard2.0, net8.0, net10.0)
- **Build**: `dotnet build` / `dotnet test`
- **Test framework**: NUnit 4 (watch for CS0121 ambiguous overloads — see `mem:feedback/nunit4`)
- **HTTP**: `WebRequest`/`WebResponse` (legacy; SYSLIB0014 kept as warning, not error)
- **JSON**: `System.Text.Json` with custom converters (`DynamicEnumConverterFactory`)
- **Polly**: used in integration tests for retry policy (`s_gitlabRetryPolicy`)
- **Integration tests**: require a running GitLab Docker container; use `NGitLabRetryAttribute` (10 retries) for flaky tests
