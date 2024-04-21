using System.Net.Http.Json;
using Application.Model;
using Domain.NetWork;
using Infrastructure.Network;
using Infrastructure.Util;

namespace Application.NetWork;

public class NetWorkApplication
{
    public NetWorkApplication()
    {
    }

    public async Task<bool> TryPing()
    {
        var aggregateId = SnowFlake.Singleton.NextId();
        var hostIp = "172.16.0.26";
        var networkConfig = new NetWorkHealthEntity(aggregateId, hostIp,1);
        networkConfig.AddWebhookHost(new WebhookEntity()
        { 
            Host = "http://localhost:5034/api/Command", 
            Param = new CommandModel
            {
                CommandLine = "pgyvisitor login -u 12370659:003 -p jx960721"
            }
        });
        HttpRequest request = new HttpRequest();
        if (NetWorkService.TryPing(networkConfig.Host) > 0) return true;

        foreach (var webhook in networkConfig.Webhook)
        {
            await request.Send(new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = JsonContent.Create(webhook.Param),
                RequestUri = new Uri( webhook.Host)
            });
        }
        
        return false;
    }
}