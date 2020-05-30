using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UEGP3.Core;

[CustomPropertyDrawer(typeof(MinMaxFloat))]
public class MinMaxDrawer : PropertyDrawer 
{
    //Draw the Property
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        MinMaxAttribute[] att = (MinMaxAttribute[]) fieldInfo.GetCustomAttributes(typeof(MinMaxAttribute), true);
        if (att.Length <= 0) //requires the attribute
            return;

        //Cache the properties
        var minProperty = property.FindPropertyRelative("min");
        var maxProperty = property.FindPropertyRelative("max");
        //Get the current min and max values.
        float minValue = minProperty.floatValue;
        float maxValue = maxProperty.floatValue;
        
        //the width of the labels that display the current min/max values.
        const float valueLabelWidth = 40;

        //the position of the max value label.
        Rect maxValueArea = new Rect
        {
            x = position.x + position.width - valueLabelWidth,
            y = position.y,
            width = valueLabelWidth,
            height = position.height
        };

        //the position of the min value label.
        Rect minValueArea = new Rect()
        {
            x = position.x + EditorGUIUtility.labelWidth,
            y = position.y,
            width = valueLabelWidth,
            height = position.height
        };

        //Draw the prefix label (essentially the variable display name)
        position = EditorGUI.PrefixLabel(position, label);
        //adjust the width of the minmax slider.
        position.width -= 2 * valueLabelWidth;
        //offset it to not overlap with the minvalue label.
        position.x += valueLabelWidth;
        //Actually do the slider
        EditorGUI.MinMaxSlider(position, new GUIContent(""), ref minValue, ref maxValue, att[0].min, att[0].max);
        //Draw the labels with some neat string formatting on those floats.
        EditorGUI.LabelField(maxValueArea, maxValue.ToString("0.00"));
        EditorGUI.LabelField(minValueArea, minValue.ToString("0.00"));

        //set the values to the serialized object.
        minProperty.floatValue = minValue;
        maxProperty.floatValue = maxValue;

        EditorGUI.EndProperty();
    }
}
