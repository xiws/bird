using System.Net.NetworkInformation;

namespace Infrastructure.Network;

public class NetWorkService
{
    public static long TryPing(string host)
    {
        using (Ping pingSender = new Ping())
        {
            PingReply reply = pingSender.Send(host);
            if (reply.Status == IPStatus.Success) return reply.RoundtripTime;
            else return -1;
        }
    }
}