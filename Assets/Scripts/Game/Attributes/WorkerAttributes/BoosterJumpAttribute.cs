using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using General;
using General.GridSystem;
using UnityEngine;

namespace Game.Sorcerum
{
    /// <summary>
    /// Dedicated JumpAttribute for BoosterBall.
    /// Sweeps smoothly over all stacks with items, cracking the top item regardless of color.
    /// </summary>
    [System.Serializable]
    public class BoosterJumpAttribute : JumpAttribute
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

            // Collect target stack cells that currently have items
            var targetCells = new List<StackGridCell>();
            if (nextPositionProvider.CellQueue != null)
            {
                foreach (var cell in nextPositionProvider.CellQueue)
                {
                    if (cell != null && cell.Stack != null && cell.Stack.HasItem())
                    {
                        targetCells.Add(cell);
                    }
                }
            }

            // Fallback traversal if CellQueue was null or empty
            if (targetCells.Count == 0)
            {
                var hasNext = nextPositionProvider.TryGetNextPosition(Vector2Int.zero, false, out var cell);
                while (hasNext && cell != null)
                {
                    if (cell.Stack != null && cell.Stack.HasItem())
                    {
                        targetCells.Add(cell);
                    }
                    hasNext = nextPositionProvider.TryGetNextPosition(new Vector2Int(cell.XPos, cell.YPos), true, out cell);
                }
            }

            // Position booster ball at launch origin
            owner.transform.position = gameContext.BallStartPos;

            if (targetCells.Count == 0)
            {
                // No stacks to crack; fly up and exit
                yield return owner.transform.DOMove(gameContext.BallStartPos + Vector3.up * 2f + Vector3.forward * 2f, 0.4f)
                    .SetEase(Ease.OutQuad)
                    .WaitForCompletion();
                yield break;
            }

            // Smoothly fly directly over all stacks in succession (no bouncing)
            for (int i = 0; i < targetCells.Count; i++)
            {
                if (workChecker != null && !workChecker.ShouldWork)
                    yield break;

                if (!owner.Go.activeInHierarchy)
                    yield break;

                var targetCell = targetCells[i];
                if (targetCell == null) continue;

                Vector3 targetPos = GetCruisingPosition(targetCell);

                // Swift transit over each stack
                float duration = (i == 0) ? 0.22f : 0.13f;
                Ease ease = (i == 0) ? Ease.InSine : Ease.Linear;

                yield return owner.transform.DOMove(targetPos, duration)
                    .SetEase(ease)
                    .WaitForCompletion();

                // Crack top item regardless of color
                if (targetCell.Stack != null && targetCell.Stack.HasItem() && targetCell.Stack.GetTop(out var topItem) && topItem != null)
                {
                    topItem.CurrentStack?.RemoveFromStack();
                    topItem.Crack();
                }
            }

            // Exit swoosh: fly upward and away after passing all stacks
            Vector3 exitPos = owner.transform.position + Vector3.up * 2.2f + Vector3.forward * 2.5f;
            yield return owner.transform.DOMove(exitPos, 0.32f)
                .SetEase(Ease.InQuad)
                .WaitForCompletion();
        }

        private Vector3 GetCruisingPosition(StackGridCell cell)
        {
            if (cell != null && cell.Stack != null)
            {
                if (cell.Stack.HasItem() && cell.Stack.GetTop(out var topItem) && topItem != null)
                {
                    return topItem.Transform.position + Vector3.up * 0.45f;
                }

                if (cell.Stack.StackRoot != null)
                {
                    return cell.Stack.StackRoot.position + Vector3.up * 0.45f;
                }
            }

            return (cell != null ? cell.transform.position : Vector3.zero) + Vector3.up * 0.45f;
        }
    }
}
