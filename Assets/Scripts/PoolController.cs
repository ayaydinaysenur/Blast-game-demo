using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolController : MonoBehaviour
{
    private List<Transform> blockPool = new List<Transform>();

    public void AddBlocksToPool(List<Transform> blocks)
    {
        blockPool.AddRange(blocks);
        // Debug.Log("Add blocks to pool " + blocks.Count);
    }

    public bool TryGetBlock(ref Transform block)
    {
        if (blockPool != null && blockPool.Count > 20)
        {
            block = blockPool[blockPool.Count - 1];
            blockPool.Remove(block);
            return true;
        }

        return false;
    }
}