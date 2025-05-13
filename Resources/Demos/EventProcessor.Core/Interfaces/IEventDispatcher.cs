namespace EventProcessor.Core.Interfaces
{
    public interface IEventDispatcher
    {
        Task DispatchAsync(object eventData);
    }
}
