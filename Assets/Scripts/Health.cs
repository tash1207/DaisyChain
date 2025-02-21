using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] GameObject moodMeter;

    [SerializeField] Slider airSlider;
    [SerializeField] Slider soilSlider;
    [SerializeField] Slider sunSlider;
    [SerializeField] Slider waterSlider;
    [SerializeField] Slider moodSlider;

    [SerializeField] TMP_Text airText;
    [SerializeField] TMP_Text soilText;
    [SerializeField] TMP_Text sunText;
    [SerializeField] TMP_Text waterText;
    [SerializeField] TMP_Text moodText;

    int airHealth = 20;
    int soilHealth = 40;
    int sunHealth = 40;
    int waterHealth = 40;
    int moodHealth = 0;
    int maxValue = 100;

    public bool maxedAir = false;
    public bool maxedSoil = false;
    public bool maxedSun = false;
    public bool maxedWater = false;

    void Start()
    {
        airSlider.value = airHealth;
        soilSlider.value = soilHealth;
        sunSlider.value = sunHealth;
        waterSlider.value = waterHealth;
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
        StartCoroutine(DecreaseSoilDelay(value));
    }

    IEnumerator DecreaseSoilDelay(int value)
    {
        yield return new WaitForSeconds(1f);
        soilText.color = Color.red;
        yield return new WaitForSeconds(0.75f);
        // TODO: Set a min of 0.
        soilHealth -= value;
        soilSlider.value = soilHealth;
        StartCoroutine(ChangeBackToWhite(soilText));
    }

    public void IncreaseSoil(int value)
    {
        StartCoroutine(IncreaseSoilDelay(value));
    }

    IEnumerator IncreaseSoilDelay(int value)
    {
        yield return new WaitForSeconds(1f);
        soilText.color = Color.green;
        yield return new WaitForSeconds(0.75f);
        soilHealth += value;
        soilSlider.value = soilHealth;
        StartCoroutine(ChangeBackToWhite(soilText));
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
        StartCoroutine(DecreaseWaterDelay(value));
    }

    IEnumerator DecreaseWaterDelay(int value)
    {
        yield return new WaitForSeconds(1f);
        waterText.color = Color.red;
        yield return new WaitForSeconds(0.75f);
        waterHealth -= value;
        waterSlider.value = waterHealth;
        StartCoroutine(ChangeBackToWhite(waterText));
    }

    public void IncreaseWater(int value)
    {
        StartCoroutine(IncreaseWaterDelay(value));
    }

    IEnumerator IncreaseWaterDelay(int value)
    {
        yield return new WaitForSeconds(1f);
        waterText.color = Color.green;
        yield return new WaitForSeconds(0.75f);
        waterHealth += value;
        waterSlider.value = waterHealth;
        StartCoroutine(ChangeBackToWhite(waterText));
    }

    IEnumerator ChangeBackToWhite(TMP_Text text)
    {
        yield return new WaitForSeconds(0.5f);
        text.color = Color.white; 
    }

    public void MaxSoil()
    {
        maxedSoil = true;
        IncreaseSoil(maxValue - soilHealth);
    }

    public void MaxWater()
    {
        maxedWater = true;
        IncreaseWater(maxValue - waterHealth);
    }

    public void ShowMood()
    {
        moodMeter.SetActive(true);
    }

    public void DecreaseMood(int value)
    {
        moodMeter.SetActive(true);
        moodHealth -= value;
        moodSlider.value = moodHealth;
    }

    public void IncreaseMood(int value)
    {
        moodMeter.SetActive(true);
        moodHealth += value;
        moodSlider.value = moodHealth;
    }
}
