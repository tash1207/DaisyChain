using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    int introSceneIndex = 1;

    public void BeginGame()
    {
        SceneManager.LoadScene(introSceneIndex);
    }
}
