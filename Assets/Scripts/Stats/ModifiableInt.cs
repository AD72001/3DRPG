using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void ModifiedEvent();

[Serializable]
public class ModifiableInt
{
    [SerializeField] private int baseValue;
    public int BaseValue { get { return baseValue; } set { baseValue = value; UpdateModifiedvalue(); } }

    [SerializeField] private int modifiedValue;
    public int ModifiedValue { get { return modifiedValue; } set { modifiedValue = value; } }

    public List<IModifier> modifiers = new List<IModifier>();

    public ModifiedEvent valueModified;
    public ModifiableInt(ModifiedEvent method = null)
    {
        modifiedValue = baseValue;
        if (method != null)
            valueModified += method;
    }

    public void RegisterEvent(ModifiedEvent method)
    {
        valueModified += method;
    }

    public void UnregisterEvent(ModifiedEvent method)
    {
        valueModified -= method;
    }

    public void UpdateModifiedvalue()
    {
        var valueToAdd = 0;

        for (int i = 0; i < modifiers.Count; i++)
        {
            modifiers[i].AddValue(ref valueToAdd);
        }

        ModifiedValue = baseValue + valueToAdd;

        if (valueModified != null)
        {
            valueModified.Invoke();
        }
    }

    public void AddModifier(IModifier _modifier)
    {
        modifiers.Add(_modifier);
        UpdateModifiedvalue();
    }

    public void RemoveModifier(IModifier _modifier)
    {
        modifiers.Remove(_modifier);
        UpdateModifiedvalue();
    }
}
