using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Loading
{
    [AddComponentMenu("Loading/Loading Bootstrapper")]
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private Animator _loadAnimation;
        [SerializeField] private Slider _loadProgress;

        private const string GameSceneName = "Game";
        private const string AnimatorLoadStateName = "Load";

        public async void Start()
        {
            //TODO: Add smooth change for progress bar loader 
            //TODO: YandexGames initialization

            _loadAnimation.Play(AnimatorLoadStateName);
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            var sceneLoader = SceneManager.LoadSceneAsync(GameSceneName);
            while (!sceneLoader.isDone)
            {
                _loadProgress.value = sceneLoader.progress;
                await UniTask.Yield();
            }
        }
    }
}