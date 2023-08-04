using EasyNetQ; 
using Fuksi.Common.Queue.Abstractions;
using Fuksi.Common.Queue.Consumer;
using Microsoft.Extensions.DependencyInjection;

namespace Fuksi.Common.Queue;

internal class EventQueue : IEventQueue
{
    private readonly IAdvancedBus _advancedBus;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public EventQueue(IAdvancedBus advancedBus, IServiceScopeFactory serviceScopeFactory)
    {
        _advancedBus = advancedBus;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Publish<TMessage>(TMessage message, PublishMessageInfo publishMessageInfo)
    {
        var exchangeName = publishMessageInfo.Exchange ?? $"{typeof(TMessage).FullName}_Exchange";
        var exchange = await _advancedBus.ExchangeDeclareAsync(exchangeName, opt =>
        {
            opt.AsDurable(true);
            opt.WithType(publishMessageInfo.ExchangeType);
        });

        var easyNetQMessage = new Message<TMessage>(message);
        await _advancedBus.PublishAsync(exchange, publishMessageInfo.RoutingKey, false, easyNetQMessage);
    }

    public async Task<IDisposable> Subscribe<TMessage, TConsumer>(string subscriptionId, SubscriptionInfo subscriptionInfo)
        where TMessage : class
        where TConsumer : IConsumer<TMessage>
    {
        var exchangeName = subscriptionInfo.Exchange ?? $"{typeof(TMessage).FullName}_Exchange";
        var exchange = await _advancedBus.ExchangeDeclareAsync(exchangeName, opt =>
        {
            opt.AsDurable(true);
            opt.WithType(subscriptionInfo.ExchangeType);
        });

        var queueName = subscriptionInfo.Queue ?? $"{typeof(TMessage).FullName}_{subscriptionId}";
        var queue = await _advancedBus.QueueDeclareAsync(queueName);

        await _advancedBus.BindAsync(exchange, queue, subscriptionInfo.RoutingKey);

        var subscription = _advancedBus.Consume<TMessage>(queue, async (message, _) =>
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var consumer = scope.ServiceProvider.GetRequiredService<TConsumer>();

            await consumer.Consume(message.Body);
        });

        return subscription;
    }
}