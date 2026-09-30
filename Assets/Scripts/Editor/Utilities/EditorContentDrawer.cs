using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Game.Sorcerum
{
    public static class EditorContentDrawer
    {
        //this method rebuilds the fields of serializable classes and structs. Not objects like scriptable object or mono-behaviours.
        public static void RebuildFields(VisualElement fieldsContainer, SerializedProperty property)
        {
            fieldsContainer.Clear();

            // Eğer property'e bir nesne atanmamışsa, çizecek bir şey yok.
            var fieldType = property.GetFieldType();

            // var isScriptableObject = fieldType != null &&
            //                          typeof(ScriptableObject).IsAssignableFrom(fieldType);
            //
            // if (isScriptableObject)
            // {
            //     fieldsContainer.Add(new InlineScriptableObjectField("test", property));
            //     return;
            // }
            
            if (property.managedReferenceValue == null)
                return;

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                // 2. Check if the assigned value is a ScriptableObject
                if (property.objectReferenceValue is ScriptableObject)
                {
                    Debug.Log("This property currently holds a ScriptableObject!");
                }
            }

            DrawProperties(fieldsContainer, property);
        }

        private static void DrawProperties(VisualElement fieldsContainer, SerializedProperty property)
        {
            var endProperty = property.GetEndProperty();
        
            var iterator = property.Copy();
        
            iterator.NextVisible(true);
        
            while (!SerializedProperty.EqualContents(iterator, endProperty))
            {
                var propField = new PropertyField(iterator.Copy());
                
                fieldsContainer.Add(propField);
        
                if (!iterator.NextVisible(false))
                {
                    break;
                }
            }
        }


        public static void LoopVariables(Object obj, Action<SerializedProperty> propertyAction)
        {
            var serializedObj = new SerializedObject(obj);

            var iterator = serializedObj.GetIterator().Copy();
            
            if(!iterator.NextVisible(true))
                return;
            
            while (iterator.NextVisible(false))
                propertyAction.Invoke(iterator.Copy());    
            
            if (serializedObj.hasModifiedProperties)
                serializedObj.ApplyModifiedProperties();
        }
        

        public static bool IsList(Type type)
        {
            return type.IsArray || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }
        
        public static Type GetElementType(Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                return type.GetGenericArguments()[0];
            }

            return type;
        }
    }
    
    public static class SerializedPropertyExtensions
    {
        public static Type GetFieldType(this SerializedProperty property)
        {
            Type parentType = property.serializedObject.targetObject.GetType();
            FieldInfo fi = parentType.GetField(property.propertyPath,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            return fi?.FieldType;
        }
    }
}