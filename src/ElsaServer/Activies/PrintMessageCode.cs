using Elsa.Workflows;
using Elsa.Workflows.Attributes;

namespace ElsaServer.Activies
{
    [Activity("MyCompany", "Print a message to the console")]
    public class PrintMessageCode : CodeActivity
    {
        protected override void Execute(ActivityExecutionContext context)
        {
            Console.WriteLine("Hello world!");
        }
    }

}
