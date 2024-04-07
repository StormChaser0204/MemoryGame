using UnityEditor;
using UnityEngine;

namespace Dependencies.ChaserLib.Unity.Editor
{
    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var sceneName = property.FindPropertyRelative("SceneName");
            var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(sceneName.stringValue);

            EditorGUI.BeginChangeCheck();
            var newScene = EditorGUI.ObjectField(position, label, oldScene, typeof(SceneAsset), false);

            if (!EditorGUI.EndChangeCheck())
                return;

            sceneName.stringValue = AssetDatabase.GetAssetPath(newScene);
        }
    }
}