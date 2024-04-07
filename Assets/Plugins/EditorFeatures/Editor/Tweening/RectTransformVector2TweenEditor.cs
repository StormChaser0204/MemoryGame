using System;
using Dependencies.ChaserLib.Tweening.Tweens;
using UnityEditor;
using UnityEngine;

namespace Dependencies.ChaserLib.EditorFeatures.Editor.Tweening
{
    [CustomEditor(typeof(RectTransformVector2Tween))]
    public class RectTransformVector2TweenEditor : TweenerEditor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Space(6f);
            EditorTools.SetLabelWidth(120f);

            var tw = target as RectTransformVector2Tween;
            GUI.changed = false;

            var comp = (RectTransformVector2Tween.Component) EditorGUILayout.EnumPopup(
                "Vector's component", tw.VectorComponent);
           
            var from = tw.From;
            var to = tw.To;
            switch (comp)
            {
                case RectTransformVector2Tween.Component.Both:
                    from = EditorGUILayout.Vector2Field("From", tw.From);
                    to = EditorGUILayout.Vector2Field("To", tw.To);
                    break;
                case RectTransformVector2Tween.Component.X:
                    from.x = FloatField("From", tw.From.x);
                    to.x = FloatField("To", tw.To.x);
                    break;
                case RectTransformVector2Tween.Component.Y:
                    from.y = FloatField("From", tw.From.y);
                    to.y = FloatField("To", tw.To.y);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (GUI.changed)
            {
                EditorTools.RegisterUndo("Tween Change", tw);
                tw.From = from;
                tw.To = to;
                tw.VectorComponent = comp;
                EditorTools.SetDirty(tw);
            }

            DrawCommonProperties();
        }

        private static float FloatField(string title, float value) => EditorGUILayout.FloatField(title, value);
    }
}