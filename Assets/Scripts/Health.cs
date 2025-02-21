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

    float airHealth = 15;
    int soilHealth = 30;
    int sunHealth = 40;
    int waterHealth = 30;
    int moodHealth = 0;
    int maxValue = 100;

    float airPerSecond = 0.85f;

    void Start()
    {
        airSlider.value = airHealth;
        soilSlider.value = soilHealth;
        sunSlider.value = sunHealth;
        waterSlider.value = waterHealth;
    }

    void Update()
    {
        if (airHealth < maxValue)
        {
            IncreaseAir(airPerSecond * Time.deltaTime);
        }
    }

    public void DecreaseAir(float value)
    {
        airHealth -= value;
        airSlider.value = airHealth;
    }

    public void IncreaseAir(float value)
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
        StartCoroutine(IncreaseSunDelay(value));
    }

    IEnumerator IncreaseSunDelay(int value)
    {
        yield return new WaitForSeconds(1f);
        sunText.color = Color.green;
        yield return new WaitForSeconds(0.75f);
        sunHealth += value;
        sunSlider.value = sunHealth;
        StartCoroutine(ChangeBackToWhite(sunText));
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
        IncreaseSoil(maxValue - soilHealth);
    }

    public void MaxSun()
    {
        IncreaseSun(maxValue - sunHealth);
    }

    public void MaxWater()
    {
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

    public bool HasMaxedPlantMeters()
    {
        bool atMaxAir = airHealth >= maxValue;
        bool atMaxSoil = soilHealth >= maxValue;
        bool atMaxSun = sunHealth >= maxValue;
        bool atMaxWater = waterHealth >= maxValue;
        return atMaxAir && atMaxSoil && atMaxSun && atMaxWater;
    }

    public bool HasMaxedHumanMeters()
    {
        return moodHealth > 70;
    }
}
