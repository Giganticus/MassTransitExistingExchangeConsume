using System;
using System.Reflection;
using System.Threading.Tasks;
using GettingStarted.Consumers;
using GettingStarted.Contracts;
using Microsoft.Extensions.Hosting;
using MassTransit;
using MassTransit.RabbitMqTransport.Topology;
using MassTransit.Topology;
//using MassTransit.Transports.Fabric;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace GettingStarted
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        var entryAssembly = Assembly.GetEntryAssembly();
                        
                      //  x.AddConsumers(entryAssembly);
                        
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host("localhost", "/", h =>
                            {
                                h.Username("guest");
                                h.Password("guest");
                            });
                           
                            cfg.ReceiveEndpoint("input-queue", e =>
                            {
                                e.ConfigureConsumeTopology = false;
                                e.UseRawJsonDeserializer(isDefault:true);
                                e.Consumer<CompassConsumer>();
                                e.Bind("my.existing.exchange", configurator =>
                                {
                                    configurator.ExchangeType = ExchangeType.Direct;
                                });
                                //e.Bind<WorkflowRequest>();
                            });
                            
                            cfg.ConfigureEndpoints(context);
                            
                            // cfg.ReceiveEndpoint("my.existing.q", configurator =>
                            // { configurator.ExchangeType = ExchangeType.Direct;
                            //     // configurator.Bind("my.existing.exchange", rabbitMqExchangeToExchangeBindingConfigurator =>
                            //     // {
                            //     //     rabbitMqExchangeToExchangeBindingConfigurator.ExchangeType = ExchangeType.Direct;
                            //     // });
                            // });
                        });

                        
                        // x.AddSagaStateMachines(entryAssembly);
                        // x.AddSagas(entryAssembly);
                        // x.AddActivities(entryAssembly);

                        // x.UsingInMemory((context, cfg) =>
                        // {
                        //     cfg.ConfigureEndpoints(context);
                        // });
                        
                        //x.AddRequestClient<WorkflowRequest>(new Uri("exchange:my.existing.exchange?type=direct"));
                    });
                   // services.AddHostedService<Worker>();
                   //services.AddHostedService<WorkflowRequester>();
                });
    }
}
