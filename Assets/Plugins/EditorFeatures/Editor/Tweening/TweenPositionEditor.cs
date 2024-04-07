using Dependencies.ChaserLib.Tweening.Tweens;
using UnityEditor;
using UnityEngine;

namespace Dependencies.ChaserLib.EditorFeatures.Editor.Tweening
{
    [CustomEditor(typeof(TweenPosition))]
    public class TweenPositionEditor : TweenerEditor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Space(6f);
            EditorTools.SetLabelWidth(120f);

            var tw = target as TweenPosition;
            GUI.changed = false;

            var from = EditorGUILayout.Vector3Field("From", tw.From);
            var to = EditorGUILayout.Vector3Field("To", tw.To);
            var worldSpace = EditorGUILayout.Toggle("World space", tw.WorldSpace);

            if (GUI.changed)
            {
                EditorTools.RegisterUndo("Tween Change", tw);
                tw.From = from;
                tw.To = to;
                tw.WorldSpace = worldSpace;
                EditorTools.SetDirty(tw);
            }

            DrawCommonProperties();
        }
    }
}