using System.Threading;
using FrostLib.Signals.impl;

namespace Dependencies.ChaserLib.Tasks
{
    public interface ICancellationTokenFactory
    {
        CancellationToken GetSceneSwitchedToken();
        CancellationToken GetAppClosingToken();
        CancellationToken GetDialogClosingToken(Signal onClosedSignal);
    }
}