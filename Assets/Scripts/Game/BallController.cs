using System;
using System.Collections;
using System.Collections.Generic;
using Game.Pool.AddressablesLocal.Scripts.Runtime;
using General;
using UnityEngine;

namespace Game.Sorcerum
{
    public class BallController : MonoBehaviour
    {
        [SerializeField] private AvailableBallScreen _availableBallScreen;

        private IContextProvider _contextProvider;
        private GameEventBus _eventBus;
        
        private Action<BallSelectedEvent> _ballSelectedEvent;
        private RunningBallsInfo _runningBallsInfo = new();

        public bool HasRunningBalls => _runningBallsInfo.HasRunningBalls;

        public void Initialize(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
            _ballSelectedEvent = OnBallSelected;

            if (_availableBallScreen == null)
            {
                if (_contextProvider != null && _contextProvider.TryGetContext<GameContext>(out var gameContext))
                {
                    _availableBallScreen = gameContext.AvailableBallScreen;
                }

                if (_availableBallScreen == null)
                {
                    _availableBallScreen = FindFirstObjectByType<AvailableBallScreen>();
                }
            }
        }

        public void Construct()
        {
            if (_contextProvider.TryGetContext<GameEventBus>(out _eventBus))
            {
                _eventBus.Register<BallSelectedEvent>(_ballSelectedEvent);
            }
        }

        public void Destruct()
        {
            StopAllCoroutines();
            _eventBus?.UnRegister<BallSelectedEvent>(_ballSelectedEvent);
            _ballSelectedEvent = null;
            _runningBallsInfo.Clear();
        }

        private void OnBallSelected(BallSelectedEvent selectedBallInfo)
        {
            StartCoroutine(OnBallSelectedRoutine(selectedBallInfo));
        }

        private IEnumerator OnBallSelectedRoutine(BallSelectedEvent selectedBallInfo)
        {
            var attributeProvider = selectedBallInfo.AttributeProvider;
            if (attributeProvider == null || _contextProvider == null)
                yield break;

            if (!_contextProvider.TryGetContext<GameContext>(out var gameContext))
                yield break;

            string poolKey = null;
            if (attributeProvider.TryGetAttribute<PoolObjectAttribute>(out var poolObjectAttribute) && poolObjectAttribute?.PoolKey != null)
            {
                poolKey = poolObjectAttribute.PoolKey.PoolKey1;
            }
            else if (attributeProvider is BallDataSo ballDataSo && ballDataSo.PoolKey != null)
            {
                poolKey = ballDataSo.PoolKey.PoolKey1;
            }

            if (string.IsNullOrEmpty(poolKey))
            {
                poolKey = "simpleBall";
            }

            var poolCollection = gameContext.PoolCollection;
            var ball = poolCollection.Get<BallBase>(poolKey);
            if (ball == null)
            {
                Debug.LogError($"[{nameof(BallController)}] Could not get ball from pool with key: '{poolKey}'");
                yield break;
            }

            ball.Initialize(attributeProvider, _contextProvider);
            _runningBallsInfo.AddRunningBall(ball);

            // Await the IEnumerator returned by ball.Jump()
            var jumpEnumerator = ball.Jump();
            if (jumpEnumerator != null)
            {
                yield return StartCoroutine(jumpEnumerator);
            }

            // Ardından ball'ın sayısına bakılır. Sıfırdan büyükse availableBallscreen'e geri gönderilir.
            if (ball.RemainingJump > 0)
            {
                var returnedCollection = new AttributeCollection();
                foreach (var attr in ball.AttributeCollection)
                {
                    returnedCollection.UpdateObject(attr);
                }
                returnedCollection.UpdateObject(new ToughnessAttribute(ball.RemainingJump));

                var screen = _availableBallScreen ?? gameContext.AvailableBallScreen ?? FindFirstObjectByType<AvailableBallScreen>();
                if (screen != null)
                {
                    screen.AddBall(returnedCollection);
                }
            }

            // Sahnedeki top havuza geri döner
            if (ball.Go.activeInHierarchy)
            {
                ball.ReturnToPool();
            }

            _runningBallsInfo.RemoveBall(ball);
        }
    }

    public class RunningBallsInfo
    {
        private HashSet<BallBase> _runningBalls = new();

        public int Count => _runningBalls.Count;
        public bool HasRunningBalls => _runningBalls.Count > 0;

        public bool AddRunningBall(BallBase ballBase)
        {
            return _runningBalls.Add(ballBase);
        }

        public bool RemoveBall(BallBase ballBase)
        {
            return _runningBalls.Remove(ballBase);
        }

        public void Clear()
        {
            _runningBalls.Clear();
        }
    }
}