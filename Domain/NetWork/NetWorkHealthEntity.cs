using Domain.Core;
using Domain.NetWork.Event;
using Infrastructure.Util;

namespace Domain.NetWork;

public class NetWorkHealthEntity : Aggregate
{
    public string Host { get; protected set; }
    
    public int DetectionType { get; protected set; }
    public List<WebhookEntity> Webhook { get; protected set; }

    public NetWorkHealthEntity() 
    {
        Webhook = new List<WebhookEntity>();
    }

    public NetWorkHealthEntity(long aggregateId,string host,int detectionType)
    {
        Webhook = new List<WebhookEntity>();
        Handle(new CreateNetWorkHealth(host, detectionType));
    }

    public void AddWebhookHost(WebhookEntity host)
    {
        AddWebhookHost(new List<WebhookEntity>() { host });
    }

    public void AddWebhookHost(List<WebhookEntity> hosts)
    {
        Handle(new AddNetWorkHealthWebhookEvent(hosts));
    }
    
    protected void Handle(CreateNetWorkHealth aggregateEvent)
    {
        this.Host = aggregateEvent.Host;
        this.DetectionType = aggregateEvent.DetectionType;
    }

    protected void Handle(AddNetWorkHealthWebhookEvent aggregateEvent)
    {
        Webhook.AddRange(aggregateEvent.Webhook);
    }
}