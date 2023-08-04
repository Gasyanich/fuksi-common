using EasyNetQ;
using Fuksi.Common.Queue.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Fuksi.Common.Queue.DI;

public static class ServiceCollectionQueueExtensions
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, string connectionString)
    {
        services.RegisterEasyNetQ(connectionString, register => register.EnableSystemTextJson());

        services.AddTransient<IEventQueue, EventQueue>();

        return services;
    }
}