using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private float _ScreenTime;
    [SerializeField] private List<RectTransform> _gameOverScreens;
    [SerializeField] private RectTransform _CakedScreen;
    private Vector2 _cakedscreenPosition;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        _cakedscreenPosition = _CakedScreen.gameObject.transform.position;
        _CakedScreen.gameObject.SetActive(false);
        foreach (var screen in _gameOverScreens)
        {
            screen.gameObject.SetActive(false);
        }
      
    }
    public IEnumerator SchowCake()
    {
        _CakedScreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        _CakedScreen.gameObject.SetActive(false);
        _CakedScreen.position = _cakedscreenPosition;
    }
    public void ShowGameOverScreen(UnityAction onFinished)
    {
        StartCoroutine(ShowDelayed(onFinished));
    }

    private IEnumerator ShowDelayed(UnityAction onFinished)
    {
        if (_CakedScreen.gameObject.activeSelf) _CakedScreen.gameObject.SetActive(false);

        int random = Random.Range(0, _gameOverScreens.Count);
        _gameOverScreens[random].gameObject.SetActive(true);
        yield return new WaitForSeconds(_ScreenTime);
        _gameOverScreens[random].gameObject.SetActive(false);

        onFinished?.Invoke();
    }
}
