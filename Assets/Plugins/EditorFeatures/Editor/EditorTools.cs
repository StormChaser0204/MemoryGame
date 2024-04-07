using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Dependencies.ChaserLib.EditorFeatures.Editor
{
    public static class EditorTools
    {
        public static bool DrawHeader(string text) => DrawHeader(text, text);

        public static bool DrawHeader(string text, string key)
        {
            var state = EditorPrefs.GetBool(key, true);

            GUILayout.Space(3f);
            if (!state)
                GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);

            GUILayout.BeginHorizontal();
            GUI.changed = false;


            text = "<b><size=11>" + text + "</size></b>";
            if (state)
                text = "\u25BC " + text;
            else
                text = "\u25BA " + text;
            if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f)))
                state = !state;


            if (GUI.changed)
                EditorPrefs.SetBool(key, state);

            GUILayout.Space(2f);
            GUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;
            if (!state)
                GUILayout.Space(3f);

            return state;
        }

        private static bool _mEndHorizontal;

        public static string TextArea = "TextArea";

        public static void BeginContents()
        {
            _mEndHorizontal = true;
            GUILayout.BeginHorizontal();
            EditorGUILayout.BeginHorizontal(TextArea, GUILayout.MinHeight(10f));

            GUILayout.BeginVertical();
            GUILayout.Space(2f);
        }

        public static void EndContents()
        {
            GUILayout.Space(3f);
            GUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            if (_mEndHorizontal)
            {
                GUILayout.Space(3f);
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(3f);
        }
        
        public static void SetLabelWidth(float width) => EditorGUIUtility.labelWidth = width;

        public static void RegisterUndo(string name, Object obj)
        {
            if (obj != null)
                Undo.RecordObject(obj, name);
        }

        public static Object LoadAsset(string path) =>
            string.IsNullOrEmpty(path) ? null : AssetDatabase.LoadMainAssetAtPath(path);

        public static T LoadAsset<T>(string path) where T : Object
        {
            var obj = LoadAsset(path);
            if (obj == null)
                return null;

            var val = obj as T;
            if (val != null)
                return val;

            if (!typeof(T).IsSubclassOf(typeof(Component)))
                return null;

            if (obj.GetType() != typeof(GameObject))
                return null;

            var go = obj as GameObject;
            return go.GetComponent(typeof(T)) as T;
        }

        public static void DrawPadding() => GUILayout.Space(18f);

        public static void SetDirty(Object obj)
        {
            if (!obj)
                return;

            EditorUtility.SetDirty(obj);

            if (AssetDatabase.Contains(obj) || Application.isPlaying)
                return;

            if (obj is Component component)
                EditorSceneManager.MarkSceneDirty(component.gameObject.scene);
            else if (!(obj is EditorWindow || obj is ScriptableObject))
                EditorSceneManager.MarkAllScenesDirty();
        }

        public static SerializedProperty DrawProperty(string label, SerializedObject serializedObject, string property)
        {
            var sp = serializedObject.FindProperty(property);

            if (sp != null)
            {
                if (sp.isArray && sp.type != "string")
                    DrawArray(serializedObject, property, label ?? property);
                else if (label != null)
                    EditorGUILayout.PropertyField(sp, new GUIContent(label));
                else
                    EditorGUILayout.PropertyField(sp);
            }
            else
            {
                Debug.LogWarning("Unable to find property " + property);
            }

            return sp;
        }

        public static void DrawArray(SerializedObject obj, string property, string title)
        {
            var sp = obj.FindProperty(property + ".Array.size");

            if (sp == null || !DrawHeader(title))
                return;

            BeginContents();
            var size = sp.intValue;
            var newSize = EditorGUILayout.IntField("Size", size);
            if (newSize != size)
                obj.FindProperty(property + ".Array.size").intValue = newSize;

            EditorGUI.indentLevel = 1;

            for (var i = 0; i < newSize; i++)
            {
                var p = obj.FindProperty($"{property}.Array.data[{i}]");
                if (p != null)
                    EditorGUILayout.PropertyField(p);
            }

            EditorGUI.indentLevel = 0;
            EndContents();
        }
    }
}