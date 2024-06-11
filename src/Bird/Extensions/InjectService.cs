using Application.NetWork;
using Application.Shell;

namespace Bird.Extensions;

public class InjectService
{
    public static IServiceCollection ServiceInject(IServiceCollection services)
    {
        services.AddSingleton<NetWorkApplication>();
        services.AddSingleton<CommandApplication>();
        
        return services;
    }
}