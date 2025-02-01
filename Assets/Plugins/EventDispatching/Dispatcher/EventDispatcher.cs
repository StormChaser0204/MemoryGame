using System;
using System.Collections.Generic;
using System.Threading;
using Plugins.EventDispatching.Binding;
using Plugins.EventHandler;
using Plugins.EventHandler.Handlers;
using UnityEngine;

namespace Plugins.EventDispatching.Dispatcher
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IDictionary<Type, List<Group>> _handlers = new Dictionary<Type, List<Group>>();

        Binder<EventHandlerBase> IEventDispatcher.Bind() => CreateBinder();

        private Binder<EventHandlerBase> CreateBinder() => new(AddHandlersGroup);

        private void AddHandlersGroup(Group group, Type signalType)
        {
            if (_handlers.ContainsKey(signalType))
                _handlers[signalType].Add(group);
            else
                _handlers.Add(signalType, new List<Group> { group });
        }

        void IEventDispatcher.Raise(IEvent ev)
        {
            var type = ev.GetType();
            var groups = _handlers[type].ToArray();
            if (groups.Length == 0)
            {
                if (Debug.isDebugBuild)
                    Debug.LogWarning($"Handler is missing for the signal: {type}.");

                return;
            }

            Process(ev, groups);
        }

        private void Process(IEvent @event, IEnumerable<Group> groups)
        {
            foreach (var group in groups)
            foreach (var handler in group.Handlers)
                ProcessSignal(@event, handler);
        }

        private void ProcessSignal(IEvent @event, Type handlerType)
        {
            var handlerBase = CreateHandler(@event, handlerType);
            switch (handlerBase)
            {
                case IHandler handler:
                {
                    handler.Handle();
                    break;
                }
                case ITaskHandler taskHandler:
                {
                    RunAndForget(taskHandler, default);
                    break;
                }
            }
        }

        private EventHandlerBase CreateHandler(IEvent @event, Type handlerType)
        {
            var handler = (EventHandlerBase) Activator.CreateInstance(handlerType, @event);
            return handler;
        }

        private static async void RunAndForget(ITaskHandler taskHandler,
            CancellationToken cancellationToken) =>
            await taskHandler.Handle(cancellationToken);
    }
}