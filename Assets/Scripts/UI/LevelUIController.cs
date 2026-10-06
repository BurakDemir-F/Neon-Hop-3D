using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Sorcerum
{
    /// <summary>
    /// Manages Level HUD and victory/transition banners using Unity UI Toolkit.
    /// </summary>
    public class LevelUIController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UIDocument _uiDocument;

        // UI Elements
        private VisualElement _rootElement;
        private VisualElement _topContainer;
        private Label _topLevelLabel;

        private VisualElement _bannerOverlay;
        private VisualElement _bannerCard;
        private Label _bannerMainLabel;
        private Label _bannerSubLabel;

        private bool _isInitialized;

        private void Awake()
        {
            EnsureInitialized();
        }

        private void OnEnable()
        {
            EnsureInitialized();
        }

        public void EnsureInitialized()
        {
            if (_isInitialized && _rootElement != null) return;

            if (_uiDocument == null)
            {
                _uiDocument = GetComponent<UIDocument>();
                if (_uiDocument == null)
                {
                    _uiDocument = FindFirstObjectByType<UIDocument>();
                }
            }

            if (_uiDocument == null || _uiDocument.rootVisualElement == null)
            {
                return;
            }

            _rootElement = _uiDocument.rootVisualElement;

            BuildUIHierarchy();
            _isInitialized = true;
        }

        private void BuildUIHierarchy()
        {
            if (_rootElement == null) return;

            // 1. Top HUD Level Badge
            _topContainer = _rootElement.Q<VisualElement>("top-level-container");
            if (_topContainer == null)
            {
                _topContainer = new VisualElement
                {
                    name = "top-level-container",
                    pickingMode = PickingMode.Ignore
                };

                _topContainer.style.position = Position.Absolute;
                _topContainer.style.top = 36f;
                _topContainer.style.left = 0f;
                _topContainer.style.right = 0f;
                _topContainer.style.alignItems = Align.Center;
                _topContainer.style.justifyContent = Justify.Center;

                var badge = new VisualElement { name = "top-level-badge", pickingMode = PickingMode.Ignore };
                badge.style.backgroundColor = new Color(0.08f, 0.11f, 0.16f, 0.88f);
                badge.style.borderLeftColor = new Color(1f, 1f, 1f, 0.25f);
                badge.style.borderRightColor = new Color(1f, 1f, 1f, 0.25f);
                badge.style.borderTopColor = new Color(1f, 1f, 1f, 0.25f);
                badge.style.borderBottomColor = new Color(1f, 1f, 1f, 0.25f);
                badge.style.borderLeftWidth = 2f;
                badge.style.borderRightWidth = 2f;
                badge.style.borderTopWidth = 2f;
                badge.style.borderBottomWidth = 2f;
                badge.style.borderTopLeftRadius = 22f;
                badge.style.borderTopRightRadius = 22f;
                badge.style.borderBottomLeftRadius = 22f;
                badge.style.borderBottomRightRadius = 22f;
                badge.style.paddingLeft = 28f;
                badge.style.paddingRight = 28f;
                badge.style.paddingTop = 8f;
                badge.style.paddingBottom = 8f;

                _topLevelLabel = new Label("LEVEL 1") { name = "top-level-label", pickingMode = PickingMode.Ignore };
                _topLevelLabel.style.color = new Color(1f, 0.86f, 0.24f, 1f); // Vibrant Gold
                _topLevelLabel.style.fontSize = 22f;
                _topLevelLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
                _topLevelLabel.style.unityTextAlign = TextAnchor.MiddleCenter;

                badge.Add(_topLevelLabel);
                _topContainer.Add(badge);
                _rootElement.Add(_topContainer);
            }
            else
            {
                _topLevelLabel = _topContainer.Q<Label>("top-level-label");
            }

            // 2. Center Banner Overlay (Level Win / Level Start)
            _bannerOverlay = _rootElement.Q<VisualElement>("level-banner-overlay");
            if (_bannerOverlay == null)
            {
                _bannerOverlay = new VisualElement
                {
                    name = "level-banner-overlay",
                    pickingMode = PickingMode.Ignore
                };

                _bannerOverlay.style.position = Position.Absolute;
                _bannerOverlay.style.left = 0f;
                _bannerOverlay.style.right = 0f;
                _bannerOverlay.style.top = 0f;
                _bannerOverlay.style.bottom = 0f;
                _bannerOverlay.style.alignItems = Align.Center;
                _bannerOverlay.style.justifyContent = Justify.Center;
                _bannerOverlay.style.display = DisplayStyle.None;
                _bannerOverlay.style.backgroundColor = new Color(0f, 0f, 0f, 0.35f);

                _bannerCard = new VisualElement { name = "banner-card", pickingMode = PickingMode.Ignore };
                _bannerCard.style.backgroundColor = new Color(0.06f, 0.08f, 0.13f, 0.95f);
                _bannerCard.style.borderLeftColor = new Color(0.18f, 0.8f, 0.44f, 1f);
                _bannerCard.style.borderRightColor = new Color(0.18f, 0.8f, 0.44f, 1f);
                _bannerCard.style.borderTopColor = new Color(0.18f, 0.8f, 0.44f, 1f);
                _bannerCard.style.borderBottomColor = new Color(0.18f, 0.8f, 0.44f, 1f);
                _bannerCard.style.borderLeftWidth = 3f;
                _bannerCard.style.borderRightWidth = 3f;
                _bannerCard.style.borderTopWidth = 3f;
                _bannerCard.style.borderBottomWidth = 3f;
                _bannerCard.style.borderTopLeftRadius = 26f;
                _bannerCard.style.borderTopRightRadius = 26f;
                _bannerCard.style.borderBottomLeftRadius = 26f;
                _bannerCard.style.borderBottomRightRadius = 26f;
                _bannerCard.style.paddingLeft = 44f;
                _bannerCard.style.paddingRight = 44f;
                _bannerCard.style.paddingTop = 22f;
                _bannerCard.style.paddingBottom = 22f;
                _bannerCard.style.alignItems = Align.Center;

                _bannerMainLabel = new Label("LEVEL WIN!") { name = "banner-main-label", pickingMode = PickingMode.Ignore };
                _bannerMainLabel.style.color = new Color(0.18f, 0.8f, 0.44f, 1f);
                _bannerMainLabel.style.fontSize = 38f;
                _bannerMainLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
                _bannerMainLabel.style.unityTextAlign = TextAnchor.MiddleCenter;

                _bannerSubLabel = new Label("COMPLETED") { name = "banner-sub-label", pickingMode = PickingMode.Ignore };
                _bannerSubLabel.style.color = new Color(0.92f, 0.94f, 0.96f, 0.9f);
                _bannerSubLabel.style.fontSize = 18f;
                _bannerSubLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
                _bannerSubLabel.style.marginTop = 6f;
                _bannerSubLabel.style.unityTextAlign = TextAnchor.MiddleCenter;

                _bannerCard.Add(_bannerMainLabel);
                _bannerCard.Add(_bannerSubLabel);
                _bannerOverlay.Add(_bannerCard);
                _rootElement.Add(_bannerOverlay);
            }
            else
            {
                _bannerCard = _bannerOverlay.Q<VisualElement>("banner-card");
                _bannerMainLabel = _bannerOverlay.Q<Label>("banner-main-label");
                _bannerSubLabel = _bannerOverlay.Q<Label>("banner-sub-label");
            }
        }

        /// <summary>
        /// Updates the top HUD to show the specified level number.
        /// </summary>
        public void ShowLevel(int levelNumber)
        {
            EnsureInitialized();

            if (_topLevelLabel != null)
            {
                _topLevelLabel.text = $"LEVEL {levelNumber}";
            }

            if (_bannerOverlay != null)
            {
                _bannerOverlay.style.display = DisplayStyle.None;
            }
        }

        /// <summary>
        /// Displays "LEVEL WIN!", transitions to "LEVEL {nextLevel}", and concludes.
        /// </summary>
        public IEnumerator ShowLevelWinRoutine(int completedLevel, int nextLevel)
        {
            EnsureInitialized();

            if (_bannerOverlay == null || _bannerMainLabel == null)
            {
                yield return new WaitForSeconds(1.5f);
                yield break;
            }

            // Phase 1: LEVEL WIN
            _bannerMainLabel.text = "LEVEL WIN!";
            _bannerMainLabel.style.color = new Color(0.18f, 0.8f, 0.44f, 1f); // Emerald
            if (_bannerSubLabel != null)
            {
                _bannerSubLabel.text = "All Discs Cleared!";
            }

            if (_bannerCard != null)
            {
                _bannerCard.style.borderLeftColor = new Color(0.18f, 0.8f, 0.44f, 1f);
                _bannerCard.style.borderRightColor = new Color(0.18f, 0.8f, 0.44f, 1f);
                _bannerCard.style.borderTopColor = new Color(0.18f, 0.8f, 0.44f, 1f);
                _bannerCard.style.borderBottomColor = new Color(0.18f, 0.8f, 0.44f, 1f);
            }

            _bannerOverlay.style.display = DisplayStyle.Flex;
            _bannerOverlay.style.opacity = 1f;

            // Animate card scale pop
            yield return AnimateCardScale(0.7f, 1.0f, 0.25f);

            yield return new WaitForSeconds(1.2f);

            // Phase 2: Show NEXT LEVEL banner
            _bannerMainLabel.text = $"LEVEL {nextLevel}";
            _bannerMainLabel.style.color = new Color(1f, 0.86f, 0.24f, 1f); // Gold
            if (_bannerSubLabel != null)
            {
                _bannerSubLabel.text = "Get Ready!";
            }

            if (_bannerCard != null)
            {
                _bannerCard.style.borderLeftColor = new Color(1f, 0.86f, 0.24f, 1f);
                _bannerCard.style.borderRightColor = new Color(1f, 0.86f, 0.24f, 1f);
                _bannerCard.style.borderTopColor = new Color(1f, 0.86f, 0.24f, 1f);
                _bannerCard.style.borderBottomColor = new Color(1f, 0.86f, 0.24f, 1f);
            }

            yield return AnimateCardScale(0.85f, 1.0f, 0.2f);

            yield return new WaitForSeconds(0.9f);

            // Hide banner and update top label
            _bannerOverlay.style.display = DisplayStyle.None;
            if (_topLevelLabel != null)
            {
                _topLevelLabel.text = $"LEVEL {nextLevel}";
            }
        }

        private IEnumerator AnimateCardScale(float fromScale, float toScale, float duration)
        {
            if (_bannerCard == null) yield break;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                // Ease out back
                float s = Mathf.Lerp(fromScale, toScale, Mathf.Sin(t * Mathf.PI * 0.5f));
                _bannerCard.style.scale = new StyleScale(new Scale(new Vector3(s, s, 1f)));
                yield return null;
            }

            _bannerCard.style.scale = new StyleScale(new Scale(new Vector3(toScale, toScale, 1f)));
        }
    }
}
