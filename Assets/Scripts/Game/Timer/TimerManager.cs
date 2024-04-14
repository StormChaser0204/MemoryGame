using UnityEngine;

namespace Game.Timer
{
    internal class TimerManager : MonoBehaviour
    {
        private TimerView _view;
        private float _currentTime;
        private bool _isActive;

        public void BindView(TimerView view) => _view = view;

        private void Update()
        {
            if (!_isActive)
                return;

            _currentTime += Time.deltaTime;
            _view.UpdateTimer(_currentTime);
        }

        public void Pause() => _isActive = false;

        public void Resume() => _isActive = true;

        public void Restart()
        {
            _currentTime = 0;
            _isActive = true;
        }

        public float GetCurrentTime() => _currentTime;
    }
}