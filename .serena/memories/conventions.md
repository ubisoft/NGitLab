---
name: conventions
description: Code conventions and patterns specific to the NGitLab codebase
metadata:
  type: project
---

- Integration test helpers live in `GitLabTestContext`; use `RetryUntilAsync` for any state that GitLab may compute asynchronously.
- `CreateMergeRequest()` (sync shim) / `CreateMergeRequestAsync()` (canonical): both wait for `DetailedMergeStatus` to leave transient states (`Checking`, `Unchecked`, `Preparing`) before returning, so callers can safely query diffs and approvals immediately.
- `DynamicEnum<TEnum>`: use `==`/`!=` operators against the concrete enum type (e.g. `mr.DetailedMergeStatus != DetailedMergeStatus.Checking`). When the field is absent (old GitLab), both `EnumValue` and `StringValue` are null, so `!= KnownValue` is always true.
- `GitLabCollectionResponse<T>` / `GetAllAsync<T>`: paginated; iterate with `ToListAsync()` or `await foreach`.
- Public API surface is tracked by `PublicAPI.Shipped.txt` / `PublicAPI.Unshipped.txt` per target framework — new public members must be added to `PublicAPI.Unshipped.txt`.
