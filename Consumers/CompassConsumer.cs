using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace GettingStartedServer.Consumers;

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
        if (context.Headers.TryGetHeader("FLOW", out var flowName))
        {
            _logger.LogDebug($"Received workflow request for workflow '{flowName}'" +
                              $"{Environment.NewLine}Payload:" +
                              $"{Environment.NewLine}{context.Message}");
        }
        else
        {
            _logger.LogDebug($"Received workflow request without FLOW header..." +
                             $"{Environment.NewLine}Payload:" +
                             $"{Environment.NewLine}{context.Message}");
             
        }
        
        dynamic d = context.Message;
        d.SomeExtraValueName = "someExtraValueData";
        JObject returnValue = JObject.FromObject(d);
        
        _logger.LogDebug($"Now with extra field:{returnValue}");
        
        return context.RespondAsync(returnValue);
    }
}