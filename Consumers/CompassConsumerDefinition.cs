using System;
using MassTransit;
using MassTransit.Configuration;
using RabbitMQ.Client;

namespace GettingStarted.Consumers;

public class CompassConsumerDefinition : ConsumerDefinition<CompassConsumer>
{
    // public CompassConsumerDefinition()
    // {
    //     //EndpointName = "my.existing.q";
    //     // IEndpointSettings<IEndpointDefinition<CompassConsumer>> settings =
    //     //     new EndpointSettings<IEndpointDefinition<CompassConsumer>>{ConfigureConsumeTopology = false, Name = "my.existing.exchange"};
    //     // EndpointDefinition = new ConsumerEndpointDefinition<CompassConsumer>(settings);
    //     //EndpointName = "my.existing.q";
    //     // var settings = new EndpointSettings<IEndpointDefinition<CompassConsumer>>();
    // }
    
    // protected override void ConfigureConsumer(
    //     IReceiveEndpointConfigurator endpointConfigurator, 
    //     IConsumerConfigurator<CompassConsumer> consumerConfigurator)
    // {
    //     endpointConfigurator.ConfigureConsumeTopology = false;
    //     endpointConfigurator.UseRawJsonDeserializer(isDefault:true);
    //     
    //     // if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rabbit)
    //     // {
    //     //     rabbit.Bind("my.existing.exchange", x =>
    //     //     {
    //     //         x.ExchangeType = ExchangeType.Direct;
    //     //     });
    //     //     
    //     //     rabbit.BindQueue = true;
    //     // }
    // }
}