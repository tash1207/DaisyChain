using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] Slider airSlider;
    [SerializeField] Slider soilSlider;
    [SerializeField] Slider sunSlider;
    [SerializeField] Slider waterSlider;

    int airHealth = 20;
    int soilHealth = 50;
    int sunHealth = 50;
    int waterHealth = 50;
    

    void Start()
    {
        airSlider.value = airHealth;
        soilSlider.value = soilHealth;
        sunSlider.value = sunHealth;
        waterSlider.value = waterHealth;

        // DontDestroyOnLoad!!
    }

    public void DecreaseAir(int value)
    {
        airHealth -= value;
        airSlider.value = airHealth;
    }

    public void IncreaseAir(int value)
    {
        airHealth += value;
        airSlider.value = airHealth;
    }

    public void DecreaseSoil(int value)
    {
        soilHealth -= value;
        soilSlider.value = soilHealth;
    }

    public void IncreaseSoil(int value)
    {
        soilHealth += value;
        soilSlider.value = soilHealth;
    }

    public void DecreaseSun(int value)
    {
        sunHealth -= value;
        sunSlider.value = sunHealth;
    }

    public void IncreaseSun(int value)
    {
        sunHealth += value;
        sunSlider.value = sunHealth;
    }

    public void DecreaseWater(int value)
    {
        waterHealth -= value;
        waterSlider.value = waterHealth;
    }

    public void IncreaseWater(int value)
    {
        waterHealth += value;
        waterSlider.value = waterHealth;
    }
}
