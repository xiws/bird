using Infrastructure.Network;
using Microsoft.AspNetCore.Mvc;

namespace Bird.Controllers;
[Route("api/[controller]")]
[ApiController]
public class IndexController : ControllerBase
{
    [HttpGet]
    public async Task<string> GetTodoItems()
    {
        return await Task.FromResult("test");
    }

    [HttpGet("ping")]
    public long Ping(string ip)
    {
        var result = NetWorkService.TryPing(ip);
        return result;
    }
}