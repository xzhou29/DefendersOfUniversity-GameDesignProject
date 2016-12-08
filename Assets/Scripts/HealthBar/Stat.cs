﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
class Stat
{

    [SerializeField]
    private BarScript bar;

    private float maxVal;

    private float currentVal;

    public float CurrentValue
    {
        get
        {
            return currentVal;
        }
        set
        {
            this.currentVal = Mathf.Clamp(value, 0, MaxVal);
            //Updates the bar
            Bar.Value = currentVal;
        }
    }


    public float MaxVal
    {
        get
        {
            return maxVal;
        }
        set
        {
            //Updates the bar's max value
            Bar.MaxValue = value;
            this.maxVal = value;
        }
    }

    public BarScript Bar
    {
        get
        {
            return bar;
        }
    }

    public void Initialize()
    {
        this.MaxVal = maxVal;
        this.CurrentValue = currentVal;
    }
}

