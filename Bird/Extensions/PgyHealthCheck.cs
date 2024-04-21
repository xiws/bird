using Application.NetWork;
using Infrastructure.Network;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Bird.Extensions;

public class PgyHealthCheck : IHealthCheck
{
    private NetWorkApplication _netWorkApplication;
    public PgyHealthCheck(NetWorkApplication  netWorkApplication )
    {
        this._netWorkApplication = netWorkApplication;
    }
    
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        if(await _netWorkApplication.TryPing()) return HealthCheckResult.Healthy("正常");
        return HealthCheckResult.Unhealthy("蒲公英断了");
    }
}