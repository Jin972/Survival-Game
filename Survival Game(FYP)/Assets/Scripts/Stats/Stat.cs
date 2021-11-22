using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField]
    private float baseVale; //Starting value 

    //Keep a list of all the modifiers on this stat
    private List<int> modifiers = new List<int>();

    //Add all modifiers together and return the results
    public float GetValue()
    {
        float finalValue = baseVale;
        modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    // Add a new modifier to list
    public void AddModifier (int modifier)
    {
        if(modifier != 0)
        {
            modifiers.Add(modifier);
        }
    }

    // Remove a modifier from list
    public void RemoveModifier(int modifier)
    {
        if (modifier != 0)
        {
            modifiers.Remove(modifier);
        }
    }
}
