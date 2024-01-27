using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private float _ScreenTime;
    [SerializeField] private List<RectTransform> _gameOverScreens;
    private Rigidbody2D _rb;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var screen in _gameOverScreens)
        {
            screen.gameObject.SetActive(false);
        }
      
    }

    public void ShowGameOverScreen(UnityAction onFinished)
    {
        StartCoroutine(ShowDelayed(onFinished));
    }

    private IEnumerator ShowDelayed(UnityAction onFinished)
    {
        int random = Random.Range(0, _gameOverScreens.Count);
        _gameOverScreens[random].gameObject.SetActive(true);
        yield return new WaitForSeconds(_ScreenTime);
        _gameOverScreens[random].gameObject.SetActive(false);

        onFinished?.Invoke();
    }
}
