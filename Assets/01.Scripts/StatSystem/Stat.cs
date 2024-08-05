using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private int _baseValue;

    public List<int> modifiers;
    public bool isPercent;

    public int GetValue()
    {
        int final = _baseValue;
        foreach (int value in modifiers)
        {
            final += value;
        }

        return final;
    }

    public void AddModifier(int value)
    {
        if (value != 0)
            modifiers.Add(value);
    }

    public void RemoveModifier(int value)
    {
        if (value != 0)
            modifiers.Remove(value);
    }

    public void SetDefalutValue(int value)
    {
        _baseValue = value;
    }
}
