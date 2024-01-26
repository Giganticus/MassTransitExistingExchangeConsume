using System;
using System.Threading.Tasks;
using GettingStarted.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace GettingStarted.Consumers;

public class CompassConsumer : IConsumer<JObject>
{
    private readonly ILogger<CompassConsumer> _logger;

    public CompassConsumer()
    {
        var factory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Debug);
        });
         _logger = factory.CreateLogger<CompassConsumer>();
    }
    
    public Task Consume(ConsumeContext<JObject> context)
    {
        dynamic d = context.Message;
        d.SomeExtraValueName = "someExtraValueData";
        JObject returnValue = JObject.FromObject(d);
        
        if (context.Headers.TryGetHeader("FLOW", out var flowName))
        {
            _logger.LogDebug($"Received workflow request for workflow '{flowName}'" +
                              $"{Environment.NewLine}Payload:" +
                              $"{Environment.NewLine}{JsonSerializer.Serialize(context.Message)}");
        }
        else
        {
            _logger.LogDebug($"Received workflow request without FLOW header..." +
                             $"{Environment.NewLine}Payload:" +
                             $"{Environment.NewLine}{JsonSerializer.Serialize(context.Message)}");
             
        }


        return context.RespondAsync(returnValue);
        // return context.RespondAsync(new WorkflowResponse
        // {
        //     compass = new Compass
        //     {
        //         correlationId = 500000002743176,
        //         executionId = 12262023,
        //         status = new Status
        //         {
        //             message = "Flow Started",
        //             statusId = 8,
        //             statusName = "Running"
        //         }
        //
        //     }
        // });
    }
}