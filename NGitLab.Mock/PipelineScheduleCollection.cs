using System;
using System.Linq;

namespace NGitLab.Mock;

public class PipelineScheduleCollection : Collection<PipelineSchedule>
{
    private readonly Project _project;

    public PipelineScheduleCollection(Project project)
        : base(project)
    {
        _project = project;
    }

    public PipelineSchedule GetById(long id)
    {
        return this.FirstOrDefault(x => x.Id == id);
    }

    public override void Add(PipelineSchedule schedule)
    {
        if (schedule is null)
            throw new ArgumentNullException(nameof(schedule));

        if (schedule.Id == default)
        {
            schedule.Id = Server.GetNewPipelineScheduleId();
        }

        base.Add(schedule);
    }
}
