using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    public enum SceneSetting
    {
        Beach,
        Construction,
        NeighborYard,
        OutsideHouse,
    }

    public SceneSetting previousSceneSetting = SceneSetting.OutsideHouse;
    public SceneSetting currentSceneSetting = SceneSetting.OutsideHouse;

    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;

        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetSceneSettings()
    {
        previousSceneSetting = currentSceneSetting;
        switch(SceneManager.GetActiveScene().buildIndex)
        {
            case 3:
                currentSceneSetting = SceneSetting.OutsideHouse;
                if (previousSceneSetting == SceneSetting.Beach)
                {
                    FindObjectOfType<SceneStart>().BeachToOutsideHouse();
                }
                else if (previousSceneSetting == SceneSetting.Construction)
                {
                    FindObjectOfType<SceneStart>().ConstructionToOutsideHouse();
                }
                else if (previousSceneSetting == SceneSetting.NeighborYard)
                {
                    FindObjectOfType<SceneStart>().NeighborYardToOutsideHouse();
                }
                break;
            case 4:
                currentSceneSetting = SceneSetting.NeighborYard;
                if (previousSceneSetting == SceneSetting.Construction)
                {
                    FindObjectOfType<SceneStart>().ConstructionToNeighborYard();
                }
                else if (previousSceneSetting == SceneSetting.OutsideHouse)
                {
                    FindObjectOfType<SceneStart>().OutsideHouseToNeighborYard();
                }
                break;
            case 5:
                currentSceneSetting = SceneSetting.Construction;
                if (previousSceneSetting == SceneSetting.NeighborYard)
                {
                    FindObjectOfType<SceneStart>().NeighborYardToConstruction();
                }
                else if (previousSceneSetting == SceneSetting.OutsideHouse)
                {
                    FindObjectOfType<SceneStart>().OutsideHouseToConstruction();
                }
                break;
            case 6:
                currentSceneSetting = SceneSetting.Beach;
                break;
        }
    }

    void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
