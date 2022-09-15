using System;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    private static Vector2 blockSize = new Vector2(0.22f, 0.22f);

    public static void SetPosition(this Transform block, Vector2 gridPosition)
    {
        block.position = blockSize * gridPosition;
    }

    public static void SetName(this Transform block, Vector2 gridPosition)
    {
        block.name = gridPosition.x + "," + gridPosition.y;
    }

    public static void SetColor(this Transform block, ColorType colorType)
    {
        block.tag = colorType.ToString();
    }

    public static string GetColor(this Transform block)
    {
        return block.tag;
    }

    public static Vector2 GetGridPosition(this Transform block)
    {
        Vector2 gridPosition = Vector2Int.RoundToInt(new Vector2((block.position.x / blockSize.x), (block.position.y / blockSize.y)));
        return gridPosition;
    }

    public static Vector2Int GetGridPosition(this Vector3 position)
    {
        Vector2Int gridPosition = Vector2Int.RoundToInt(new Vector2((position.x / blockSize.x), (position.y / blockSize.y)));
        return gridPosition;
    }

    public static Vector3 GetPositionByGridPosition(this Vector2 gridPosition)
    {
        Vector3 position = new Vector3(gridPosition.x * blockSize.x, gridPosition.y * blockSize.y, 0);
        return position;
    }

    public static Transform GetObjectByPosition(this Vector3 position)
    {
        // Debug.Log("raycast pos: " + position);
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        // If it hits something...
        if (hit.collider != null)
        {
            // Debug.Log("We hit " + hit.transform.name);
            return hit.transform;
        }

        return null;
    }

    public static Transform GetObjectByPosition(this Vector2 gridPosition)
    {
        return gridPosition.GetPositionByGridPosition().GetObjectByPosition();
    }

    public static void SetSprite(this Vector2 position, Sprite sprite)
    {
        GetObjectByPosition(position).GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public static void SetSprite(this Transform block, Sprite sprite)
    {
        block.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public static void SetSortingOrder(this Transform block, Vector2 gridPosition)
    {
        if (block.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            spriteRenderer.sortingOrder = (int) gridPosition.y;
        }
    }

    public static ColorType GetColorType(this Transform transform)
    {
        Enum.TryParse(transform.GetColor(), out ColorType colorType);
        return colorType;
    }

    public static ColorType GetColorType(this string color)
    {
        Enum.TryParse(color, out ColorType colorType);
        return colorType;
    }

    public static List<Transform> GetTransformsByRaycast(this Vector2 gridPosition, Vector2 direction)
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(gridPosition.GetPositionByGridPosition(), direction, 100);
        List<Transform> hittedTransforms = new List<Transform>();
        // If it hits something...
        if (hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                hittedTransforms.Add(hit[i].transform);
            }

            return hittedTransforms;
        }

        return null;
    }

    public static void UpdateBlockProperties(this Transform block, Vector2 gridPosition)
    {
        block.SetName(gridPosition);
        block.SetSortingOrder(gridPosition);
    }

    public static Vector3 GetDistance(this int blockCount)
    {
        return blockCount * new Vector3(0, blockSize.y, 0);
    }
}