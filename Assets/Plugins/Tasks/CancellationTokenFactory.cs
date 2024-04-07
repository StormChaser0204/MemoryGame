using System.Threading;
using FrostLib.Signals.impl;

namespace Dependencies.ChaserLib.Tasks
{
    public class CancellationTokenFactory : ICancellationTokenFactory
    {
        //Without this token UniTask.Yield and similar tasks will not stop when exiting the play mode in Editor
        //So we will link it with any other tokens as well
        private readonly CancellationToken _onAppClosingToken;

        public CancellationTokenFactory(CancellationToken onAppClosingToken) =>
            _onAppClosingToken = onAppClosingToken;

        public CancellationToken GetSceneSwitchedToken()
        {
            var cts = new CancellationTokenSource();
            new ExecuteOnSceneSwitchedOnce(() =>
            {
                if (cts.Token.CanBeCanceled)
                    cts.Cancel();

                cts.Dispose();
            });

            return CancellationTokenSource
                .CreateLinkedTokenSource(
                    cts.Token,
                    GetAppClosingToken())
                .Token;
        }

        public CancellationToken GetAppClosingToken() => _onAppClosingToken;

        public CancellationToken GetDialogClosingToken(Signal onClosedSignal)
        {
            var cts = new CancellationTokenSource();
            onClosedSignal.AddOnce(() => OnDialogClose(cts));
            return cts.Token;
        }


        private static void OnDialogClose(CancellationTokenSource cts)
        {
            if (cts.Token.CanBeCanceled)
                cts.Cancel();

            cts.Dispose();
        }
    }
}