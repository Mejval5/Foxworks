using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Foxworks.Attributes
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ButtonAttribute))]
    public class ButtonDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string methodName = (attribute as ButtonAttribute)?.MethodName;
            if (methodName == null)
            {
                return;
            }

            Object target = property.serializedObject.targetObject;
            Type type = target.GetType();

            MethodInfo method = type.GetMethod(methodName);
            if (method == null)
            {
                GUI.Label(position, "Method could not be found. Is it public?");
                return;
            }

            if (method.GetParameters().Length > 0)
            {
                GUI.Label(position, "Method cannot have parameters.");
                return;
            }

            if (GUI.Button(position, method.Name))
            {
                method.Invoke(target, null);
            }
        }
    }
#endif

    public class ButtonAttribute : PropertyAttribute
    {
        public ButtonAttribute(string methodName)
        {
            MethodName = methodName;
        }

        public string MethodName { get; }
    }
}