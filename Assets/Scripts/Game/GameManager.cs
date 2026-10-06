using System;
using Game.Pool.AddressablesLocal.Scripts.Runtime;
using General;
using UnityEngine;

namespace Game.Sorcerum
{
    /// <summary>
    /// Central manager responsible for bootstrapping the level, orchestrating LevelManager,
    /// and hooking up UI elements like AvailableBallScreen.
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

        [Header("Level Data")]
        [SerializeField] private LevelDataSo _currentLevelData;
        [SerializeField] private bool _autoBuildOnStart = true;
        
        private ContextProvider _contextProvider;

        // Events
        public event Action<LevelDataSo> OnLevelStarted;
        public event Action<IAttributeProvider, int> OnBallSelected;

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
                StartLevel(_currentLevelData);
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
            
            if (_masterPool != null)
            {
                _masterPool.CheckAndInitialize(_masterPool.transform);
            }
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

            OnLevelStarted?.Invoke(_currentLevelData);
            Debug.Log($"[{nameof(GameManager)}] Level successfully built and initialized with '{_currentLevelData.name}'!");
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
