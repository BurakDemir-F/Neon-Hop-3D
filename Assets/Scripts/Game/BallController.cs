using System;
using System.Collections.Generic;
using Game.Pool.AddressablesLocal.Scripts.Runtime;
using General;
using UnityEngine;

namespace Game.Sorcerum
{
    public class BallController : MonoBehaviour
    {
        private IContextProvider _contextProvider;
        private GameEventBus _eventBus;
        private IPoolCollection _poolCollection;
        
        private Action<BallSelectedEvent> _ballSelectedEvent;
        public void Initialize(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
            _ballSelectedEvent = OnBallSelected;
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
            _eventBus?.UnRegister<BallSelectedEvent>(_ballSelectedEvent);

            _ballSelectedEvent = null;
        }

        private void OnBallSelected(BallSelectedEvent selectedBallInfo)
        {
            var attributeProvider = selectedBallInfo.AttributeProvider;

            if (!attributeProvider.TryGetAttribute<PoolObjectAttribute>(out var poolObjectAttribute))
                return;

            _contextProvider.TryGetContext<GameContext>(out var gameContext);

            var poolCollection = gameContext.PoolCollection;

            var ball = poolCollection.Get<BallBase>(poolObjectAttribute.PoolKey.PoolKey1);
            
        }
    }

    public class RunningBallsInfo
    {
        private HashSet<BallBase> _runningBalls = new();

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