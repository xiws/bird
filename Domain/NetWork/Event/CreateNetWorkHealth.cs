using Domain.Core;

namespace Domain.NetWork.Event;

public class CreateNetWorkHealth : EventAggregate
{
    public string Host { get; protected set; }
    
    public int DetectionType { get; protected set; }

    public CreateNetWorkHealth(string host, int detectionType)
    {
        Host = host;
        DetectionType = detectionType;
    }

    public CreateNetWorkHealth()
    {
    }
}