using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;

public class StartScreenMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject matchList;
    
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
