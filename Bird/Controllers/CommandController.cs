using Application.Shell;
using Bird.Model;
using Microsoft.AspNetCore.Mvc;

namespace Bird.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CommandController:ControllerBase
{
    private CommandApplication cmd;

    public CommandController(CommandApplication cmd)
    {
        this.cmd = cmd;
    }
    
    /// <summary>
    /// 执行命令
    /// </summary>
    /// <param name="cmdLine"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<string> Post(CommandModel cmdLine)
    {
        return Task.FromResult(cmd.Process(cmdLine.CommandLine));
    }
}