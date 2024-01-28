using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class playerCreatedLevel
{
    public GameObject level;
    public int iD;
    public List<GameObject> triggerObj;
}
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _spawnPoint;
    [SerializeField] private PlayerController _playerPrefab;
    [SerializeField] private CameraFollow _cameraFollow;
    [SerializeField] private float nextStageOffsetX = 10;
    [SerializeField] private int maximumLoadedStages = 10;
    [SerializeField] GameObject stageBuilderPrefab;
    [SerializeField] StagePartBuilder stageBuilder;
    public StagePartBuilder StageBuilder
    {
        get
        {
            if (stageBuilder == null)
            {
                stageBuilder = Instantiate(stageBuilderPrefab).GetComponent<StagePartBuilder>();
            }
            return stageBuilder;
        }
    }

    private List<GameObject> createdStageObjects = new List<GameObject>();



    private PlayerController _player;
    private GameObject _lastCheckPoint;
    private Vector3 stagePos;
    private List<Dictionary<int, int>> _playerLevels = new List<Dictionary<int, int>>();
    private List<playerCreatedLevel> _playerCreatedLevels = new();
    private Dictionary<int, int> _playerLevel;
    private int _lastPlayerID = -1;
    private bool _bFirstLevelLoad = true;

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
        if (SpawnPoint.Instance != null)
        {
            _spawnPoint = SpawnPoint.Instance.gameObject;
        }
        Debug.Log(_spawnPoint.transform.ToString());
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

        float x = _lastCheckPoint.transform.position.x - 3;
        float toX = x + 16;

        Camera.main.GetComponent<CameraFollow>().SetBounds(new Vector2(x, toX));
        //limmit player and camera movement
        //_cameraFollow.SetBounds(new Vector2(_player.transform.position.x, _player.transform.position.y));
        //_player.SetBound(9);
        //TODO::Sound
    }

    //TODO: add _cameraFollow.SetBounds(_player.transform.position.x, _player.transform.position.y); To limmet the camera movment on the left side
    public void LoadNextStage()
    {

        if (createdStageObjects.Count >= maximumLoadedStages)
        {
            Destroy(createdStageObjects[0]);
            createdStageObjects.RemoveAt(0);
        }
        //stagePos = Vector3.zero;
        stagePos.x += nextStageOffsetX;
        //Random.Range(0, )
        GameObject levelToLoad;
        if (_playerLevels.Count == 0)
        {
            int randomLevelNR = 0;
            if (_bFirstLevelLoad)
            {
                _bFirstLevelLoad = false;
            }
            else
            {
                randomLevelNR = Random.Range(0, stageParts.Length);
            }
           
            levelToLoad = Instantiate(stageParts[randomLevelNR], stagePos, Quaternion.identity);
        }
        else
        {
            playerCreatedLevel tempPlayerLevel = new playerCreatedLevel();
            tempPlayerLevel.level = StageBuilder.CreateStageByStagePart(_playerLevels[0], stagePos, out tempPlayerLevel.triggerObj);
            tempPlayerLevel.iD = _playerLevels.First()[0];
            _playerCreatedLevels.Add(tempPlayerLevel);
            levelToLoad = tempPlayerLevel.level;
        }
        createdStageObjects.Add(levelToLoad);
    }

    public void CheckIsLevelPartByPlayer(GameObject pLevel)
    {
        if (_lastPlayerID >= 0)
            LevelFinshedFromOberverX(_lastPlayerID);

        _lastPlayerID = -1;
        foreach (var item in _playerCreatedLevels)
        {
            if (item.level == pLevel)
            {
                LevelLoadedFromOberverX(item.iD, item.triggerObj.Count);
                _lastPlayerID = item.iD;
            }
        }
    }
    // int id = Id of the Observer / levelcreator
    public void SetLevel(Dictionary<int, int> level, int id)
    {
        _playerLevels.Append(level);
    }

    // Nofify that observers level is ready loaded
    public void LevelLoadedFromOberverX(int pOberverID, int amountOfStuffToInteractWith)
    {
        NetManager.singleton.Player.CmdLevelLoaded(pOberverID, amountOfStuffToInteractWith);
    }

    // Nofify that observers level is finished (player run throug)
    public void LevelFinshedFromOberverX(int pOberverID)
    {
        NetManager.singleton.Player.CmdLevelFinshed(pOberverID);
    }
    // Observer interacts with level
    public void ObserverInteractWithLevel(int netId, int interactable)
    {
        if (!(_lastPlayerID == netId)) return;
        foreach (var classItem in _playerCreatedLevels)
        {
            if (classItem.iD == netId)
            {
                for (int i = 0; i < classItem.triggerObj.Count; i++)
                {
                    if (i == interactable)
                    {
                        if (classItem.triggerObj[i].TryGetComponent(out BaseTriggerdObj baseTriggerd))
                        {
                            baseTriggerd.TriggerEffect();
                        }
                    }
                }
            }
        }
    }

    // Player died on level with netID // add points server


    public void DestroyStage()
    {
        createdStageObjects.RemoveAt(0);
    }
    public PlayerController GetPlayerController() { return _player; }
}
