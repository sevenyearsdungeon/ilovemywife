using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SliderWithLabels : MonoBehaviour
{
    public Slider slider;
    [SerializeField]
    TMP_Text minValueText;
    [SerializeField]
    TMP_Text maxValueText;
    [SerializeField]
    TMP_Text currentValueText;

    private void Awake()
    {
        minValueText.SetText($"{slider.minValue:0.##}");
        maxValueText.SetText($"{slider.maxValue:0.##}");
        currentValueText.SetText($"{slider.value:0.##}");
        slider.onValueChanged.Invoke(slider.value);
    }

    public void OnSliderValueChanged(float v)
    {
        currentValueText.SetText($"{v:0.##}");
    }
    
}
