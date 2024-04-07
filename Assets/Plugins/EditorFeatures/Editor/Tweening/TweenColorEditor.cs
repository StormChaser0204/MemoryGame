using Dependencies.ChaserLib.Tweening.Tweens;
using UnityEditor;
using UnityEngine;

namespace Dependencies.ChaserLib.EditorFeatures.Editor.Tweening
{
    [CustomEditor(typeof(TweenColor))]
    public class TweenColorEditor : TweenerEditor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Space(6f);
            EditorTools.SetLabelWidth(120f);

            var tw = target as TweenColor;
            GUI.changed = false;

            var from = EditorGUILayout.ColorField("From", tw.From);
            var to = EditorGUILayout.ColorField("To", tw.To);
            var useCache = EditorGUILayout.Toggle("Use Cache", tw.UseCache);

            if (GUI.changed)
            {
                EditorTools.RegisterUndo("Tween Change", tw);
                tw.From = from;
                tw.To = to;
                tw.UseCache = useCache;
                EditorTools.SetDirty(tw);
            }

            DrawCommonProperties();
        }
    }
}