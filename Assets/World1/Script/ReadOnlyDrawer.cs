using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Store previous GUI enabled state
        bool previousGUIState = GUI.enabled;
        // Disable the GUI
        GUI.enabled = false;
        // Draw the property field
        EditorGUI.PropertyField(position, property, label, true);
        // Restore the previous GUI enabled state
        GUI.enabled = previousGUIState;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Ensure the property can be drawn correctly
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
#endif
