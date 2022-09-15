using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ColorType
{
    Red,
    Green,
    Blue,
    Yellow,
    Purple,
    Pink
}

public class GameArea
{
    public Vector2 size;
    public int colorNumber;

    // public List<,> colors = 
    public GameArea(Vector2 size, int colorNumber)
    {
        this.size = size;
        this.colorNumber = colorNumber;
    }
}

public class GameAreaCreator : MonoBehaviour
{
    [Header("***Game Area Settings***")] [SerializeField]
    private GameObject block;

    [SerializeField] [Range(2, 10)] private int rowNumber, columnNumber;
    [SerializeField] [Range(2, 6)] private int colorNumber;
    [SerializeField] private GameController gameController;
    [SerializeField] private PoolController poolController;

    private void Start()
    {
        GameArea gameArea = new GameArea(new Vector2(columnNumber, rowNumber), colorNumber);
        gameController.SetGameArea(gameArea);
        GenerateBoard();
        StartCoroutine(gameController.WaitForUpdateGroups(new WaitForEndOfFrame()));
    }

    public void SetPoolController(PoolController poolController)
    {
        this.poolController = poolController;
    }

    private void GenerateBoard()
    {
        for (int i = 0; i < columnNumber; i++)
        {
            for (int j = 0; j < rowNumber; j++)
            {
                CreateAndPrepareBlock(new Vector2(i, j));
            }
        }
    }

    public void CreateNewBlocks(ref Dictionary<Vector2, List<Vector2>> newPoints, ref Dictionary<Vector2, List<Transform>> newTransforms)
    {
        List<Vector2> points = new List<Vector2>();
        for (int i = 0; i < newPoints.Count; i++)
        {
            List<Transform> newTransformsOnColumn = new List<Transform>();
            points = newPoints.ElementAt(i).Value;
            for (int j = 0; j < points.Count; j++)
            {
                newTransformsOnColumn.Add(CreateAndPrepareBlock(points[j]));
            }

            newTransforms.Add(points[0], newTransformsOnColumn);
        }
    }

    private Transform CreateAndPrepareBlock(Vector2 gridPosition)
    {
        Transform newBlock = CreateNewBlock();
        PrepareNewBlock(ref newBlock, gridPosition);
        return newBlock;
    }

    private Transform CreateNewBlock()
    {
        Transform newBlock = null;
        if (!poolController.TryGetBlock(ref newBlock))
        {
            newBlock = Instantiate(block, transform).transform;
        }

        newBlock.gameObject.SetActive(true);
        return newBlock;
    }

    private void PrepareNewBlock(ref Transform newBlock, Vector2 gridPosition)
    {
        newBlock.SetPosition(gridPosition);
        newBlock.SetName(gridPosition);
        newBlock.SetColor(SelectRandomColor());
        newBlock.SetSprite(gameController.GetColorIcon(newBlock.transform.GetColorType(), GroupType.Default));
        newBlock.SetSortingOrder(gridPosition);
    }

    private ColorType SelectRandomColor()
    {
        int randomNumber = Random.Range(0, colorNumber);
        ColorType randomColor = (ColorType) randomNumber;
        return randomColor;
    }
}