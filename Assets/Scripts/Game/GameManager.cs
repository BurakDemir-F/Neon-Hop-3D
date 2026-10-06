using System;
using System.Collections;
using System.Collections.Generic;
using Game.Pool.AddressablesLocal.Scripts.Runtime;
using General;
using UnityEngine;

namespace Game.Sorcerum
{
    /// <summary>
    /// Central manager responsible for bootstrapping the level, orchestrating LevelManager,
    /// tracking level progression, win conditions, and hooking up UI elements.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Scene References")]
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private AvailableBallScreen _availableBallScreen;
        [SerializeField] private MasterPool _masterPool;
        [SerializeField] private StackQueueController _stackQueueController;
        [SerializeField] private BallController _ballController;
        [SerializeField] private LevelUIController _levelUIController;

        [Header("Level Progression")]
        [SerializeField] private List<LevelDataSo> _levels = new();
        [SerializeField] private int _currentLevelIndex = 0;
        [SerializeField] private LevelDataSo _currentLevelData;
        [SerializeField] private bool _autoBuildOnStart = true;
        
        private ContextProvider _contextProvider;
        private bool _isLevelCompleted;

        // Events
        public event Action<LevelDataSo> OnLevelStarted;
        public event Action<int> OnLevelWon;
        public event Action<IAttributeProvider, int> OnBallSelected;

