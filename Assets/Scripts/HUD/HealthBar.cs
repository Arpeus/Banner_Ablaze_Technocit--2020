using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private float fillAmount;

    [SerializeField] Image content;

    public float _maxValue { get; set; }

    public float _value {
        set
        {
            fillAmount = Map(value, 0, _maxValue, 0 ,1);
        }
    }

    void Update()
    {
        HandleBar();
    }

    private void HandleBar()
    {
        if(fillAmount != content.fillAmount)
        {
            content.fillAmount = fillAmount;
        }
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
