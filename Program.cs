using System.Threading.Tasks;
using GettingStartedServer.Consumers;
using MassTransit;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace GettingStartedServer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services.AddMassTransit(x =>
                    {
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
                                e.UseNewtonsoftRawJsonDeserializer();
                            
                                e.Consumer<CompassConsumer>();
                                e.Bind("my.existing.exchange", configurator =>
                                {
                                    configurator.ExchangeType = ExchangeType.Direct;
                                });
                          
                            });
                            
                            cfg.ConfigureEndpoints(context);
                        });
                    });
                });
    }
}
