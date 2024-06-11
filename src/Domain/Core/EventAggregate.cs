using Infrastructure.Util;

namespace Domain.Core;

public abstract class EventAggregate :IEventAggregate
{
    public long EventId { get; }

    protected EventAggregate()
    {
        EventId = SnowFlake.Singleton.NextId();
    }
}