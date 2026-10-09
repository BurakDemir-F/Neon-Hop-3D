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
            if (owner == null)
                yield break;

            if (!_context.TryGetContext<GameContext>(out var gameContext) || gameContext == null)
                yield break;

            var nextPositionProvider = gameContext.NextPositionProvider;
            if (nextPositionProvider == null)
                yield break;

            owner.transform.position = gameContext.BallStartPos;

            owner.AttributeCollection.TryGetAttribute<MovementAttribute>(out var movementAttribute);
            float moveDuration = movementAttribute != null && movementAttribute.MoveDuration > 0f
                ? movementAttribute.MoveDuration
                : 0.35f;
            Ease ease = movementAttribute != null ? movementAttribute.Ease : Ease.OutQuad;

            var hasNextCell = nextPositionProvider.TryGetNextPosition(Vector2Int.zero, false, out var nextCell);

            while (hasNextCell && workChecker.ShouldWork && owner.RemainingJump > 0 && owner.Go.activeInHierarchy)
            {
                if (nextCell == null || nextCell.Stack == null)
                {
                    hasNextCell = nextPositionProvider.TryGetNextPosition(
                        new Vector2Int(nextCell != null ? nextCell.XPos : 0, nextCell != null ? nextCell.YPos : 0),
                        true,
                        out nextCell);
                    continue;
                }

                var hasItem = nextCell.Stack.HasItem();
                nextCell.Stack.GetTop(out var stackableItem);

                // Parabolic jump arc over to the top stackable item
                Vector3 targetPos = (hasItem && stackableItem != null ? stackableItem.Transform.position : nextCell.Stack.StackRoot.position) +
                                    Vector3.up * 0.4f;

                yield return owner.transform.DOJump(targetPos, 0.8f, 1, moveDuration)
                    .SetEase(ease)
                    .WaitForCompletion();

                // Match and resolve once for this stack (do NOT crack items underneath)
                gameContext.GameResolver.ResolveGame(owner, nextCell.Stack);

                if (owner.RemainingJump <= 0 || !owner.Go.activeInHierarchy)
                {
                    yield break;
                }

                // Advance to the next stack cell in the queue
                hasNextCell = nextPositionProvider.TryGetNextPosition(
                    new Vector2Int(nextCell.XPos, nextCell.YPos),
                    true,
                    out nextCell);
            }
        }
    }
}