        public int CurrentLevelIndex => _currentLevelIndex;
        public int CurrentLevelNumber => _currentLevelIndex + 1;
        public int TotalLevels => _levels != null ? _levels.Count : 0;
        public IReadOnlyList<LevelDataSo> Levels => _levels;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            ResolveDependencies();
        }

        private void Start()
        {
            if (_autoBuildOnStart)
            {
                if (_levels != null && _levels.Count > 0)
                {
                    LoadLevel(_currentLevelIndex);
                }
                else if (_currentLevelData != null)
                {
                    StartLevel(_currentLevelData);
                }
            }
        }

        private void ResolveDependencies()
        {
            if (_levelManager == null)
            {
                _levelManager = FindFirstObjectByType<LevelManager>();
            }

            if (_availableBallScreen == null)
            {
                _availableBallScreen = FindFirstObjectByType<AvailableBallScreen>();
                if (_availableBallScreen == null)
                {
                    var screenGo = new GameObject("AvailableBallScreen");
                    _availableBallScreen = screenGo.AddComponent<AvailableBallScreen>();
                }
            }

            if (_ballController == null)
            {
                _ballController = FindFirstObjectByType<BallController>();
                if (_ballController == null)
                {
                    _ballController = gameObject.AddComponent<BallController>();
                }
            }

            if (_levelUIController == null)
            {
                _levelUIController = FindFirstObjectByType<LevelUIController>();
                if (_levelUIController == null)
                {
                    _levelUIController = gameObject.AddComponent<LevelUIController>();
                }
            }
            
            if (_masterPool != null)
            {
                _masterPool.CheckAndInitialize(_masterPool.transform);
            }
        }

        /// <summary>
        /// Loads a specific level by its 0-based index.
        /// </summary>
        public void LoadLevel(int levelIndex)
        {
            if (_levels != null && _levels.Count > 0)
            {
                _currentLevelIndex = Mathf.Clamp(levelIndex, 0, _levels.Count - 1);
                _currentLevelData = _levels[_currentLevelIndex];
            }

            StartLevel(_currentLevelData);
        }

        /// <summary>
        /// Advances to the next level in the list.
        /// </summary>
        [ContextMenu("Next Level")]
        public void NextLevel()
        {
            if (_levels != null && _levels.Count > 0)
            {
                _currentLevelIndex = (_currentLevelIndex + 1) % _levels.Count;
                _currentLevelData = _levels[_currentLevelIndex];
            }

            StartLevel(_currentLevelData);
        }

        /// <summary>
        /// Starts and builds a level using the specified LevelDataSo.
        /// </summary>
        public void StartLevel(LevelDataSo levelData)
        {
            ResolveDependencies();

            if (levelData == null)
            {
                Debug.LogWarning($"[{nameof(GameManager)}] No LevelData provided to build level!", this);
                return;
            }

            _isLevelCompleted = false;
            _currentLevelData = levelData;

            if (_levelManager != null)
            {
                _contextProvider = new ContextProvider();
                _contextProvider.UpdateContext(new GameEventBus());
                
                var worldData = _levelManager.BuildLevel(_currentLevelData, _masterPool, _contextProvider);
                if (worldData != null && _stackQueueController != null)
                {
                    _stackQueueController.Initialize(worldData.StackCellList);
                }
                
                _contextProvider.UpdateContext(new GameContext(_stackQueueController?.NextPositionProvider, _masterPool, _availableBallScreen));
                _contextProvider.UpdateContext(new GameDataContext());

                if (_ballController != null)
                {
                    _ballController.Destruct();
                    _ballController.Initialize(_contextProvider);
                    _ballController.Construct();
                }
            }
            else
            {
                Debug.LogError($"[{nameof(GameManager)}] LevelManager not found in scene!", this);
            }

            // Hook up UI ball events for gameplay integration and feedback
            if (_availableBallScreen != null)
            {
                _availableBallScreen.OnBallSelected -= HandleBallSelected;
                _availableBallScreen.OnBallSelected += HandleBallSelected;
            }

            // Display current level in UI
            if (_levelUIController != null)
            {
                _levelUIController.ShowLevel(CurrentLevelNumber);
            }

            OnLevelStarted?.Invoke(_currentLevelData);
            Debug.Log($"[{nameof(GameManager)}] Level {CurrentLevelNumber} successfully built and initialized with '{_currentLevelData.name}'!");
        }

        /// <summary>
        /// Checks if all stacks in the current level are cleared. Triggers Level Win if so.
        /// </summary>
        public void CheckLevelComplete()
        {
            if (_isLevelCompleted) return;

            if (_levelManager != null && _levelManager.AreAllStacksCleared())
            {
                _isLevelCompleted = true;
                StartCoroutine(LevelCompleteRoutine());
            }
        }

        private IEnumerator LevelCompleteRoutine()
        {
            int finishedLevel = CurrentLevelNumber;
            int nextLevel = (TotalLevels > 0) ? ((_currentLevelIndex + 1) % TotalLevels) + 1 : finishedLevel + 1;

            Debug.Log($"[{nameof(GameManager)}] Level {finishedLevel} WON! All stacks cleared. Transitioning to Level {nextLevel}...");
            OnLevelWon?.Invoke(finishedLevel);

            if (_levelUIController != null)
            {
                yield return _levelUIController.ShowLevelWinRoutine(finishedLevel, nextLevel);
            }
            else
            {
                yield return new WaitForSeconds(1.5f);
            }

            NextLevel();
        }

        /// <summary>
        /// Restarts the current level from scratch.
        /// </summary>
        [ContextMenu("Restart Level")]
        public void RestartLevel()
        {
            if (_currentLevelData != null)
            {
                StartLevel(_currentLevelData);
            }
        }

        private void HandleBallSelected(IAttributeProvider ballProvider, int index)
        {
            if (_isLevelCompleted) return;

            if (_ballController != null && _ballController.HasRunningBalls)
            {
                Debug.LogWarning($"[{nameof(GameManager)}] A ball is already jumping! Ignoring selection until it completes.");
                return;
            }

            int hitCount = 1;
            if (ballProvider.TryGetAttribute<ToughnessAttribute>(out var toughness))
            {
                hitCount = toughness.HitCount;
            }

            Debug.Log($"[{nameof(GameManager)}] Ball #{index} selected! (HitCount: {hitCount})");

            // Consume/remove the ball from AvailableBallScreen UI
            if (_availableBallScreen != null)
            {
                _availableBallScreen.ConsumeSelectedBall();
            }

            // Publish event to EventBus for BallController to spawn and launch the ball
            if (_contextProvider != null && _contextProvider.TryGetContext<GameEventBus>(out var eventBus))
            {
                eventBus.Publish(new BallSelectedEvent(ballProvider));
            }

            OnBallSelected?.Invoke(ballProvider, index);
        }

        public void SetLevels(List<LevelDataSo> levels)
        {
            _levels = levels;
        }

        private void OnDestroy()
        {
            if (_availableBallScreen != null)
            {
                _availableBallScreen.OnBallSelected -= HandleBallSelected;
            }

            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
