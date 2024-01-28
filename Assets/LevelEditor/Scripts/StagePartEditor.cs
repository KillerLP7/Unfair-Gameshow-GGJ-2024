using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StagePartEditor : MonoBehaviour
{
    public static StagePartEditor instance;

    [SerializeField] private StagePartBuilder stageBuilder;
    [SerializeField] private RectTransform hazardToolboxContent;
    [SerializeField] private GameObject hazardToolboxElementPrefab;
    [SerializeField] private GameObject hazardGridDisplayedPrefab;

    [SerializeField] private Button[] manualActivationButtons;

    [SerializeField] private Grid stageGrid;
    [SerializeField] LayerMask gridLayer;

    [SerializeField] int verticalCellLine = 1;
    [SerializeField] int maxAvailablePoints = 5;
    [SerializeField] bool spaceRequiredBeetweenHazards = true;
    [SerializeField] TextMeshProUGUI availablePointsText;
    [SerializeField] TextMeshProUGUI maxPointsText;
    int currentUsedPoints
    {
        get
        {
            int points = 0;
            foreach (int item in placedHazards.Values)
            {
                points += stageBuilder.AllStageHazards[item].size;
            }
            return points;
        }
    }

    private Dictionary<int, int> placedHazards = new Dictionary<int, int>();
    private Dictionary<int, Transform> displayedHazardIcons = new Dictionary<int, Transform>();

    private List<RectTransform> activeHazardToolboxElements = new List<RectTransform>();
    private int currentSelectedHazard = 0;
    private Canvas canvas;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        canvas = stageGrid.transform.GetComponentInParent<Canvas>();
        CreateToolbox();
        SetManualActivationKeysActive(0);
        SelectHazardToPlace(0);
        if (maxPointsText != null)
        {
            maxPointsText.text = "" + (maxAvailablePoints);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaceHazardInGridAtPos(Input.mousePosition);
        }

        if (availablePointsText != null)
        {
            availablePointsText.text = "" + (maxAvailablePoints - currentUsedPoints);
        }
    }

    private void CreateToolbox()
    {
        if (stageBuilder == null || hazardToolboxContent == null || hazardToolboxElementPrefab == null || stageGrid == null)
        {
            Debug.LogError("Not all references are set correctly for StagePartEditor");
            return;
        }

        foreach (RectTransform item in activeHazardToolboxElements)
        {
            Destroy(item.gameObject);
        }
        activeHazardToolboxElements.Clear();

        for (int i = hazardToolboxContent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(hazardToolboxContent.transform.GetChild(i).gameObject);
        }

        int hazardIterator = 0;
        foreach (StagePartHazard hazard in stageBuilder.AllStageHazards)
        {
            int currentIterator = hazardIterator;
            hazardIterator++;

            GameObject newHazardToolboxEntry = Instantiate(hazardToolboxElementPrefab, hazardToolboxContent);

            newHazardToolboxEntry.GetComponent<Button>().onClick.AddListener(() => { SelectHazardToPlace(currentIterator); });
            newHazardToolboxEntry.transform.Find("Icon").GetComponent<Image>().sprite = hazard.hazardSprite;
            newHazardToolboxEntry.transform.Find("SelectedBackground").gameObject.SetActive(false);

            if (hazard.size > 0)
            {
                newHazardToolboxEntry.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = "" + hazard.size;
            }
            else
            {
                newHazardToolboxEntry.transform.Find("Cost").gameObject.SetActive(false);
                newHazardToolboxEntry.transform.Find("bgCost").gameObject.SetActive(false);
            }

            activeHazardToolboxElements.Add(newHazardToolboxEntry.GetComponent<RectTransform>());

        }
    }

    private void SelectHazardToPlace(int index)
    {
        currentSelectedHazard = index;
        for (int i = 0; i < activeHazardToolboxElements.Count; i++)
        {
            activeHazardToolboxElements[i].transform.Find("SelectedBackground").gameObject.SetActive(i == index);
        }
    }

    private void PlaceHazardInGridAtPos(Vector3 targetPosition)
    {
        if (stageBuilder.AllStageHazards[currentSelectedHazard].size <= 0)
        {
            RectTransform rt = stageGrid.transform.GetComponent<RectTransform>();
            int cellVerticalCount = (int)(rt.rect.height / stageGrid.cellSize.y);
            int cellHorizontalCount = (int)(rt.rect.width / stageGrid.cellSize.x);
            Vector3Int cell = stageGrid.WorldToCell(targetPosition);

            if (cell.x >= 0 && cell.x < cellHorizontalCount && cell.y >= 0 && cell.y < cellVerticalCount)
            {
                if (placedHazards.ContainsKey(cell.x))
                {
                    Destroy(displayedHazardIcons[cell.x].gameObject);
                    displayedHazardIcons.Remove(cell.x);
                    placedHazards.Remove(cell.x);
                }
            }
        }
        else if (stageBuilder.AllStageHazards[currentSelectedHazard].size <= maxAvailablePoints - currentUsedPoints)
        {
            RectTransform rt = stageGrid.transform.GetComponent<RectTransform>();
            int cellVerticalCount = (int)(rt.rect.height / stageGrid.cellSize.y);
            int cellHorizontalCount = (int)(rt.rect.width / stageGrid.cellSize.x);
            Vector3Int cell = stageGrid.WorldToCell(targetPosition);

            if (cell.x >= 0 && cell.x <= cellHorizontalCount - stageBuilder.AllStageHazards[currentSelectedHazard].size && cell.y >= 0 && cell.y < cellVerticalCount)
            {
                if (!spaceRequiredBeetweenHazards || !IsCellToCloseToPlacedHazard(cell.x, stageBuilder.AllStageHazards[currentSelectedHazard].size))
                {
                    for (int i = 0; i < stageBuilder.AllStageHazards[currentSelectedHazard].size; i++)
                    {
                        if (placedHazards.ContainsKey(cell.x + i))
                        {
                            Destroy(displayedHazardIcons[cell.x + i].gameObject);
                            displayedHazardIcons.Remove(cell.x + i);
                            placedHazards.Remove(cell.x + i);
                        }
                    }
                    placedHazards[cell.x] = currentSelectedHazard;
                    displayedHazardIcons[cell.x] = Instantiate(hazardGridDisplayedPrefab, stageGrid.CellToWorld(new Vector3Int(cell.x, verticalCellLine)), Quaternion.identity, stageGrid.transform).transform;
                    displayedHazardIcons[cell.x].Find("Icon").GetComponent<Image>().sprite = stageBuilder.AllStageHazards[currentSelectedHazard].hazardSprite;
                    displayedHazardIcons[cell.x].Find("ExtraSizeBlocker").GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, displayedHazardIcons[cell.x].GetComponent<RectTransform>().rect.width * stageBuilder.AllStageHazards[currentSelectedHazard].size);
                }
            }
        }
    }

    public bool IsCellToCloseToPlacedHazard(int cellX, int size)
    {
        bool toCLose = false;
        for (int i = 0; i < size + 1; i++)
        {
            if (placedHazards.ContainsKey(cellX + i))
            {
                toCLose = true;
            }
        }
        int highestkey = -1;
        foreach (int item in placedHazards.Keys)
        {
            if (item > highestkey && item < cellX)
            {
                highestkey = item;
            }
        }
        if (highestkey >= 0 && cellX <= highestkey + stageBuilder.AllStageHazards[placedHazards[highestkey]].size)
        {
            toCLose = true;
        }
        return toCLose;
    }

    public void SetManualActivationKeysActive(int amount)
    {
        int counter = 0;
        foreach (Button item in manualActivationButtons)
        {
            item.interactable = amount > counter;
            counter++;
        }
    }

    public void ActivateManualActivation(int index)
    {
        if (NetManager.singleton != null)
        {
            NetManager.singleton.LocalObserver.CmdInteract(index);
        }
    }

    public void FinishEditAndSend()
    {
        //List<GameObject> test;
        //stageBuilder.CreateStageByStagePart(placedHazards, Vector3.zero, out test);
        if (NetManager.singleton != null)
        {
            NetManager.singleton.LocalObserver.CmdSendLevel(new List<int>(placedHazards.Keys), new List<int>(placedHazards.Values));
        }
        foreach (int item in placedHazards.Keys)
        {
            Destroy(displayedHazardIcons[item].gameObject);
        }
        displayedHazardIcons.Clear();
        placedHazards.Clear();
    }

    public void ClickOnGrid()
    {
        PlaceHazardInGridAtPos(Input.mousePosition);
    }

    private void OnDrawGizmos()
    {
        if (stageGrid != null)
        {
            Gizmos.color = Color.red;
            RectTransform rt = stageGrid.transform.GetComponent<RectTransform>();
            int cellVerticalCount = (int)(rt.rect.height / stageGrid.cellSize.y);
            int cellHorizontalCount = (int)(rt.rect.width / stageGrid.cellSize.x);
            for (int i = 0; i < cellHorizontalCount + 1; i++)
            {
                Gizmos.DrawLine(stageGrid.CellToWorld(new Vector3Int(i, 0)), stageGrid.CellToWorld(new Vector3Int(i, cellVerticalCount)));
            }
            for (int i = 0; i < cellVerticalCount + 1; i++)
            {
                Gizmos.DrawLine(stageGrid.CellToWorld(new Vector3Int(0, i)), stageGrid.CellToWorld(new Vector3Int(cellHorizontalCount, i)));
            }

        }
    }
}
