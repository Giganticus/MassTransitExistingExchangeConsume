using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GettingStarted.Contracts;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GettingStarted;

// public class Worker(IBus bus) : BackgroundService
// {
//     protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//     {
//         while (!stoppingToken.IsCancellationRequested)
//         {
//             await bus.Publish(new Contracts.GettingStarted
//             {
//                 Value = $"The time is {DateTimeOffset.Now}"
//             }, stoppingToken);
//
//             await Task.Delay(1000, stoppingToken);
//         }
//     }
// }

public class WorkflowRequester(
    IHost host,
    ILogger<WorkflowRequester> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
        
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            
            var requestClient = (IRequestClient<WorkflowRequest>)scope.ServiceProvider.GetService(typeof(IRequestClient<WorkflowRequest>));
            
            if (requestClient == null)
                throw new Exception("Unable to resolve client");
            
            var response = await requestClient.GetResponse<WorkflowResponse>(
                new WorkflowRequest
                {
                    Payload = "hello world"
                }, stoppingToken, timeout: RequestTimeout.None);

            logger.LogInformation($"Received response {JsonSerializer.Serialize(response)}");        
            
            await Task.Delay(1000, stoppingToken);
        }
    }
}