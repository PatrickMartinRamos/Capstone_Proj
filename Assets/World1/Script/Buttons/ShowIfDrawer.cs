using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute attr = (ShowIfAttribute)attribute;
        SerializedProperty condition = property.serializedObject.FindProperty(attr.conditionName);

        return (condition != null && condition.boolValue)
            ? EditorGUI.GetPropertyHeight(property, label, true)
            : 0f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute attr = (ShowIfAttribute)attribute;
        SerializedProperty condition = property.serializedObject.FindProperty(attr.conditionName);

        if (condition != null && condition.boolValue)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}
#endif