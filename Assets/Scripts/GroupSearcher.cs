using System;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;


[Serializable]
public class ColorTypeAndIcons
{
    //color type - icons for gruops and default icon .. 
    public ColorType colorType;
    public List<Sprite> icons;
}

[Serializable]
public class GroupTypeAndMinNumber
{
    public GroupType groupType;
    public int minNumber;
}

public enum GroupType
{
    Default,
    A,
    B,
    C,
}

public class GroupSearcher
{
    private ColorsAndIcons colorsAndIcons;
    private List<GroupTypeAndMinNumber> groupTypeAndMinNumbers;
    private List<Vector2> directions = new List<Vector2>() {Vector2.right, Vector2.left, Vector2.up, Vector2.down};

    public void SetValues(ColorsAndIcons colorsAndIcons, List<GroupTypeAndMinNumber> groupTypeAndMinNumbers)
    {
        this.colorsAndIcons = colorsAndIcons;
        this.groupTypeAndMinNumbers = groupTypeAndMinNumbers;
    }

    public void SearchForGroups(Vector2 gameAreaSize) //, out List<List<Vector2>> groups, out List<Vector2> ungroupedElements
    {
        Vector2 gridPos;
        List<Vector2> group = new List<Vector2>();
        List<Vector2> groupedElements = new List<Vector2>();
        for (int i = 0; i < gameAreaSize.x; i++)
        {
            for (int j = 0; j < gameAreaSize.y; j++)
            {
                gridPos = new Vector2(i, j);
                if (!groupedElements.Contains(gridPos) && gridPos.GetObjectByPosition() != null)
                {
                    SearchForGroupByBlock(gridPos.GetObjectByPosition(), gameAreaSize, out group);
                    if (group.Count > 1)
                    {
                        groupedElements.AddRange(group);
                        SetGroupIcons(group);
                    }
                    else
                    {
                        SetIcon(gridPos);
                    }
                }
            }
        }
    }

    public void SearchForGroupByBlock(Transform block, Vector2 gameAreaSize, out List<Vector2> group)
    {
        List<Vector2> groupNeighbours = new List<Vector2>();
        List<Vector2> unsearchedNeighbours = new List<Vector2>();
        unsearchedNeighbours.Add(block.GetGridPosition());
        do
        {
            GetSameColorNeighbours(unsearchedNeighbours[0], gameAreaSize, block.tag, ref unsearchedNeighbours, ref groupNeighbours);
        } while (unsearchedNeighbours.Count > 0);

        group = groupNeighbours;
    }

    private void GetSameColorNeighbours(Vector2 gridPosition, Vector2 gameAreaSize, string color, ref List<Vector2> unsearchedNeighbours, ref List<Vector2> groupNeighbours)
    {
        Vector2 neighbourPosition;
        //raycast for 4 direction
        for (int i = 0; i < directions.Count; i++)
        {
            neighbourPosition = GetNeighbourGridPosition(gridPosition, directions[i]);
            if (!groupNeighbours.Contains(neighbourPosition) && IsCorrectGridPosition(neighbourPosition, gameAreaSize) && IsSameColor(neighbourPosition, color))
            {
                unsearchedNeighbours.Add(neighbourPosition);
                groupNeighbours.Add(neighbourPosition);
            }
        }

        unsearchedNeighbours.Remove(gridPosition);
    }

    private Vector2 GetNeighbourGridPosition(Vector2 gridPosition, Vector2 neighbourDirection)
    {
        return gridPosition + neighbourDirection;
    }

    private bool IsCorrectGridPosition(Vector2 gridPosition, Vector2 gameAreaSize)
    {
        if ((gridPosition.x >= 0 && gridPosition.x < gameAreaSize.x) &&
            (gridPosition.y >= 0 && gridPosition.y < gameAreaSize.y))
        {
            return true;
        }

        return false;
    }

    private bool IsSameColor(Vector2 neighbourPosition, string color)
    {
        Transform neighbour = neighbourPosition.GetPositionByGridPosition().GetObjectByPosition();
        if (neighbour)
        {
            if (neighbour.CompareTag(color))
            {
                return true;
            }
        }

        return false;
    }

    public void SetGroupIcons(List<Vector2> group)
    {
        Enum.TryParse(group[0].GetObjectByPosition().GetColor(), out ColorType colorType);
        GroupType groupType = GetGroupType(group.Count);
        Sprite icon = colorsAndIcons.GetColorIcon(colorType, groupType);
        for (int i = 0; i < group.Count; i++)
        {
            group[i].SetSprite(icon);
        }
    }

    public void SetIcon(Vector2 gridPosition)
    {
        Enum.TryParse(gridPosition.GetObjectByPosition().GetColor(), out ColorType colorType);
        Sprite icon = colorsAndIcons.GetColorIcon(colorType, GroupType.Default);
        gridPosition.SetSprite(icon);
    }

    public Sprite GetColorIcon(ColorType colorType, GroupType groupType)
    {
        return colorsAndIcons.GetColorIcon(colorType, groupType);
    }

    private GroupType GetGroupType(int groupSize)
    {
        GroupTypeAndMinNumber groupTypeAndMinNumber = groupTypeAndMinNumbers.FindLast(groupType => groupType.minNumber <= groupSize);
        // Debug.Log("group size: " + groupSize + " , groupType " + groupTypeAndMinNumber.groupType + " min number " + groupTypeAndMinNumber.minNumber);
        return groupTypeAndMinNumbers.FindLast(groupType => groupType.minNumber <= groupSize).groupType;
    }
}