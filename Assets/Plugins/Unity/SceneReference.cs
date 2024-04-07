using System;
using UnityEngine.SceneManagement;

namespace Dependencies.ChaserLib.Unity
{
    /// Nice way to avoid direct references to scenes.
    /// Allows to avoid preload of each scene and its content on app start 
    [Serializable]
    public class SceneReference
    {
        #region Operators

        public static bool operator ==(Scene scene, SceneReference selector) =>
            scene.path == selector.SceneName;

        public static bool operator ==(SceneReference selector, Scene scene) =>
            scene.path == selector.SceneName;

        public static bool operator !=(Scene scene, SceneReference selector) =>
            scene.path != selector.SceneName;

        public static bool operator !=(SceneReference selector, Scene scene) =>
            scene.path != selector.SceneName;

        #endregion

        //Assign by PropertyDrawer
        public string SceneName;
        
        private Action _callback;

        public void GoHere() => SceneManager.LoadScene(SceneName);

        public void GoHereAsync() => SceneManager.LoadSceneAsync(SceneName);

        public void GoHereAsync(Action callback)
        {
            _callback = callback;
            SceneManager.sceneLoaded += SceneLoaded;
            SceneManager.LoadSceneAsync(SceneName);
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (_callback != null)
            {
                var r = _callback;
                _callback = null;
                r();
            }

            SceneManager.sceneLoaded -= SceneLoaded;
        }

        public override string ToString() => SceneName;

        public override int GetHashCode() => SceneName.GetHashCode();

        public override bool Equals(object obj)
        {
            if (obj is SceneReference other)
                return other.GetHashCode() == GetHashCode();

            return false;
        }
    }
}