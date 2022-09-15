using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<GroupTypeAndMinNumber> groupTypeAndMinNumbers;
    [SerializeField] private ColorsAndIcons colorsAndIcons;
    [SerializeField] private GameAreaCreator gameAreaCreator;
    [SerializeField] private PoolController poolController;
    [SerializeField] private Toggle toggle;
    [SerializeField] private List<RectTransform> texts;
    private Transform clickedBlock;
    private GameArea gameArea;
    private GroupSearcher groupSearcher;
    private BlastController blastController;
    private ScrollController scrollController;
    private SpawnController spawnController;
    private Dictionary<Vector2, List<Vector2>> bottomPosAndUpperPoints = new Dictionary<Vector2, List<Vector2>>();
    private Dictionary<Vector2, List<Vector2>> spawnPoints = new Dictionary<Vector2, List<Vector2>>();
    private Dictionary<Vector2, List<Transform>> newBlocks = new Dictionary<Vector2, List<Transform>>();
    private bool isAutoMode, isTimeUp;

    private void Awake()
    {
        groupSearcher = new GroupSearcher();
        blastController = new BlastController();
        scrollController = new ScrollController();
        spawnController = new SpawnController();
        blastController.SetPoolController(poolController);
        gameAreaCreator.SetPoolController(poolController);
        groupSearcher.SetValues(colorsAndIcons, groupTypeAndMinNumbers);
        // groupSearcher.SearchForGroups(gameArea.size);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAutoMode && isTimeUp)
        {
            Debug.Log("Play in Auto Mode");
            isTimeUp = false;
            do
            {
                clickedBlock = new Vector2(Random.Range(0, gameArea.size.x), Random.Range(0, gameArea.size.y)).GetObjectByPosition();
                // Debug.Log("Random clicked block " + clickedBlock.name);
            } while (clickedBlock == null);

            DoGameFlow();
        }

        else if (!isAutoMode && Input.GetMouseButtonDown(0))
        {
            clickedBlock = GetClickedBlock();
            if (clickedBlock != null)
            {
                DoGameFlow();
            }
        }

        ScrollText();
    }

    private void DoGameFlow()
    {
        bottomPosAndUpperPoints.Clear();
        spawnPoints.Clear();
        newBlocks.Clear();
        //blast clicked group and scroll upper blocks
        groupSearcher.SearchForGroupByBlock(clickedBlock, gameArea.size, out List<Vector2> group);
        blastController.Blast(group, ref bottomPosAndUpperPoints);
        scrollController.ScrollUpperBlocks(ref bottomPosAndUpperPoints);
        //create new blocks to fill empty space and scroll
        spawnController.Spawn(gameArea.size, ref bottomPosAndUpperPoints, ref spawnPoints);
        gameAreaCreator.CreateNewBlocks(ref spawnPoints, ref newBlocks);
        scrollController.ScrollNewBlocks(ref newBlocks);
        //refresh group icons when scroll's completed.
        StartCoroutine(WaitForUpdateGroups(new WaitForSeconds(0.25f)));
    }

    public IEnumerator WaitForUpdateGroups(YieldInstruction yieldInstruction)
    {
        yield return yieldInstruction;
        groupSearcher.SearchForGroups(gameArea.size);
        isTimeUp = true;
    }

    public Sprite GetColorIcon(ColorType colorType, GroupType groupType)
    {
        return groupSearcher.GetColorIcon(colorType, groupType);
    }

    public void SetGameArea(GameArea gameArea)
    {
        this.gameArea = gameArea;
    }

    private Transform GetClickedBlock()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition).GetObjectByPosition();
    }

    public void CallAsyncFunc(Action action)
    {
        StartCoroutine(AsyncFunc(action));
    }

    IEnumerator AsyncFunc(Action action)
    {
        yield return null;
        action?.Invoke();
    }

    private void ScrollText()
    {
        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].position += Time.deltaTime * Vector3.right * 20;
            if (texts[i].localPosition.x > 450)
            {
                texts[i].localPosition = new Vector3(-300, texts[i].localPosition.y, 0);
            }
        }
    }

    public void SetAutoMode()
    {
        Debug.Log("Is auto mode active: " + toggle.isOn);
        isAutoMode = toggle.isOn;
        isTimeUp = toggle.isOn;
    }
}