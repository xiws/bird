using Domain.Core;

namespace Domain.NetWork;

public class WebhookEntity: IEntity
{
    public string Host { get; set; }
    
    public object Param { get; set; }
}