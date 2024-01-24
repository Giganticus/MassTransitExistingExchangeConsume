using MassTransit;

namespace GettingStarted.Contracts;


// [ExcludeFromTopology]
[EntityName("my.existing.exchange")]
public class WorkflowRequest
{
    public string Payload { get; init; }
}