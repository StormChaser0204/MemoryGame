using Dependencies.ChaserLib.Tweening.Tweens;
using UnityEditor;
using UnityEngine;

namespace Dependencies.ChaserLib.EditorFeatures.Editor.Tweening
{
    [CustomEditor(typeof(Tweener), true)]
    public class TweenerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Space(6f);
            EditorTools.SetLabelWidth(110f);
            base.OnInspectorGUI();
            DrawCommonProperties();
        }

        protected void DrawCommonProperties()
        {
            var tw = target as Tweener;

            if (EditorTools.DrawHeader("Tweener"))
            {
                EditorTools.BeginContents();
                EditorTools.SetLabelWidth(110f);

                GUI.changed = false;

                var note = EditorGUILayout.TextField("Note", tw.Note);
                var style = (Tweener.StyleType) EditorGUILayout.EnumPopup("Play Style", tw.Style);
                var curve = EditorGUILayout.CurveField("Animation Curve", tw.AnimationCurve,
                    GUILayout.Width(170f),
                    GUILayout.Height(62f));

                GUILayout.BeginHorizontal();
                var dur = EditorGUILayout.FloatField("Duration", tw.Duration, GUILayout.Width(170f));
                GUILayout.Label("seconds");
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                var del = EditorGUILayout.FloatField("Start Delay", tw.Delay, GUILayout.Width(170f));
                GUILayout.Label("seconds");
                GUILayout.EndHorizontal();

                var tg = EditorGUILayout.IntField("Tween Group", tw.TweenGroup, GUILayout.Width(170f));
                var ts = EditorGUILayout.Toggle("Ignore TimeScale", tw.IgnoreTimeScale);
                var fx = EditorGUILayout.Toggle("Use Fixed Update", tw.UseFixedUpdate);

                if (GUI.changed)
                {
                    EditorTools.RegisterUndo("Tween Change", tw);
                    tw.AnimationCurve = curve;
                    tw.Style = style;
                    tw.IgnoreTimeScale = ts;
                    tw.TweenGroup = tg;
                    tw.Duration = dur;
                    tw.Delay = del;
                    tw.UseFixedUpdate = fx;
                    tw.Note = note;
                    EditorTools.SetDirty(tw);
                }

                EditorTools.EndContents();
            }

            EditorTools.SetLabelWidth(80f);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnFinished"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}