using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private string initialScene;
    [SerializeField] private string globalsScene;

    public static GameStarter Instance { get; private set; }

    private void Awake()
    {
        SceneManager.LoadScene(globalsScene, LoadSceneMode.Additive);
        Instance = this;
    }

    private void Start()
    {
        SceneManager.LoadSceneAsync(initialScene);
    }
}
