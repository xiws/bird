namespace Domain.Core;

public interface IEventAggregate
{
    long EventId { get; }
}