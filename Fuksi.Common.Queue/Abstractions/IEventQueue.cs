using Fuksi.Common.Queue.Consumer;

namespace Fuksi.Common.Queue.Abstractions;

public interface IEventQueue
{
    /// <summary>
    /// Подписаться на сообщение
    /// </summary>
    /// <param name="subscriptionId">Id подписки (полное имя сервиса)</param>
    /// <param name="subscriptionInfo">Настройки подписки</param>
    /// <typeparam name="TMessage">Тип класса Сообщения</typeparam>
    /// <typeparam name="TConsumer">Консьюмер</typeparam>
    Task<IDisposable> Subscribe<TMessage, TConsumer>(string subscriptionId, SubscriptionInfo subscriptionInfo)
        where TMessage : class
        where TConsumer : IConsumer<TMessage>;

    /// <summary>
    /// Опубликовать сообщение
    /// </summary>
    /// <param name="message">Сообщение</param>
    /// <param name="publishMessageInfo">Параметры публикации</param>
    /// <typeparam name="TMessage">Тип класса Сообщения</typeparam>
    /// <returns></returns>
    Task Publish<TMessage>(TMessage message, PublishMessageInfo publishMessageInfo);
}