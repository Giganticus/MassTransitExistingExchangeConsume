using MassTransit;

namespace GettingStarted.Contracts;

//[ExcludeFromTopology]
//[EntityName("my.existing.exchange")]
public class WorkflowResponse
{
    public Compass compass { get; set; }
}

public class Compass
{
    public long correlationId { get; set; }
    public int executionId { get; set; }
    public Status status { get; set; }
}

public class Status
{
    public string message { get; set; }
    public int statusId { get; set; }
    public string statusName { get; set; }
}

