using System.Threading;
using Cysharp.Threading.Tasks;

namespace Plugins.EventHandler.Handlers
{
    public abstract class TaskEventHandler : EventHandlerBase, ITaskHandler
    {
        protected TaskEventHandler(IEvent ev) : base(ev)
        {
        }

        public abstract UniTask Handle(CancellationToken cancellationToken = default);
    }
}