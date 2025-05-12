using System;

namespace NGitLab.Models;

[Flags]
public enum JobScopeMask
{
    All = Created | Pending | Running | Failed | Success | Canceled | Skipped | Manual,
    Created = 0x01,
    Pending = 0x02,
    Running = 0x04,
    Failed = 0x08,
    Success = 0x10,
    Canceled = 0x20,
    Skipped = 0x40,
    Manual = 0x80,
}
