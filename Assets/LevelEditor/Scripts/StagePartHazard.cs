using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HazardCard", menuName = "Hazards/NewHazard", order = 1)]
public class StagePartHazard : ScriptableObject
{
    public string Name;
    public GameObject hazardPrefab;
    public Sprite hazardSprite;
    public int size = 1;
    public float xOffset = 1;
    public float yOffset = 1;
    public bool isInteractable = false;
}
