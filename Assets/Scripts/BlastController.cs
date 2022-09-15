using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlastController
{
    private PoolController poolController;

    public void SetPoolController(PoolController poolController)
    {
        this.poolController = poolController;
    }

    public void Blast(List<Vector2> group, ref Dictionary<Vector2, List<Vector2>> bottomPosAndUpperPoints)
    {
        List<Transform> blastedBlocks = new List<Transform>();
        for (int i = 0; i < group.Count; i++)
        {
            blastedBlocks.Add(group[i].GetObjectByPosition());
            group[i].GetObjectByPosition().gameObject.SetActive(false);
        }

        poolController.AddBlocksToPool(blastedBlocks);
        GroupObjectsByPosition(group, ref bottomPosAndUpperPoints);
    }

    private void GroupObjectsByPosition(List<Vector2> group, ref Dictionary<Vector2, List<Vector2>> bottomPosAndUpperPoints)
    {
        var grouped = group.OrderByDescending(x => x.y).GroupBy(x => x.x).ToList();
        for (int i = 0; i < grouped.Count; i++)
        {
            bottomPosAndUpperPoints.Add(grouped[i].LastOrDefault(), grouped[i].ToList());
        }
    }
}