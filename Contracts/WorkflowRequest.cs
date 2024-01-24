using MassTransit;

namespace GettingStarted.Contracts;

[EntityName("my.existing.exchange")]
public class WorkflowRequest
{
    public string Payload { get; init; }
}