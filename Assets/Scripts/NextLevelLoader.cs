using UnityEngine;
using UnityEngine.Tilemaps;


public class NextLevelLoader : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    
    private void Awake()
    {
        tilemap.SetTile(new Vector3Int(0,-1,0), null);
    }
}
