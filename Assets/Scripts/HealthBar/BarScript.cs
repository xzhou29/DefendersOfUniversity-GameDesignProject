using System;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{

    [SerializeField]
    private Image content;

   // [SerializeField]
   // private Text valueText;

    [SerializeField]
    private float lerpSpeed;

    private float fillAmount;

    [SerializeField]
    private bool lerpColors;

    [SerializeField]
    private bool lerpBar;

    [SerializeField]
    private Color fullColor;

    [SerializeField]
    private Color lowColor;

    public float MaxValue { get; set; }

    public float Value
    {
        set
        {
            fillAmount = Map(value, 0, MaxValue, 0, 1);
        }
    }

    void Start()
    {
        if (lerpColors) 
        {
            content.color = fullColor;
        }
    }

    void Update ()
    {
        HandleBar();

    }

    private void HandleBar()
    {
        if (fillAmount != content.fillAmount) 
        {
            if (lerpBar)
            {
                content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
            }
            else
            {
                content.fillAmount = fillAmount;
            }

            if (lerpColors)
            {   
                content.color = Color.Lerp(lowColor, fullColor, fillAmount);
            }
           
        }
    }

    /// <param name="value">The value to evaluate</param>
    /// <param name="inMin">The minimum value of the evaluated variable</param>
    /// <param name="inMax">The maximum value of the evaluated variable</param>
    /// <param name="outMin">The minum number we want to map to</param>
    /// <param name="outMax">The maximum number we want to map to</param>
    /// <returns></returns>
    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
