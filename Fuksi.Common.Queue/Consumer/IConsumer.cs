namespace Fuksi.Common.Queue.Consumer;

public interface IConsumer<in TMessage>
{
    Task Consume(TMessage message);
}