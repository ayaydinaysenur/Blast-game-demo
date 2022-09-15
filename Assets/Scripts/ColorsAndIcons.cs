using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorsAndIcons", menuName = "ScriptableObjects/CreateColorAndIcons", order = 1)]
public class ColorsAndIcons : ScriptableObject
{
    public List<ColorTypeAndIcons> colorIcons;

    public Sprite GetColorIcon(ColorType colorType, GroupType groupType)
    {
        // Debug.Log("group type: " + groupType);
        return colorIcons.Find(x => x.colorType == colorType).icons[(int) groupType];
    }
}