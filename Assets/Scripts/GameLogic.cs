using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public bool foundWaterValveKey = false;
    public bool waterTurnedOn = false;
    public bool needsTowel = false;
    public bool hasTowel = false;

    public bool happinessFromNeighbor = false;
    public bool happinessFromConstruction = false;
    public bool happinessFromBeach = false;
    public bool happinessFromJoke = false;
}
