using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class ScrollController
{
    public void ScrollNewBlocks(ref Dictionary<Vector2, List<Transform>> spawnedTransforms)
    {
        List<Transform> blocksOnColumn = new List<Transform>();
        for (int i = 0; i < spawnedTransforms.Count; i++)
        {
            blocksOnColumn = spawnedTransforms.ElementAt(i).Value;
            for (int j = 0; j < blocksOnColumn.Count; j++)
            {
                ScrollBlock(blocksOnColumn[j], blocksOnColumn.Count);
            }
        }
    }

    public void ScrollUpperBlocks(ref Dictionary<Vector2, List<Vector2>> bottomPosAndPointsOnColumn)
    {
        for (int i = 0; i < bottomPosAndPointsOnColumn.Count; i++)
        {
            ScrollUpperBlocksByBlastedPoints(bottomPosAndPointsOnColumn.ElementAt(i).Key, bottomPosAndPointsOnColumn.ElementAt(i).Value);
        }
    }

    private void ScrollUpperBlocksByBlastedPoints(Vector2 bottomPosition, List<Vector2> pointsOnColumn)
    {
        List<Transform> upperBlocks = new List<Transform>();
        GetUpperBlocksOnColumn(bottomPosition, ref upperBlocks);
        if (upperBlocks != null)
        {
            for (int j = 0; j < upperBlocks.Count; j++)
            {
                ScrollBlock(upperBlocks[j], pointsOnColumn);
            }
        }
    }

    private void GetUpperBlocksOnColumn(Vector2 bottomPosition, ref List<Transform> upperBlocks)
    {
        upperBlocks = bottomPosition.GetTransformsByRaycast(Vector2.up);
    }

    private void ScrollBlock(Transform block, List<Vector2> pointsOnSameColumn)
    {
        int belowElementCount = pointsOnSameColumn.FindAll(x => block.GetGridPosition().y > x.y).ToList().Count; //GetBelowElementsCount(block, transformsOnSameColumn);
        ScrollBlock(block, belowElementCount);
    }

    private void ScrollBlock(Transform block, int scrollCount)
    {
        Vector3 targetPosition = block.position - scrollCount.GetDistance();
        block.DOMoveY(targetPosition.y, scrollCount * 0.05f).OnComplete(() => { block.UpdateBlockProperties(block.GetGridPosition()); });
    }
}