using System.Collections.Generic;
using General.GridSystem;
using UnityEngine;

namespace Game.Sorcerum
{
    public class StackQueueController : MonoBehaviour
    {
        private List<StackGridCell> _stackQueue = new();
        private INextPositionProvider _nextPositionProvider;

        public INextPositionProvider NextPositionProvider => _nextPositionProvider;

        public void Initialize(IReadOnlyList<StackGridCell> items)
        {
            _stackQueue.Clear();
            
            foreach (var itemStack in items)
                _stackQueue.Add(itemStack);

            _nextPositionProvider = new NextPositionProvider(_stackQueue);
        }
    }

    public class NextPositionProvider : INextPositionProvider
    {
        private IReadOnlyList<StackGridCell> _cellQueue;

        public NextPositionProvider(IReadOnlyList<StackGridCell> cellList)
        {
            _cellQueue = cellList;
        }
        
        public bool TryGetNextPosition(Vector2Int currentPos, bool hasCurrentPos, out StackGridCell nextCell)
        {
            nextCell = null;
            var stackQueueCount = _cellQueue.Count;

            if (!hasCurrentPos)
            {
                var hasItem = stackQueueCount > 0;

                nextCell = hasItem ? _cellQueue[0] : null;

                return hasItem;
            }

            for (var i = 0; i < _cellQueue.Count - 1; i++)
            {
                var itemStack = _cellQueue[i];
                
                if (((IGridCell)itemStack).Position != currentPos)
                    continue;
                
                nextCell = _cellQueue[i + 1];
                return true;
            }

            return false;
        }
    }

    public interface INextPositionProvider
    {
        bool TryGetNextPosition(Vector2Int currentPos, bool hasCurrentPos, out StackGridCell nextCell);
    }
}