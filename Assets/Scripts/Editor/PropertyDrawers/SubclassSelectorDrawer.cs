using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Sorcerum
{
    [CustomPropertyDrawer(typeof(SubclassSelector))]
    public class SubclassSelectorDrawer : PropertyDrawer
    {
        // public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        // {
        //     EditorGUI.LabelField(EditorGUILayout.GetControlRect(), label.text, ">>> Subclass Selector! <<<");
        // }

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            
            var fieldsContainer = new VisualElement();
            
            var type = EditorContentDrawer.GetElementType(fieldInfo.FieldType);
            
            ImplementationCacheManager.FindAllImplementationsFor(type);
            
            var implementations = ImplementationCacheManager.Implementations[type];
            var names = ImplementationCacheManager.ImplementationNames[type];
            var nameTypeDict = ImplementationCacheManager.NameTypes;
            
            var namesWithNone = new List<string> {"None"};
            namesWithNone.AddRange(names);
            
            int currentIndex = 0;
            
            if (property.managedReferenceValue != null)
                currentIndex = implementations.IndexOf(property.managedReferenceValue.GetType()) + 1;
            
            var dropDown = new DropdownField(namesWithNone, currentIndex){label = property.displayName};
            
            dropDown.RegisterValueChangedCallback(evt =>
            {
                if(property == null)
                    return;
                
                int selectedIndex = namesWithNone.IndexOf(evt.newValue);
                
                if(selectedIndex == 0)
                {
                    property.managedReferenceValue = null;
                    property.serializedObject.ApplyModifiedProperties();
                    return;
                }

                var newType = nameTypeDict[evt.newValue];
            
                if (newType != null)
                {
                    // Yeni bir instance oluştur ve property'e ata
                    property.managedReferenceValue = Activator.CreateInstance(newType);
                    property.serializedObject.ApplyModifiedProperties();
            
                    // Değişiklik sonrası UI'ı yeniden oluştur
                    EditorContentDrawer.RebuildFields(fieldsContainer, property);
                }
            });
            
            
            root.Add(dropDown);
            root.Add(fieldsContainer);
            
            EditorContentDrawer.RebuildFields(fieldsContainer, property);
            
            return root;
        }
        

       
    }
}
