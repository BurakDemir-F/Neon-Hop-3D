using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Sorcerum
{
    /// <summary>
    /// Screen controller managing the display and interaction of available balls in the tray.
    /// Built using Unity's UI Toolkit (UnityEngine.UIElements).
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public class AvailableBallScreen : MonoBehaviour
    {
        [Header("UI Document & Templates")]
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private VisualTreeAsset _ballItemTemplate;
        [SerializeField] private StyleSheet _styleSheet;

        [Header("Appearance")]
        [SerializeField] private float _ballSize = 72f;
        [SerializeField] private float _ballSpacing = 16f;
        [SerializeField] private Color _trayBackgroundColor = new Color(0.10f, 0.12f, 0.16f, 0.88f);

        // UI Elements
        private VisualElement _rootElement;
        private VisualElement _ballsContainer;
        private VisualElement _trayElement;
        private Label _headerLabel;

        // Runtime State
        private readonly List<BallUIItem> _spawnedBallItems = new();
        private int _selectedIndex = -1;

        // Events
        public event Action<IAttributeProvider, int> OnBallSelected;
        public event Action<IAttributeProvider, int> OnBallClicked;
        public event Action OnSelectionCleared;

        public int SelectedIndex => _selectedIndex;
        public int BallCount => _spawnedBallItems.Count;
        public IAttributeProvider SelectedBall => (_selectedIndex >= 0 && _selectedIndex < _spawnedBallItems.Count)
            ? _spawnedBallItems[_selectedIndex].Provider
            : null;

        public IReadOnlyList<IAttributeProvider> CurrentBalls
        {
            get
            {
                var list = new List<IAttributeProvider>(_spawnedBallItems.Count);
                for (int i = 0; i < _spawnedBallItems.Count; i++)
                {
                    list.Add(_spawnedBallItems[i].Provider);
                }
                return list;
            }
        }

        private void Awake()
        {
            EnsureInitialized();
        }

        private void OnEnable()
        {
            EnsureInitialized();
        }

        /// <summary>
        /// Ensures UIDocument and visual element references are ready.
        /// </summary>
        public void EnsureInitialized()
        {
            if (_uiDocument == null)
            {
                _uiDocument = GetComponent<UIDocument>();
            }

            if (_uiDocument == null)
            {
                Debug.LogError($"[{nameof(AvailableBallScreen)}] UIDocument component is missing!", this);
                return;
            }

            _rootElement = _uiDocument.rootVisualElement;
            if (_rootElement == null) return;

            // Apply custom stylesheet if assigned
            if (_styleSheet != null && !_rootElement.styleSheets.Contains(_styleSheet))
            {
                _rootElement.styleSheets.Add(_styleSheet);
            }

            // Look for existing container in assigned UXML template
            _ballsContainer = _rootElement.Q<VisualElement>("available-balls-container");
            _trayElement = _rootElement.Q<VisualElement>("available-balls-tray");
            _headerLabel = _rootElement.Q<Label>("header-label");

            // If not found in UXML, build responsive UI hierarchy via code
            if (_ballsContainer == null)
            {
                BuildFallbackHierarchy();
            }
        }

        /// <summary>
        /// Populates the tray with available balls from a list of IAttributeProvider.
        /// Extracts BallVisualAttribute and ToughnessAttribute for each ball.
        /// </summary>
        public void ShowAvailableBalls(List<IAttributeProvider> providerList)
        {
            EnsureInitialized();
            ClearBalls();

            if (providerList == null || providerList.Count == 0)
                return;

            for (int i = 0; i < providerList.Count; i++)
            {
                var attributeProvider = providerList[i];
                if (attributeProvider == null) continue;

                var ballItem = CreateBallItem(attributeProvider, i);
                _spawnedBallItems.Add(ballItem);
                _ballsContainer.Add(ballItem.Root);
            }
        }

        /// <summary>
        /// Overload accepting AvailableBallData from LevelData.
        /// </summary>
        public void ShowAvailableBalls(AvailableBallData ballData)
        {
            if (ballData == null || ballData.AvailableBalls == null)
            {
                ClearBalls();
                return;
            }

            var providerList = new List<IAttributeProvider>(ballData.AvailableBalls.Count);
            foreach (var ball in ballData.AvailableBalls)
            {
                if (ball != null)
                {
                    providerList.Add(ball);
                }
            }

            ShowAvailableBalls(providerList);
        }

        /// <summary>
        /// Overload accepting a list of BallDataSo directly.
        /// </summary>
        public void ShowAvailableBalls(IReadOnlyList<BallDataSo> balls)
        {
            if (balls == null)
            {
                ClearBalls();
                return;
            }

            var providerList = new List<IAttributeProvider>(balls.Count);
            for (int i = 0; i < balls.Count; i++)
            {
                if (balls[i] != null)
                {
                    providerList.Add(balls[i]);
                }
            }

            ShowAvailableBalls(providerList);
        }

        /// <summary>
        /// Overload accepting LevelDataSo directly.
        /// </summary>
        public void ShowAvailableBalls(LevelDataSo levelData)
        {
            if (levelData == null || levelData.LevelDataVo == null)
            {
                ClearBalls();
                return;
            }

            ShowAvailableBalls(levelData.LevelDataVo.BallData);
        }

        /// <summary>
        /// Adds a single ball to the tray.
        /// </summary>
        public void AddBall(IAttributeProvider provider)
        {
            if (provider == null) return;
            EnsureInitialized();

            int newIndex = _spawnedBallItems.Count;
            var ballItem = CreateBallItem(provider, newIndex);
            _spawnedBallItems.Add(ballItem);
            _ballsContainer.Add(ballItem.Root);
        }

        /// <summary>
        /// Consumes and removes the currently selected ball.
        /// Returns the consumed IAttributeProvider, or null if none selected.
        /// </summary>
        public IAttributeProvider ConsumeSelectedBall()
        {
            if (_selectedIndex < 0 || _selectedIndex >= _spawnedBallItems.Count)
                return null;

            return RemoveBallAt(_selectedIndex);
        }

        /// <summary>
        /// Removes the ball at the specified index.
        /// </summary>
        public IAttributeProvider RemoveBallAt(int index)
        {
            if (index < 0 || index >= _spawnedBallItems.Count)
                return null;

            var item = _spawnedBallItems[index];
            _ballsContainer.Remove(item.Root);
            _spawnedBallItems.RemoveAt(index);

            // Re-index remaining balls
            for (int i = 0; i < _spawnedBallItems.Count; i++)
            {
                _spawnedBallItems[i].Index = i;
            }

            if (_selectedIndex == index)
            {
                _selectedIndex = -1;
                OnSelectionCleared?.Invoke();
            }
            else if (_selectedIndex > index)
            {
                _selectedIndex--;
            }

            return item.Provider;
        }

        /// <summary>
        /// Selects the ball at the given index, applying highlight visuals.
        /// </summary>
        public void SelectBall(int index)
        {
            if (index < 0 || index >= _spawnedBallItems.Count)
                return;

            if (_selectedIndex >= 0 && _selectedIndex < _spawnedBallItems.Count)
            {
                SetItemSelectedVisual(_spawnedBallItems[_selectedIndex], false);
            }

            _selectedIndex = index;
            var selectedItem = _spawnedBallItems[_selectedIndex];
            SetItemSelectedVisual(selectedItem, true);

            OnBallSelected?.Invoke(selectedItem.Provider, _selectedIndex);
        }

        /// <summary>
        /// Deselects any currently selected ball.
        /// </summary>
        public void DeselectBall()
        {
            if (_selectedIndex >= 0 && _selectedIndex < _spawnedBallItems.Count)
            {
                SetItemSelectedVisual(_spawnedBallItems[_selectedIndex], false);
            }

            _selectedIndex = -1;
            OnSelectionCleared?.Invoke();
        }

        /// <summary>
        /// Clears all balls from the tray.
        /// </summary>
        public void ClearBalls()
        {
            _selectedIndex = -1;
            if (_ballsContainer != null)
            {
                _ballsContainer.Clear();
            }
            _spawnedBallItems.Clear();
            OnSelectionCleared?.Invoke();
        }

        /// <summary>
        /// Shows or hides the entire screen.
        /// </summary>
        public void SetVisible(bool isVisible)
        {
            if (_rootElement != null)
            {
                _rootElement.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        /// <summary>
        /// Sets a custom title on the tray header.
        /// </summary>
        public void SetHeaderTitle(string title)
        {
            if (_headerLabel != null)
            {
                _headerLabel.text = title;
            }
        }

        private BallUIItem CreateBallItem(IAttributeProvider attributeProvider, int index)
        {
            Sprite visualSprite = null;
            int hitCount = 1;

            if (attributeProvider.TryGetAttribute<BallVisualAttribute>(out var ballVisualAttribute))
            {
                visualSprite = ballVisualAttribute.BallVisual;
            }

            if (attributeProvider.TryGetAttribute<ToughnessAttribute>(out var toughnessAttribute))
            {
                hitCount = toughnessAttribute.HitCount;
            }

            VisualElement itemRoot;
            VisualElement iconElement;
            Label numberLabel;

            if (_ballItemTemplate != null)
            {
                itemRoot = _ballItemTemplate.Instantiate();
                iconElement = itemRoot.Q<VisualElement>("ball-icon") ?? itemRoot;
                numberLabel = itemRoot.Q<Label>("ball-number");
            }
            else
            {
                // Dynamic programmatic creation
                itemRoot = new VisualElement();
                itemRoot.AddToClassList("ball-item");
                itemRoot.name = $"ball-item-{index}";

                itemRoot.style.width = _ballSize;
                itemRoot.style.height = _ballSize;
                itemRoot.style.marginLeft = _ballSpacing * 0.5f;
                itemRoot.style.marginRight = _ballSpacing * 0.5f;
                itemRoot.style.justifyContent = Justify.Center;
                itemRoot.style.alignItems = Align.Center;

                // Round circle
                itemRoot.style.borderTopLeftRadius = _ballSize * 0.5f;
                itemRoot.style.borderTopRightRadius = _ballSize * 0.5f;
                itemRoot.style.borderBottomLeftRadius = _ballSize * 0.5f;
                itemRoot.style.borderBottomRightRadius = _ballSize * 0.5f;

                // Border
                itemRoot.style.borderLeftWidth = 3;
                itemRoot.style.borderRightWidth = 3;
                itemRoot.style.borderTopWidth = 3;
                itemRoot.style.borderBottomWidth = 3;
                itemRoot.style.borderLeftColor = new Color(1f, 1f, 1f, 0.4f);
                itemRoot.style.borderRightColor = new Color(1f, 1f, 1f, 0.4f);
                itemRoot.style.borderTopColor = new Color(1f, 1f, 1f, 0.4f);
                itemRoot.style.borderBottomColor = new Color(1f, 1f, 1f, 0.4f);

                iconElement = itemRoot;

                // Number label
                numberLabel = new Label();
                numberLabel.name = "ball-number";
                numberLabel.AddToClassList("ball-number");
                numberLabel.style.color = Color.white;
                numberLabel.style.fontSize = Mathf.RoundToInt(_ballSize * 0.38f);
                numberLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
                numberLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
                numberLabel.style.unityTextOutlineColor = new Color(0f, 0f, 0f, 0.85f);
                numberLabel.style.unityTextOutlineWidth = 1.5f;
                numberLabel.pickingMode = PickingMode.Ignore;

                itemRoot.Add(numberLabel);
            }

            // Apply visual sprite or fallback palette color
            if (visualSprite != null)
            {
                iconElement.style.backgroundImage = new StyleBackground(visualSprite);
                iconElement.style.backgroundColor = Color.clear;
            }
            else if (attributeProvider.TryGetAttribute<ColorIdAttribute>(out var colorIdAttr))
            {
                iconElement.style.backgroundColor = ColorIntConverter.IntToColor(colorIdAttr.GetId());
            }
            else
            {
                iconElement.style.backgroundColor = GetFallbackBallColor(index);
            }

            // Apply hit count text
            if (numberLabel != null)
            {
                numberLabel.text = hitCount > 0 ? hitCount.ToString() : "";
            }

            var ballUIItem = new BallUIItem(itemRoot, iconElement, numberLabel, attributeProvider, index);

            // Register click handler
            itemRoot.RegisterCallback<ClickEvent>(evt =>
            {
                OnBallItemClicked(ballUIItem);
            });

            return ballUIItem;
        }

        private void OnBallItemClicked(BallUIItem item)
        {
            OnBallClicked?.Invoke(item.Provider, item.Index);

            if (_selectedIndex == item.Index)
            {
                DeselectBall();
            }
            else
            {
                SelectBall(item.Index);
            }
        }

        private void SetItemSelectedVisual(BallUIItem item, bool isSelected)
        {
            if (item?.Root == null) return;

            if (isSelected)
            {
                item.Root.AddToClassList("ball-item--selected");
                item.Root.style.scale = new StyleScale(new Scale(new Vector3(1.18f, 1.18f, 1f)));
                item.Root.style.borderLeftColor = new Color(1f, 0.85f, 0.15f, 1f);
                item.Root.style.borderRightColor = new Color(1f, 0.85f, 0.15f, 1f);
                item.Root.style.borderTopColor = new Color(1f, 0.85f, 0.15f, 1f);
                item.Root.style.borderBottomColor = new Color(1f, 0.85f, 0.15f, 1f);
                item.Root.style.borderLeftWidth = 4;
                item.Root.style.borderRightWidth = 4;
                item.Root.style.borderTopWidth = 4;
                item.Root.style.borderBottomWidth = 4;
            }
            else
            {
                item.Root.RemoveFromClassList("ball-item--selected");
                item.Root.style.scale = new StyleScale(new Scale(Vector3.one));
                item.Root.style.borderLeftColor = new Color(1f, 1f, 1f, 0.4f);
                item.Root.style.borderRightColor = new Color(1f, 1f, 1f, 0.4f);
                item.Root.style.borderTopColor = new Color(1f, 1f, 1f, 0.4f);
                item.Root.style.borderBottomColor = new Color(1f, 1f, 1f, 0.4f);
                item.Root.style.borderLeftWidth = 3;
                item.Root.style.borderRightWidth = 3;
                item.Root.style.borderTopWidth = 3;
                item.Root.style.borderBottomWidth = 3;
            }
        }

        private Color GetFallbackBallColor(int index)
        {
            Color[] palette =
            {
                new Color(0.18f, 0.80f, 0.95f), // Cyan / Turquoise
                new Color(0.96f, 0.26f, 0.58f), // Pink / Magenta
                new Color(1.00f, 0.60f, 0.10f), // Orange
                new Color(0.30f, 0.85f, 0.40f), // Emerald Green
                new Color(0.65f, 0.35f, 0.95f), // Violet / Purple
                new Color(1.00f, 0.85f, 0.20f)  // Gold / Yellow
            };

            return palette[index % palette.Length];
        }

        private void BuildFallbackHierarchy()
        {
            _rootElement.Clear();

            // Screen Root (Anchored at the bottom)
            var screenWrapper = new VisualElement();
            screenWrapper.name = "available-ball-screen";
            screenWrapper.AddToClassList("screen-root");
            screenWrapper.style.position = Position.Absolute;
            screenWrapper.style.bottom = 24;
            screenWrapper.style.left = 0;
            screenWrapper.style.right = 0;
            screenWrapper.style.alignItems = Align.Center;
            screenWrapper.style.justifyContent = Justify.FlexEnd;
            screenWrapper.pickingMode = PickingMode.Ignore;

            // Tray Panel
            _trayElement = new VisualElement();
            _trayElement.name = "available-balls-tray";
            _trayElement.AddToClassList("tray");
            _trayElement.style.flexDirection = FlexDirection.Column;
            _trayElement.style.alignItems = Align.Center;
            _trayElement.style.backgroundColor = _trayBackgroundColor;
            _trayElement.style.borderTopLeftRadius = 24;
            _trayElement.style.borderTopRightRadius = 24;
            _trayElement.style.borderBottomLeftRadius = 24;
            _trayElement.style.borderBottomRightRadius = 24;
            _trayElement.style.paddingTop = 10;
            _trayElement.style.paddingBottom = 16;
            _trayElement.style.paddingLeft = 20;
            _trayElement.style.paddingRight = 20;
            _trayElement.style.borderLeftWidth = 2;
            _trayElement.style.borderRightWidth = 2;
            _trayElement.style.borderTopWidth = 2;
            _trayElement.style.borderBottomWidth = 2;
            _trayElement.style.borderLeftColor = new Color(1f, 1f, 1f, 0.15f);
            _trayElement.style.borderRightColor = new Color(1f, 1f, 1f, 0.15f);
            _trayElement.style.borderTopColor = new Color(1f, 1f, 1f, 0.15f);
            _trayElement.style.borderBottomColor = new Color(1f, 1f, 1f, 0.15f);

            // Header Label
            _headerLabel = new Label("AVAILABLE BALLS");
            _headerLabel.name = "header-label";
            _headerLabel.AddToClassList("header-label");
            _headerLabel.style.color = new Color(0.85f, 0.88f, 0.95f, 0.9f);
            _headerLabel.style.fontSize = 13;
            _headerLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            _headerLabel.style.marginBottom = 10;
            _headerLabel.pickingMode = PickingMode.Ignore;
            _trayElement.Add(_headerLabel);

            // Balls Container
            _ballsContainer = new VisualElement();
            _ballsContainer.name = "available-balls-container";
            _ballsContainer.AddToClassList("balls-container");
            _ballsContainer.style.flexDirection = FlexDirection.Row;
            _ballsContainer.style.alignItems = Align.Center;
            _ballsContainer.style.justifyContent = Justify.Center;
            _ballsContainer.style.flexWrap = Wrap.NoWrap;
            _trayElement.Add(_ballsContainer);

            screenWrapper.Add(_trayElement);
            _rootElement.Add(screenWrapper);
        }
    }

    /// <summary>
    /// Holds visual and data references for an individual ball item in the UI.
    /// </summary>
    public class BallUIItem
    {
        public VisualElement Root { get; }
        public VisualElement IconElement { get; }
        public Label NumberLabel { get; }
        public IAttributeProvider Provider { get; }
        public int Index { get; set; }

        public BallUIItem(VisualElement root, VisualElement iconElement, Label numberLabel, IAttributeProvider provider, int index)
        {
            Root = root;
            IconElement = iconElement;
            NumberLabel = numberLabel;
            Provider = provider;
            Index = index;
        }
    }
}