using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienciaSliderManager : MonoBehaviour
{
    public bool isUpdating;
    private Slider slider;
    private Stats stats;
    // Start is called before the first frame update
    void Awake()
    {
        slider = GetComponent<Slider>();
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
    }

    private void OnEnable()
    {
        UpdateSliderValues();
    }

    // Update is called once per frame
    void Update()
    {
        if (isUpdating)
        {
            UpdateSliderValues();
        }
    }

    private void UpdateSliderValues()
    {
        slider.minValue = 0;
        slider.maxValue = stats.nextLevelExperience;
        slider.value = stats.experience;
    }
}
