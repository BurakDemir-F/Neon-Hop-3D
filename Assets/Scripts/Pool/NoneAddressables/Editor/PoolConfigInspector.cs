using System;
using System.Collections.Generic;
using System.Text;
using Game.Pool.AddressablesLocal.Scripts.SO;
using Game.Pool.AddressablesLocal.Scripts.VO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Game.Sorcerum
{
    //TODO:: add highlight functionality.
    
    [CustomEditor(typeof(PoolConfigSo))]
    public class PoolConfigInspector : UnityEditor.Editor
    {
        private PoolConfigSo _poolConfig;
        private SerializedObject _configObj;
        private VisualElement _resultElement;

        private void OnEnable()
        {
            _poolConfig = target as PoolConfigSo;
            _configObj = new SerializedObject(_poolConfig);
            _resultElement = new VisualElement();
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root,_configObj,this);

            var textField = new TextField("Search:");

            textField.RegisterValueChangedCallback(OnSearchTextUpdated);

            
            root.Add(textField);
            root.Add(_resultElement);
            
            return root;
        }

        private List<PoolConfig> _resultConfigs = new List<PoolConfig>();

        private void OnSearchTextUpdated(ChangeEvent<string> evt)
        {
            _resultElement.Clear();
            
            var searchText = evt.newValue;
            
            _resultConfigs.Clear();
            
            foreach (var poolConfig in _poolConfig.PoolConfig)
            {
                if (!poolConfig.PoolKey.Contains(searchText,StringComparison.OrdinalIgnoreCase))
                    continue;
                
                _resultConfigs.Add(poolConfig);
            }

            var listView = new ListView(_resultConfigs,makeItem:() => new Button(), bindItem: ((element, i) =>
            {
                var button = (Button)element;

                var configOrder = 0;
                var currentConfig = _resultConfigs[i];

                for (var index = 0; index < _poolConfig.PoolConfig.Count; index++)
                {
                    var poolConfig = _poolConfig.PoolConfig[index];
                    if (poolConfig.PoolKey == currentConfig.PoolKey)
                    {
                        configOrder = index;
                        break;
                    }
                }

                button.text = $"{currentConfig.PoolKey}, order: {configOrder}";
            }));
            
            _resultElement.Add(listView);
        }
    }
}