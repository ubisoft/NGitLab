# Contributing to NGitLab

Thanks for your interest in contributing! This page covers the things that are specific to this repository.
For how to run the tests, see [Running Unit Tests locally](README.md#running-unit-tests-locally).

## Public API changes

`NGitLab` is published on NuGet, so accidental breaking changes to its public API must be caught before release.
To do that, the tests generate the public API surface of the assemblies (using
[PublicApiGenerator](https://github.com/PublicApiGenerator/PublicApiGenerator)) and compare it to a committed
snapshot (using [Verify](https://github.com/VerifyTests/Verify)).

| Project | Test | Snapshot(s) |
| --- | --- | --- |
| `NGitLab` | `NGitLab.Tests/Internals/AssemblyTests.cs` | `NGitLab.Tests/Internals/snapshots/*.verified.txt` |
| `NGitLab.Mock` | `NGitLab.Mock.Tests/Internals/AssemblyTests.cs` | `NGitLab.Mock.Tests/Internals/snapshots/*.verified.txt` (one per target framework) |

### Checking whether your change affects the public API

Run the `AssemblyTests` of both test projects:

```PowerShell
dotnet test NGitLab.Tests --filter "FullyQualifiedName~AssemblyTests"
dotnet test NGitLab.Mock.Tests --filter "FullyQualifiedName~AssemblyTests"
```

`NGitLab.Mock.Tests` targets several frameworks and each one has its own snapshot, so run the tests for every
target framework (the default `dotnet test` does this).
These tests do not need Docker.

If the public API changed, the test fails and Verify writes a `*.received.txt` file next to each
`*.verified.txt` file that differs. Diff the two files to see exactly what changed.

### Is my change breaking?

- **Additions** (new types, methods, properties, enum values) are not breaking.
- **Removals, renames, signature changes, or changes to the type of a member** are breaking.
  Avoid them. Mark the old member `[Obsolete]` and add the new one next to it, or call it out in your pull request so
  that maintainers can plan for it.

### Updating the snapshots

If the change is intentional, accept the new API by replacing each `*.verified.txt` file with its
`*.received.txt` counterpart:

```PowerShell
# From the repository root
Get-ChildItem -Recurse -Filter "AssemblyTests.*.received.txt" | ForEach-Object {
    Move-Item -Force $_.FullName ($_.FullName -replace '\.received\.txt$', '.verified.txt')
}
```

Then re-run the tests to confirm they pass, and commit the updated `*.verified.txt` files together with your code change.
`*.received.txt` files are git-ignored and must never be committed.

Reviewers: the diff of the `*.verified.txt` files is the API changelog of the pull request, so please read it.

### Notes

- The `RepositoryUrl` assembly metadata is scrubbed from the snapshot because it depends on the local git remote.
  A diff on that line is not expected.
- The snapshots are generated per target framework where the API differs between frameworks, so make sure you update
  all of the `*.verified.txt` files that changed.
