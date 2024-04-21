using Elsa.Workflows.Activities;
using Elsa.Workflows.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Bird.Controllers;

[ApiController]
[Route("run-workflow")]
public class RunWorkflowController : ControllerBase
{
    private IWorkflowRunner workflowRunner;

    public RunWorkflowController(IWorkflowRunner workflowRunner)
    {
        this.workflowRunner = workflowRunner;
    }
    
    [HttpGet]
    public async Task Get()
    {
        await workflowRunner.RunAsync(new WriteLine("Hello ASP.NET world!"));
    }
    
}