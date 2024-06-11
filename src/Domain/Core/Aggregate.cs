namespace Domain.Core;

public abstract class Aggregate :IAggregate
{
    public virtual long AggregateId { get; protected set; }

    protected void Handle<T>(T eventAggregate) where T : IEventAggregate
    {
        dynamic tes = this;
        tes.Handle(eventAggregate);
    }
}