using System;
using UnityEngine.SceneManagement;

namespace Dependencies.ChaserLib.Tasks
{
    public class ExecuteOnSceneSwitchedOnce
    {
        private Action _action;

        public ExecuteOnSceneSwitchedOnce() => SceneManager.activeSceneChanged += OnSceneChanged;

        public ExecuteOnSceneSwitchedOnce(Action action) : this() => SetAction(action);

        public void SetAction(Action action) => _action = action;

        private void OnSceneChanged(Scene arg0, Scene arg1)
        {
            _action?.Invoke();
            Unsub();
        }

        private void Unsub() => SceneManager.activeSceneChanged -= OnSceneChanged;

        public void Cancel() => Unsub();
    }
}