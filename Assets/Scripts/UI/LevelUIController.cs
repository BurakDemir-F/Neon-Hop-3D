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
        private Button _bannerActionButton;

        private VisualElement _boosterContainer;
        private Button _boosterButton;

        public event Action OnBoosterClicked;

        private bool _isInitialized;
        private bool _retryTriggered;

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

            var gameFont = Resources.Load<Font>("GameFont");

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
                _topContainer.style.top = 70f;
                _topContainer.style.left = 0f;
                _topContainer.style.right = 0f;
                _topContainer.style.alignItems = Align.Center;
                _topContainer.style.justifyContent = Justify.Center;

                var badge = new VisualElement { name = "top-level-badge", pickingMode = PickingMode.Ignore };
                badge.style.backgroundColor = new Color(0.06f, 0.08f, 0.14f, 0.94f);
                badge.style.borderLeftColor = new Color(0f, 0.92f, 1f, 1f); // Neon Cyan
                badge.style.borderRightColor = new Color(0f, 0.92f, 1f, 1f);
                badge.style.borderTopColor = new Color(0f, 0.92f, 1f, 1f);
                badge.style.borderBottomColor = new Color(0f, 0.92f, 1f, 1f);
                badge.style.borderLeftWidth = 3.5f;
                badge.style.borderRightWidth = 3.5f;
                badge.style.borderTopWidth = 3.5f;
                badge.style.borderBottomWidth = 3.5f;
                badge.style.borderTopLeftRadius = 32f;
                badge.style.borderTopRightRadius = 32f;
                badge.style.borderBottomLeftRadius = 32f;
                badge.style.borderBottomRightRadius = 32f;
                badge.style.paddingLeft = 44f;
                badge.style.paddingRight = 44f;
                badge.style.paddingTop = 14f;
                badge.style.paddingBottom = 14f;

                _topLevelLabel = new Label("LEVEL 1") { name = "top-level-label", pickingMode = PickingMode.Ignore };
                _topLevelLabel.style.color = new Color(1f, 0.86f, 0.24f, 1f); // Vibrant Gold
                _topLevelLabel.style.fontSize = 32f;
                _topLevelLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
                _topLevelLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
                _topLevelLabel.style.unityTextOutlineColor = new Color(1f, 0.70f, 0f, 0.5f);
                _topLevelLabel.style.unityTextOutlineWidth = 1.5f;

                if (gameFont != null)
                {
                    _topLevelLabel.style.unityFont = gameFont;
                    _topLevelLabel.style.unityFontDefinition = FontDefinition.FromFont(gameFont);
                }

                badge.Add(_topLevelLabel);
                _topContainer.Add(badge);
                _rootElement.Add(_topContainer);
            }
            else
            {
                _topLevelLabel = _topContainer.Q<Label>("top-level-label");
                if (_topLevelLabel != null && gameFont != null)
                {
                    _topLevelLabel.style.unityFont = gameFont;
                    _topLevelLabel.style.unityFontDefinition = FontDefinition.FromFont(gameFont);
                }
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
                _bannerOverlay.style.backgroundColor = new Color(0f, 0f, 0f, 0.55f);

                _bannerCard = new VisualElement { name = "banner-card", pickingMode = PickingMode.Ignore };
                _bannerCard.style.backgroundColor = new Color(0.05f, 0.07f, 0.12f, 0.96f);
                _bannerCard.style.borderLeftColor = new Color(0f, 1f, 0.65f, 1f); // Neon Emerald
                _bannerCard.style.borderRightColor = new Color(0f, 1f, 0.65f, 1f);
                _bannerCard.style.borderTopColor = new Color(0f, 1f, 0.65f, 1f);
                _bannerCard.style.borderBottomColor = new Color(0f, 1f, 0.65f, 1f);
                _bannerCard.style.borderLeftWidth = 4.5f;
                _bannerCard.style.borderRightWidth = 4.5f;
                _bannerCard.style.borderTopWidth = 4.5f;
                _bannerCard.style.borderBottomWidth = 4.5f;
                _bannerCard.style.borderTopLeftRadius = 40f;
                _bannerCard.style.borderTopRightRadius = 40f;
                _bannerCard.style.borderBottomLeftRadius = 40f;
                _bannerCard.style.borderBottomRightRadius = 40f;
                _bannerCard.style.paddingLeft = 60f;
                _bannerCard.style.paddingRight = 60f;
                _bannerCard.style.paddingTop = 32f;
                _bannerCard.style.paddingBottom = 32f;
                _bannerCard.style.alignItems = Align.Center;

                _bannerMainLabel = new Label("LEVEL WIN!") { name = "banner-main-label", pickingMode = PickingMode.Ignore };
                _bannerMainLabel.style.color = new Color(0f, 1f, 0.65f, 1f);
                _bannerMainLabel.style.fontSize = 58f;
                _bannerMainLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
                _bannerMainLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
                _bannerMainLabel.style.unityTextOutlineColor = new Color(0f, 1f, 0.65f, 0.6f);
                _bannerMainLabel.style.unityTextOutlineWidth = 2.5f;

                _bannerSubLabel = new Label("COMPLETED") { name = "banner-sub-label", pickingMode = PickingMode.Ignore };
                _bannerSubLabel.style.color = new Color(0.92f, 0.95f, 1f, 0.95f);
                _bannerSubLabel.style.fontSize = 26f;
                _bannerSubLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
                _bannerSubLabel.style.marginTop = 10f;
                _bannerSubLabel.style.unityTextAlign = TextAnchor.MiddleCenter;

                if (gameFont != null)
                {
                    _bannerMainLabel.style.unityFont = gameFont;
                    _bannerMainLabel.style.unityFontDefinition = FontDefinition.FromFont(gameFont);
                    _bannerSubLabel.style.unityFont = gameFont;
                    _bannerSubLabel.style.unityFontDefinition = FontDefinition.FromFont(gameFont);
                }

                _bannerActionButton = new Button(OnRetryButtonClicked)
                {
                    name = "banner-action-button",
                    text = "↺ RETRY"
                };
                _bannerActionButton.style.marginTop = 24f;
                _bannerActionButton.style.paddingLeft = 40f;
                _bannerActionButton.style.paddingRight = 40f;
                _bannerActionButton.style.paddingTop = 14f;
                _bannerActionButton.style.paddingBottom = 14f;
                _bannerActionButton.style.backgroundColor = new Color(0.14f, 0.05f, 0.08f, 0.95f);
                _bannerActionButton.style.borderLeftColor = new Color(1f, 0.18f, 0.33f, 1f); // Neon Crimson
                _bannerActionButton.style.borderRightColor = new Color(1f, 0.18f, 0.33f, 1f);
                _bannerActionButton.style.borderTopColor = new Color(1f, 0.18f, 0.33f, 1f);
                _bannerActionButton.style.borderBottomColor = new Color(1f, 0.18f, 0.33f, 1f);
                _bannerActionButton.style.borderLeftWidth = 3.5f;
                _bannerActionButton.style.borderRightWidth = 3.5f;
                _bannerActionButton.style.borderTopWidth = 3.5f;
                _bannerActionButton.style.borderBottomWidth = 3.5f;
                _bannerActionButton.style.borderTopLeftRadius = 28f;
                _bannerActionButton.style.borderTopRightRadius = 28f;
                _bannerActionButton.style.borderBottomLeftRadius = 28f;
                _bannerActionButton.style.borderBottomRightRadius = 28f;
                _bannerActionButton.style.color = Color.white;
                _bannerActionButton.style.fontSize = 24f;
                _bannerActionButton.style.unityFontStyleAndWeight = FontStyle.Bold;
                _bannerActionButton.style.display = DisplayStyle.None;

                if (gameFont != null)
                {
                    _bannerActionButton.style.unityFont = gameFont;
                    _bannerActionButton.style.unityFontDefinition = FontDefinition.FromFont(gameFont);
                }

                _bannerActionButton.RegisterCallback<PointerDownEvent>(evt =>
                {
                    _bannerActionButton.style.scale = new StyleScale(new Scale(new Vector3(0.92f, 0.92f, 1f)));
                });
                _bannerActionButton.RegisterCallback<PointerUpEvent>(evt =>
                {
                    _bannerActionButton.style.scale = new StyleScale(new Scale(Vector3.one));
                });

                _bannerCard.Add(_bannerMainLabel);
                _bannerCard.Add(_bannerSubLabel);
                _bannerCard.Add(_bannerActionButton);
                _bannerOverlay.Add(_bannerCard);
                _rootElement.Add(_bannerOverlay);
            }
            else
            {
                _bannerCard = _bannerOverlay.Q<VisualElement>("banner-card");
                _bannerMainLabel = _bannerOverlay.Q<Label>("banner-main-label");
                _bannerSubLabel = _bannerOverlay.Q<Label>("banner-sub-label");
                _bannerActionButton = _bannerOverlay.Q<Button>("banner-action-button");
                if (_bannerMainLabel != null && gameFont != null)
                {
                    _bannerMainLabel.style.unityFont = gameFont;
                    _bannerMainLabel.style.unityFontDefinition = FontDefinition.FromFont(gameFont);
                }
                if (_bannerSubLabel != null && gameFont != null)
                {
                    _bannerSubLabel.style.unityFont = gameFont;
                    _bannerSubLabel.style.unityFontDefinition = FontDefinition.FromFont(gameFont);
                }
                if (_bannerActionButton != null && gameFont != null)
                {
                    _bannerActionButton.style.unityFont = gameFont;
                    _bannerActionButton.style.unityFontDefinition = FontDefinition.FromFont(gameFont);
                }
            }

            // 3. Right-Side Booster Summon Button
            _boosterContainer = _rootElement.Q<VisualElement>("booster-container");
            if (_boosterContainer == null)
            {
                _boosterContainer = new VisualElement
                {
                    name = "booster-container",
                    pickingMode = PickingMode.Position
                };
                _boosterContainer.style.position = Position.Absolute;
                _boosterContainer.style.right = 28f;
                _boosterContainer.style.bottom = 260f;
                _boosterContainer.style.alignItems = Align.Center;
                _boosterContainer.style.justifyContent = Justify.Center;

                _boosterButton = new Button(OnBoosterButtonClicked)
                {
                    name = "booster-button"
                };
                _boosterButton.style.width = 105f;
                _boosterButton.style.height = 105f;
                _boosterButton.style.backgroundColor = new Color(0.07f, 0.10f, 0.16f, 0.95f);
                _boosterButton.style.borderLeftColor = new Color(1f, 0.82f, 0.15f, 1f); // Neon Gold
                _boosterButton.style.borderRightColor = new Color(1f, 0.82f, 0.15f, 1f);
                _boosterButton.style.borderTopColor = new Color(1f, 0.82f, 0.15f, 1f);
                _boosterButton.style.borderBottomColor = new Color(1f, 0.82f, 0.15f, 1f);
                _boosterButton.style.borderLeftWidth = 4f;
                _boosterButton.style.borderRightWidth = 4f;
                _boosterButton.style.borderTopWidth = 4f;
                _boosterButton.style.borderBottomWidth = 4f;
                _boosterButton.style.borderTopLeftRadius = 52.5f;
                _boosterButton.style.borderTopRightRadius = 52.5f;
                _boosterButton.style.borderBottomLeftRadius = 52.5f;
                _boosterButton.style.borderBottomRightRadius = 52.5f;
                _boosterButton.style.alignItems = Align.Center;
                _boosterButton.style.justifyContent = Justify.Center;
                _boosterButton.style.paddingLeft = 0f;
                _boosterButton.style.paddingRight = 0f;
                _boosterButton.style.paddingTop = 0f;
                _boosterButton.style.paddingBottom = 0f;

                var iconLabel = new Label("⚡")
                {
                    name = "booster-icon-label",
                    pickingMode = PickingMode.Ignore
                };
                iconLabel.style.fontSize = 42f;
                iconLabel.style.color = new Color(1f, 0.88f, 0.25f, 1f);
                iconLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
                iconLabel.style.unityTextOutlineColor = new Color(1f, 0.65f, 0f, 0.7f);
                iconLabel.style.unityTextOutlineWidth = 2f;

                var textLabel = new Label("BOOST")
                {
                    name = "booster-text-label",
                    pickingMode = PickingMode.Ignore
                };
                textLabel.style.fontSize = 14f;
                textLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
                textLabel.style.color = new Color(1f, 1f, 1f, 0.95f);
                textLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
                textLabel.style.marginTop = -2f;

                if (gameFont != null)
                {
                    textLabel.style.unityFont = gameFont;
                    textLabel.style.unityFontDefinition = FontDefinition.FromFont(gameFont);
                }

                _boosterButton.Add(iconLabel);
                _boosterButton.Add(textLabel);

                _boosterButton.RegisterCallback<PointerDownEvent>(evt =>
                {
                    _boosterButton.style.scale = new StyleScale(new Scale(new Vector3(0.92f, 0.92f, 1f)));
                });
                _boosterButton.RegisterCallback<PointerUpEvent>(evt =>
                {
                    _boosterButton.style.scale = new StyleScale(new Scale(Vector3.one));
                });

                _boosterContainer.Add(_boosterButton);
                _rootElement.Add(_boosterContainer);
            }
            else
            {
                _boosterButton = _boosterContainer.Q<Button>("booster-button");
            }
        }

        private void Update()
        {
            if (_boosterButton != null)
            {
                var ballController = GameManager.Instance != null ? GameManager.Instance.BallController : null;
                bool canBoost = ballController == null || !ballController.HasRunningBalls;
                _boosterButton.SetEnabled(canBoost);
                _boosterButton.style.opacity = canBoost ? 1f : 0.45f;
            }
        }

        private void OnBoosterButtonClicked()
        {
            OnBoosterClicked?.Invoke();

            var ballController = GameManager.Instance != null ? GameManager.Instance.BallController : FindFirstObjectByType<BallController>();
            if (ballController != null && !ballController.HasRunningBalls)
            {
                ballController.LaunchBooster();
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
                _bannerOverlay.pickingMode = PickingMode.Ignore;
            }

            if (_bannerActionButton != null)
            {
                _bannerActionButton.style.display = DisplayStyle.None;
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

            if (_bannerActionButton != null)
            {
                _bannerActionButton.style.display = DisplayStyle.None;
            }
            _bannerOverlay.pickingMode = PickingMode.Ignore;

            // Phase 1: LEVEL WIN
            _bannerMainLabel.text = "LEVEL WIN!";
            _bannerMainLabel.style.color = new Color(0f, 1f, 0.65f, 1f); // Neon Emerald
            _bannerMainLabel.style.unityTextOutlineColor = new Color(0f, 1f, 0.65f, 0.6f);
            if (_bannerSubLabel != null)
            {
                _bannerSubLabel.text = "All Discs Cleared!";
            }

            if (_bannerCard != null)
            {
                _bannerCard.style.borderLeftColor = new Color(0f, 1f, 0.65f, 1f);
                _bannerCard.style.borderRightColor = new Color(0f, 1f, 0.65f, 1f);
                _bannerCard.style.borderTopColor = new Color(0f, 1f, 0.65f, 1f);
                _bannerCard.style.borderBottomColor = new Color(0f, 1f, 0.65f, 1f);
            }

            _bannerOverlay.style.display = DisplayStyle.Flex;
            _bannerOverlay.style.opacity = 1f;

            // Animate card scale pop
            yield return AnimateCardScale(0.7f, 1.0f, 0.25f);

            yield return new WaitForSeconds(1.2f);

            // Phase 2: Show NEXT LEVEL banner
            _bannerMainLabel.text = $"LEVEL {nextLevel}";
            _bannerMainLabel.style.color = new Color(1f, 0.86f, 0.24f, 1f); // Neon Gold
            _bannerMainLabel.style.unityTextOutlineColor = new Color(1f, 0.70f, 0f, 0.6f);
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

        /// <summary>
        /// Displays "LEVEL FAILED" card with retry button when the player runs out of balls.
        /// </summary>
        public IEnumerator ShowLevelFailedRoutine(int failedLevel)
        {
            EnsureInitialized();

            if (_bannerOverlay == null || _bannerMainLabel == null)
            {
                yield return new WaitForSeconds(2.0f);
                GameManager.Instance?.RestartLevel();
                yield break;
            }

            _retryTriggered = false;

            var neonRed = new Color(1f, 0.18f, 0.33f, 1f); // Neon Crimson
            _bannerMainLabel.text = "LEVEL FAILED";
            _bannerMainLabel.style.color = neonRed;
            _bannerMainLabel.style.unityTextOutlineColor = new Color(1f, 0.05f, 0.2f, 0.7f);

            if (_bannerSubLabel != null)
            {
                _bannerSubLabel.text = "Out of Balls!";
                _bannerSubLabel.style.color = new Color(1f, 0.88f, 0.90f, 0.95f);
            }

            if (_bannerCard != null)
            {
                _bannerCard.style.borderLeftColor = neonRed;
                _bannerCard.style.borderRightColor = neonRed;
                _bannerCard.style.borderTopColor = neonRed;
                _bannerCard.style.borderBottomColor = neonRed;
            }

            if (_bannerActionButton != null)
            {
                _bannerActionButton.style.display = DisplayStyle.Flex;
            }

            _bannerOverlay.pickingMode = PickingMode.Position;
            _bannerOverlay.style.display = DisplayStyle.Flex;
            _bannerOverlay.style.opacity = 1f;

            // Animate card pop
            yield return AnimateCardScale(0.7f, 1.0f, 0.25f);

            // Wait for user to tap retry or timeout after 4.5 seconds
            float elapsed = 0f;
            float timeout = 4.5f;
            while (!_retryTriggered && elapsed < timeout)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (!_retryTriggered)
            {
                _retryTriggered = true;
                _bannerOverlay.style.display = DisplayStyle.None;
                _bannerOverlay.pickingMode = PickingMode.Ignore;
                if (_bannerActionButton != null)
                {
                    _bannerActionButton.style.display = DisplayStyle.None;
                }
                GameManager.Instance?.RestartLevel();
            }
        }

        private void OnRetryButtonClicked()
        {
            if (_retryTriggered) return;
            _retryTriggered = true;

            if (_bannerOverlay != null)
            {
                _bannerOverlay.style.display = DisplayStyle.None;
                _bannerOverlay.pickingMode = PickingMode.Ignore;
            }
            if (_bannerActionButton != null)
            {
                _bannerActionButton.style.display = DisplayStyle.None;
            }

            GameManager.Instance?.RestartLevel();
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
