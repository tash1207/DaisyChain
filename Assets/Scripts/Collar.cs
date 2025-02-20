using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collar : MonoBehaviour
{
    [SerializeField] GameObject collar;
    bool isWearingCollar = false;

    public void WearCollar()
    {
        collar.SetActive(true);
        isWearingCollar = true;
    }

    public bool IsWearingCollar()
    {
        return isWearingCollar;
    }
}
