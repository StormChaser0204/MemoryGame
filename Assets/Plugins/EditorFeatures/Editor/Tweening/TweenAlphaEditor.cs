using Dependencies.ChaserLib.Tweening.Tweens;
using UnityEditor;
using UnityEngine;

namespace Dependencies.ChaserLib.EditorFeatures.Editor.Tweening
{
    [CustomEditor(typeof(TweenAlpha))]
    public class TweenAlphaEditor : TweenerEditor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Space(6f);
            EditorTools.SetLabelWidth(120f);

            var tw = target as TweenAlpha;
            GUI.changed = false;

            var from = EditorGUILayout.Slider("From", tw.From, 0f, 1f);
            var to = EditorGUILayout.Slider("To", tw.To, 0f, 1f);

            if (GUI.changed)
            {
                EditorTools.RegisterUndo("Tween Change", tw);
                tw.From = from;
                tw.To = to;
                EditorTools.SetDirty(tw);
            }

            DrawCommonProperties();
        }
    }
}