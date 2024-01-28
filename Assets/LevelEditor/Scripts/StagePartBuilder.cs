using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePartBuilder : MonoBehaviour
{
    public GameObject BaseStagePrefab;

    public float yOffsetOfHazards = 5;
    public float xOffsetHeightOfHazards = 5;

    [SerializeField]  private List<StagePartHazard> allStageHazards = new List<StagePartHazard>();
    public List<StagePartHazard> AllStageHazards { get { return allStageHazards; } }

    public GameObject CreateStageByStagePart(Dictionary<int,int> stagePart, Vector3 spawnPosition)
    {
        GameObject stage = Instantiate(BaseStagePrefab, spawnPosition, Quaternion.identity);

        Dictionary<int, GameObject> hazards = new Dictionary<int, GameObject>();
        foreach (int item in stagePart.Keys)
        {
            hazards[item] = Instantiate(AllStageHazards[stagePart[item]].hazardPrefab, stage.transform);
            hazards[item].transform.localPosition = new Vector3(item - xOffsetHeightOfHazards + AllStageHazards[stagePart[item]].xOffset, -yOffsetOfHazards + AllStageHazards[stagePart[item]].yOffset, 0);
        }
        
        return stage;
    }

    public GameObject CreateStageByStagePart(Dictionary<int, int> stagePart, Vector3 spawnPosition, out List<GameObject> interactableHazards)
    {
        GameObject stage = Instantiate(BaseStagePrefab, spawnPosition, Quaternion.identity);
        interactableHazards = new List<GameObject>();

        Dictionary<int, GameObject> hazards = new Dictionary<int, GameObject>();
        foreach (int item in stagePart.Keys)
        {
            hazards[item] = Instantiate(AllStageHazards[stagePart[item]].hazardPrefab, stage.transform);
            hazards[item].transform.localPosition = new Vector3(item - xOffsetHeightOfHazards + AllStageHazards[stagePart[item]].xOffset, -yOffsetOfHazards + AllStageHazards[stagePart[item]].yOffset, 0);
            if (AllStageHazards[stagePart[item]].isInteractable)
            {
                interactableHazards.Add(hazards[item]);
            }
        }

        return stage;
    }
}
