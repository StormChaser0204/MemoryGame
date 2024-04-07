using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dependencies.ChaserLib.Timer
{
    public class TimerManager : MonoBehaviour
    {
        public static TimerManager Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                _instance = (TimerManager) FindAnyObjectByType(typeof(TimerManager))
                            ?? (new GameObject(nameof(TimerManager))).AddComponent<TimerManager>();

                DontDestroyOnLoad(_instance.gameObject);
                return _instance;
            }
        }

        private static TimerManager _instance;

        private readonly TimersQueue _updateQueue = new TimersQueue();
        private readonly TimersQueue _fixedUpdateQueue = new TimersQueue();

        private TimerManager() => SceneManager.sceneUnloaded += OnSceneUnload;

        private void Update() => _updateQueue.Tick();

        private void FixedUpdate() => _fixedUpdateQueue.Tick();

        public void Add(FrostTimer timer) => _updateQueue.Add(timer);

        public void AddFixed(FrostTimer timer) => _fixedUpdateQueue.Add(timer);

        private void OnSceneUnload(Scene scene)
        {
            DisposeByFlag(_updateQueue);
            DisposeByFlag(_fixedUpdateQueue);
        }

        private static void DisposeByFlag(TimersQueue queue) => queue.ForEachTimer(t =>
        {
            if (t.DisposedOnSceneUnload)
                t.StopAndDispose();
        });

        public static void DisposeAll()
        {
            Instance._updateQueue.Clear();
            Instance._fixedUpdateQueue.Clear();
        }

        private class TimersQueue
        {
            private readonly List<FrostTimer> _timers = new List<FrostTimer>();
            private readonly List<FrostTimer> _timersToAdd = new List<FrostTimer>();

            public void Add(FrostTimer timer) => _timersToAdd.Add(timer);

            public void Tick()
            {
                RemoveDisposed();
                AddNewTimers();
                ResendTick();
            }

            private void AddNewTimers()
            {
                if (_timersToAdd.Count == 0)
                    return;

                _timers.AddRange(_timersToAdd.Where(t => !t.ShouldBeDisposed));
                _timersToAdd.Clear();
            }

            private void ResendTick()
            {
                foreach (var timer in _timers)
                    timer.Tick();
            }

            private void RemoveDisposed() => _timers.RemoveAll(timer => timer.ShouldBeDisposed);

            public void Clear()
            {
                _timersToAdd.Clear();
                _timers.Clear();
            }

            public void ForEachTimer(Action<FrostTimer> action)
            {
                foreach (var timer in _timers)
                    action.Invoke(timer);

                foreach (var timer in _timersToAdd)
                    action.Invoke(timer);
            }
        }
    }
}