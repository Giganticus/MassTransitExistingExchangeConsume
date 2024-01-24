using System;
using System.Text.Json;
using System.Threading.Tasks;
using GettingStarted.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace GettingStarted.Consumers;

public class CompassConsumer : IConsumer<WorkflowRequest>
{
    private readonly ILogger<CompassConsumer> _logger;

    public CompassConsumer()
    {
    }
    
    public CompassConsumer(ILogger<CompassConsumer> logger)
    {
        _logger = logger;
    }
    
    public Task Consume(ConsumeContext<WorkflowRequest> context)
    {
        if (context.Headers.TryGetHeader("FLOW", out var flowName))
        {
            _logger.LogInformation(
                $"Received workflow request for workflow '{flowName}'" +
                $"{Environment.NewLine}Payload:" +
                $"{Environment.NewLine}{JsonSerializer.Serialize(context.Message)}");
        }
        else
        {
            _logger.LogInformation( $"Received workflow request without FLOW header..." +
                                   $"{Environment.NewLine}Payload:" +
                                   $"{Environment.NewLine}{JsonSerializer.Serialize(context.Message)}"); 
        }

        // context.Publish<WorkflowResponse>(
        //     new WorkflowResponse
        //     {
        //         compass = new Compass
        //         {
        //             correlationId = 500000002743176,
        //             executionId = 12262023,
        //             status = new Status
        //             {
        //                 message = "Flow Started",
        //                 statusId = 8,
        //                 statusName = "Running"
        //             }
        //         }
        //     });
        return context.RespondAsync(new WorkflowResponse
        {
            compass = new Compass
            {
                correlationId = 500000002743176,
                executionId = 12262023,
                status = new Status
                {
                    message = "Flow Started",
                    statusId = 8,
                    statusName = "Running"
                }
        
            }
        });
        
        return Task.CompletedTask;
    }
}