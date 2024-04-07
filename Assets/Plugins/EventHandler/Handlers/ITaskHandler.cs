using System.Threading;
using Cysharp.Threading.Tasks;

namespace Plugins.EventHandler.Handlers
{
    public interface ITaskHandler
    {
        UniTask Handle(CancellationToken cancellationToken = default);
    }
}