using System.Collections;
using DG.Tweening;
using General;
using General.GridSystem;
using UnityEngine;

namespace Game.Sorcerum
{
    [System.Serializable]
    public class JumpAttribute : WorkerAttribute<BallBase>
    {
        public override IEnumerator DoAttributeWork(BallBase owner, WorkChecker workChecker)
        {
            _context.TryGetContext<GameContext>(out var gameContext);

            var nextPositionProvider = gameContext.NextPositionProvider;

            owner.transform.position = gameContext.BallStartPos;

            owner.AttributeCollection.TryGetAttribute<MovementAttribute>(out var movementAttribute);
            
            var hasNextCell = nextPositionProvider.TryGetNextPosition(Vector2Int.zero, false, out var nextCell);
            
            if(!hasNextCell)
                yield break;
            
            var hasNextStack = nextCell.Stack.GetTop(out var stackableItem);

            while (hasNextStack && workChecker.ShouldWork)
            {
                yield return owner.transform.DOMove(stackableItem.Transform.position, movementAttribute.MoveDuration)
                    .SetEase(movementAttribute.Ease).WaitForCompletion();

                hasNextCell = nextPositionProvider.TryGetNextPosition(new Vector2Int(nextCell.XPos,
                        nextCell.YPos),
                    true,
                    out nextCell);
                
                if(!hasNextCell)
                    break;
                
                hasNextStack = nextCell.Stack.GetTop(out stackableItem);
            }
        }
        
    }
}