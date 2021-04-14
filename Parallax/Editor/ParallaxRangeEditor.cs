using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Range))]

public class ParallaxRangeEditor : PropertyDrawer
{
    public int value;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Range range = (Range)base.attribute;
        if (property.propertyType == SerializedPropertyType.Integer)
        {
            value = EditorGUI.IntSlider(position, label, property.intValue, range.min, range.max);
            value -= (value % (int)Mathf.Round(range.step));

            property.intValue = value;
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use Range with int.");
        }
    }
}
