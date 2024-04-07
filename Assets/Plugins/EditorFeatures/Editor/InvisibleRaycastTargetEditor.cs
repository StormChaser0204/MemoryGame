using Dependencies.ChaserLib.UI;
using UnityEditor;
using UnityEditor.UI;

namespace Dependencies.ChaserLib.EditorFeatures.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(InvisibleRaycastTarget), false)]
    public class InvisibleRaycastTargetEditor : GraphicEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_Script);
            // skipping AppearanceControlsGUI
            RaycastControlsGUI();
            serializedObject.ApplyModifiedProperties();
        }
    }
}