using Microsoft.AspNetCore.Mvc;

namespace ElsaServer.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ElsaController: ControllerBase
    {
        [HttpGet]
        public async Task<string> GetTodoItems()
        {
            return await Task.FromResult("test");
        }
    }
}
