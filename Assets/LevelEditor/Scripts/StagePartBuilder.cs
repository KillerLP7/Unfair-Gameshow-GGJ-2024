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

    public GameObject CreateStageByStagePart(Dictionary<int,int> stagePart)
    {
        GameObject stage = Instantiate(BaseStagePrefab);
        //stage.SetActive(false);

        Dictionary<int, GameObject> hazards = new Dictionary<int, GameObject>();
        foreach (int item in stagePart.Keys)
        {
            hazards[item] = Instantiate(AllStageHazards[stagePart[item]].hazardPrefab, stage.transform);
            hazards[item].transform.localPosition = new Vector3(item - xOffsetHeightOfHazards, -yOffsetOfHazards, 0);
        }
        

        return null;
    }
}
