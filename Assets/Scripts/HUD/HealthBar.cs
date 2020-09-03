using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private float fillAmount;

    [SerializeField] private float lerpSpeed;

    public Image content;

    public bool initialize = false;

    public float _maxValue { get; set; }

    public float _value {
        set
        {
            fillAmount = Map(value, 0, _maxValue, 0 ,1);
        }
    }

    void Update()
    {
        if (initialize)
        {
            HandleBar();            
        }
    }

    public void SetLifeBar(int maxValue, int value)
    {
        _maxValue = maxValue;
        content.fillAmount = Map(value, 0, _maxValue, 0, 1);
        _value = value;//Map(value, 0, _maxValue, 0, 1);
        initialize = true;
    }

    private void HandleBar()
    {
        if(fillAmount != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
            if (fillAmount == content.fillAmount)
                initialize = false;
        }
       
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
