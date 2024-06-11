using Infrastructure.Util;

namespace Application.Shell;

public class CommandApplication
{
    public string Process(string cmdLine)
    {
        return CommandProcess.Exec(cmdLine);
    }
}