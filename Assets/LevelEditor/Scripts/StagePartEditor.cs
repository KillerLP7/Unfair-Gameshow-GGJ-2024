using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StagePartEditor : MonoBehaviour
{
    [SerializeField] private StagePartBuilder stageBuilder;
    [SerializeField] private RectTransform hazardToolboxContent;
    [SerializeField] private GameObject hazardToolboxElementPrefab;

    [SerializeField] private Grid stageGrid;
    [SerializeField] LayerMask gridLayer;

    private Dictionary<int, int> placedHazards = new Dictionary<int, int>();
    private Dictionary<int, Transform> displayedHazardIcons = new Dictionary<int, Transform>();

    private List<RectTransform> activeHazardToolboxElements = new List<RectTransform>();
    private int currentSelectedHazard = 0;
    private Canvas canvas;

    private void Start()
    {
        canvas = stageGrid.transform.GetComponentInParent<Canvas>();
        CreateToolbox();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaceHazardInGridAtPos(Input.mousePosition);
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

            activeHazardToolboxElements.Add(newHazardToolboxEntry.GetComponent<RectTransform>());

        }
    }

    private void SelectHazardToPlace(int index)
    {
        currentSelectedHazard = index;
    }

    private void PlaceHazardInGridAtPos(Vector3 targetPosition)
    {
        RectTransform rt = stageGrid.transform.GetComponent<RectTransform>();
        int cellVerticalCount = (int)(rt.rect.height / stageGrid.cellSize.y);
        int cellHorizontalCount = (int)(rt.rect.width / stageGrid.cellSize.x);
        Vector3Int cell = stageGrid.WorldToCell(targetPosition);

        if (cell.x >= 0 && cell.x <= cellHorizontalCount - stageBuilder.AllStageHazards[currentSelectedHazard].size && cell.y >= 0 && cell.y < cellVerticalCount)
        {
            for (int i = 0; i < stageBuilder.AllStageHazards[currentSelectedHazard].size; i++)
            {
                if (placedHazards.ContainsKey(cell.x+i))
                {
                    Destroy(displayedHazardIcons[cell.x + i].gameObject);
                    displayedHazardIcons.Remove(cell.x + i);
                    placedHazards.Remove(cell.x + i);
                } 
            }
            placedHazards[cell.x] = currentSelectedHazard;
            displayedHazardIcons[cell.x] = Instantiate(stageBuilder.AllStageHazards[currentSelectedHazard].hazardPrefab, stageGrid.CellToWorld(new Vector3Int(cell.x,1)), Quaternion.identity, canvas.transform).transform;
        }
    }

    public void FinishEditAndSend()
    {

    }

    private void OnDrawGizmos()
    {
        if (stageGrid != null)
        {
            Gizmos.color = Color.red;
            RectTransform rt = stageGrid.transform.GetComponent<RectTransform>();
            int cellVerticalCount = (int)(rt.rect.height / stageGrid.cellSize.y);
            int cellHorizontalCount = (int)(rt.rect.width / stageGrid.cellSize.x);
            for (int i = 0; i < cellHorizontalCount+1; i++)
            {
                Gizmos.DrawLine(stageGrid.CellToWorld(new Vector3Int(i, 0)), stageGrid.CellToWorld(new Vector3Int(i, cellVerticalCount)));
            }
            for (int i = 0; i < cellVerticalCount+1; i++)
            {
                Gizmos.DrawLine(stageGrid.CellToWorld(new Vector3Int(0, i)), stageGrid.CellToWorld(new Vector3Int(cellHorizontalCount, i)));
            }

        }
    }
}
