namespace Fuksi.Common.Queue;

public class PublishMessageInfo
{
    /// <summary>
    /// Имя Exchange. По умолчанию полное имя типа + _Exchange
    /// <example>Fuksi.Vk.SendMessageToUser_Exchange</example>
    /// </summary>
    public string? Exchange { get; set; }

    /// <summary>
    /// Тип Exchange. По умолчанию topic
    /// </summary>
    public string? ExchangeType { get; set; } = "topic";

    /// <summary>
    /// Routing Key. По умолчанию #
    /// </summary>
    public string? RoutingKey { get; set; } = "#";
}