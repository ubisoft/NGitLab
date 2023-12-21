using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab;

public interface IEventClient
{
    IEnumerable<Event> Get(EventQuery query);
}
