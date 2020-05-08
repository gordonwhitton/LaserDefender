using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class Level : MonoBehaviour
{

    [SerializeField] float delayInSeconds = 2f;

    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);//load the first scene
    }

    public void LoadGame()
    {
        Debug.Log("Clicked LoadGame");

        SceneManager.LoadScene("Game"); //based upon name of scene (not ideal if we change name of scene)

        FindObjectOfType<GameSession>().ResetGame();
    }

    public void LoadGameOver()
    { 
        StartCoroutine(WaitAndLoad());
    }

    private IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene("Game Over");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
