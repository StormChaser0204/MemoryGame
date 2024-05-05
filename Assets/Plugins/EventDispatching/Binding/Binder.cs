using System;
using Plugins.EventHandler;

namespace Plugins.EventDispatching.Binding
{
    public class Binder<TEventHandlerBase>
    {
        private Group Group { get; set; } = new();
        private Type SignalType { get; set; }

        private readonly Action<Group, Type> _action;

        internal Binder(Action<Group, Type> adder) => _action = adder;

        public Binder<TEventHandlerBase> Handler<T>()
            where T : TEventHandlerBase
        {
            Add(typeof(T));
            return this;
        }
        
        private void Add(Type type) => Group.Add(type);

        public void To<T>() where T : IEvent
        {
            SignalType = typeof(T);
            Finish(SignalType);
        }

        private void Finish(Type eventType) => _action(Group, eventType);
    }
}