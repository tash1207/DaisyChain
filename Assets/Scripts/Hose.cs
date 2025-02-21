using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hose : MonoBehaviour
{
    [SerializeField] GameObject fixedHose;

    void Start()
    {
        // Hacky way to make sure the canonical game logic is checked.
        foreach (GameLogic gameLogic in FindObjectsOfType<GameLogic>())
        {
            if (gameLogic.waterTurnedOn)
            {
                fixedHose.SetActive(true);
                break;
            }
        }
    }
}
