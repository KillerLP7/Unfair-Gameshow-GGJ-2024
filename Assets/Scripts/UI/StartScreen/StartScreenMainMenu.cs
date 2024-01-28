using UnityEngine;

public class StartScreenMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject matchList;

    private void Start()
    {
#if UNITY_SERVER
        gameObject.SetActive(false);
        matchList.SetActive(true);
#endif
        SoundManager.Instance.PlayTitleTheme();
    }

    public void ShowMatchList()
    {
        matchList.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
