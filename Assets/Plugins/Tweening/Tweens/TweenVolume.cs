using UnityEngine;

namespace Dependencies.ChaserLib.Tweening.Tweens
{
    [RequireComponent(typeof(AudioSource))]
    [AddComponentMenu("ChaserLib/Tween/TweenVolume")]
    public class TweenVolume : Tweener
    {
        [Range(0f, 1f)] public float From = 1f;
        [Range(0f, 1f)] public float To = 1f;

        private AudioSource _source;

        public AudioSource AudioSource
        {
            get
            {
                if (_source != null)
                    return _source;

                _source = GetComponent<AudioSource>();

                if (_source != null)
                    return _source;

                Debug.LogError("TweenVolume needs an AudioSource to work with", this);
                enabled = false;

                return _source;
            }
        }

        public float Value
        {
            get => AudioSource != null ? _source.volume : 0f;
            set
            {
                if (AudioSource != null)
                    _source.volume = value;
            }
        }

        protected override void OnUpdate(float factor, bool isFinished)
        {
            Value = From * (1f - factor) + To * factor;
            _source.enabled = _source.volume > 0.01f;
        }

        public static TweenVolume Begin(GameObject go, float duration, float targetVolume)
        {
            var comp = Begin<TweenVolume>(go, duration);
            comp.From = comp.Value;
            comp.To = targetVolume;

            if (targetVolume > 0f)
                return comp;

            var s = comp.AudioSource;
            s.enabled = true;
            s.Play();

            return comp;
        }

        public override void SetStartToCurrentValue() => From = Value;
        public override void SetEndToCurrentValue() => To = Value;
    }
}