using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    int introSceneIndex = 1;

    public void BeginGame()
    {
        StartCoroutine(LoadIntroScene());
    }

    public void EndGame()
    {
        Application.Quit();
    }

    IEnumerator LoadIntroScene()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(introSceneIndex);
    }
}
