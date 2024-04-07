using Dependencies.ChaserLib.Tools;
using UnityEngine;

namespace Dependencies.ChaserLib.Tweening.Tweens
{
    [AddComponentMenu("ChaserLib/Tween/Spring Position")]
    public class SpringPosition : MonoBehaviour
    {
        public delegate void OnFinishedCallabck();

        public static SpringPosition Current;

        public bool IgnoreTimeScale = false;
        public OnFinishedCallabck OnFinished;
        public float Strength = 10f;
        public Vector3 Target = Vector3.zero;
        public bool WorldSpace = false;

        private Transform _trans;
        private float _threshold;

        private void Start() => _trans = transform;

        private void Update()
        {
            var delta = IgnoreTimeScale ? UnityEngine.Time.unscaledDeltaTime : Time.deltaTime;

            if (WorldSpace)
            {
                if (Mathf.Approximately(_threshold, 0f))
                    _threshold = (Target - _trans.position).sqrMagnitude * 0.001f;
                _trans.position = MathTools.SpringLerp(_trans.position, Target, Strength, delta);

                if (_threshold >= (Target - _trans.position).sqrMagnitude)
                {
                    _trans.position = Target;
                    NotifyListeners();
                    enabled = false;
                }
            }
            else
            {
                if (Mathf.Approximately(_threshold, 0f))
                    _threshold = (Target - _trans.localPosition).sqrMagnitude * 0.00001f;
                _trans.localPosition =
                    MathTools.SpringLerp(_trans.localPosition, Target, Strength, delta);

                if (_threshold >= (Target - _trans.localPosition).sqrMagnitude)
                {
                    _trans.localPosition = Target;
                    NotifyListeners();
                    enabled = false;
                }
            }
        }

        private void NotifyListeners()
        {
            Current = this;
            OnFinished?.Invoke();
            Current = null;
        }

        public static SpringPosition Begin(GameObject go, Vector3 pos, float strength)
        {
            var sp = go.GetComponent<SpringPosition>();
            if (sp == null)
                sp = go.AddComponent<SpringPosition>();

            sp.Target = pos;
            sp.Strength = strength;
            sp.OnFinished = null;
            if (!sp.enabled)
                sp.enabled = true;

            return sp;
        }
    }
}