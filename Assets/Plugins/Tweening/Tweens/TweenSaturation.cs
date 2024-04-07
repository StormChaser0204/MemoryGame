using UnityEngine;
using UnityEngine.UI;

namespace Dependencies.ChaserLib.Tweening.Tweens
{
    public class TweenSaturation : Tweener
    {
        [Range(0f, 1f)] public float From = 1f;
        [Range(0f, 1f)] public float To = 1f;

        private static readonly int ShaderProperty = Shader.PropertyToID("_SaturationLerpValue");

        private Graphic _graphic;
        private bool _cached;

        public float Value
        {
            get
            {
                if (!_cached)
                    Cache();

                return _graphic.materialForRendering.GetFloat(ShaderProperty);
            }
            set
            {
                if (!_cached)
                    Cache();

                _graphic.materialForRendering.SetFloat(ShaderProperty, value);
            }
        }

        private void Cache()
        {
            _cached = true;

            _graphic = GetComponent<Graphic>();
            var newMaterial = new Material(Shader.Find("ChaserLib/Tweening/UISaturation"))
            {
                name = "UI Saturation Instance"
            };

            _graphic.material = newMaterial;
        }

        protected override void OnUpdate(float factor, bool isFinished) =>
            Value = Mathf.Lerp(From, To, factor);
    }
}