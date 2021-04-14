
using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class Range : PropertyAttribute
{
    
    public readonly int min;
    public readonly int max;
    public readonly int step;

    public Range(int min, int max, int step)
    {
        this.min = min;
        this.max = max;
        this.step = step;
    }
}
