using System.Diagnostics;

namespace Infrastructure.Util;

public class CommandProcess
{
    public static string Exec(string cmdLine)
    {
        Process process = new Process
        {
#if  DEBUG
            StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {cmdLine}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
#else 
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{cmdLine}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
#endif
        }; 
        process.Start(); 
        var result = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return result;
    }
}