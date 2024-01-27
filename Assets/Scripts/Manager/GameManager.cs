using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _spawnPoint;
    [SerializeField] private PlayerController _playerPrefab;
    [SerializeField] private CameraFollow _cameraFollow;

    private PlayerController _player;
    private GameObject _lastCheckPoint;
    private Vector3 stagePos;

    public static GameManager Instance { get; private set; }
    public GameObject[] stageParts;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        _player = Instantiate(_playerPrefab, _spawnPoint.transform.position, Quaternion.identity);
        _cameraFollow.toFollow = _player.gameObject;
        _lastCheckPoint = _spawnPoint;
    }

    // Update is called once per frame
    void Update()
    {

    }
    //TODO::loadLevel
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
        //TODO::Sound
    }

    public void LoadNextStage()
    {
        stagePos = transform.position;
        stagePos.x += 10;
        Instantiate(stageParts[0], stagePos, Quaternion.identity);
    }
}
