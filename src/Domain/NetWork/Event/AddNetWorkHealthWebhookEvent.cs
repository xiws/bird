using Domain.Core;

namespace Domain.NetWork.Event;

public class AddNetWorkHealthWebhookEvent : EventAggregate
{
    public List<WebhookEntity> Webhook { get; set; }

    public AddNetWorkHealthWebhookEvent(List<WebhookEntity> webhook)
    {
        Webhook = webhook;
    }
}