namespace Plugins.EventHandler.Handlers
{
    public abstract class EventHandlerBase
    {
        private readonly IEvent _ev;

        protected EventHandlerBase(IEvent ev) => _ev = ev;

        protected T EventAs<T>() where T : class => _ev as T;

        protected bool EventIs<T>() => _ev is T;
    }
}