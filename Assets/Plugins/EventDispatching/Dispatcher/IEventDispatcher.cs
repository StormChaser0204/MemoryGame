using Plugins.EventDispatching.Binding;
using Plugins.EventHandler;
using Plugins.EventHandler.Handlers;

namespace Plugins.EventDispatching.Dispatcher
{
    public interface IEventDispatcher
    {
        Binder<EventHandlerBase> Bind();
        void Raise(IEvent ev);
    }
}