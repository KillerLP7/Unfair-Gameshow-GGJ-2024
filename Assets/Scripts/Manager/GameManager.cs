using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _spawnPoint;
    [SerializeField] private PlayerController _playerPrefab;
    [SerializeField] private CameraFollow _cameraFollow;
    [SerializeField] private float x;

    private List<GameObject> createdStageObjects = new List<GameObject>();



    private PlayerController _player;
    private GameObject _lastCheckPoint;
    private Vector3 stagePos;

    public static GameManager Instance { get; private set; }
    public GameObject[] stageParts;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        _spawnPoint = SpawnPoint.Instance.gameObject;
        _player = Instantiate(_playerPrefab, _spawnPoint.transform.position, Quaternion.identity);
        _cameraFollow = Camera.main.GetComponent<CameraFollow>();
        _cameraFollow.toFollow = _player.gameObject;
        _lastCheckPoint = _spawnPoint;

        LoadNextStage();
        LoadNextStage();
        LoadNextStage();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //TODO:: void TriggerObs(int 0) if Aktuelller spielr = client
    //lsite mit objekten von Aktuellen Spieler �n der Scene
    public void PlayerDeath()
    {
        _player.gameObject.SetActive(false);
        UIManager.Instance.ShowGameOverScreen(PlayerReset);
        SoundManager.Instance.PlayDeathSound();
    }
    public void PlayerReset()
    {
        _player.transform.position = _lastCheckPoint.transform.position;
        _player.gameObject.SetActive(true);
        //TODO::Sound
    }

    public void UpdateCheckPoint(GameObject pCheckPoint)
    {
        _lastCheckPoint = pCheckPoint;
        //limmit player and camera movement
        //_cameraFollow.SetBounds(new Vector2(_player.transform.position.x, _player.transform.position.y));
        //_player.SetBound(9);
        //TODO::Sound
    }

    //TODO: add _cameraFollow.SetBounds(_player.transform.position.x, _player.transform.position.y); To limmet the camera movment on the left side
    public void LoadNextStage()
    {
        if (createdStageObjects.Count >= 5)
        {
            Destroy(createdStageObjects[0]);
            createdStageObjects.RemoveAt(0);
        }
        //stagePos = Vector3.zero;
        stagePos.x += x;
        //Random.Range(0, )
        createdStageObjects.Add(Instantiate(stageParts[0], stagePos, Quaternion.identity));
    }

    public void SetLevel(Dictionary<int, int> level)
    {
        foreach (var item in level.Keys)
        {
            Debug.Log(item + " " + level[item]);
        }
    }

    public void DestroyStage()
    {
        createdStageObjects.RemoveAt(0);
    }
    public PlayerController GetPlayerController() { return _player; }
}
