using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]      //Makes whole script visible in inspector.
public class Stat        //A stat like damage would have this type. Has a base, can add and remove modifiers.
{
    [SerializeField] int baseValue;       //Depends what stat is for. Damage, health, dodge etc... all have a base value. Can set in inspector same as int.

    public List<int> modifiers;              //When equip weapon or item, can modify stats.

    public int GetValue()           //get the value of damage for eg, including modifiers if any.
    {
        int finalValue = baseValue;

        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }

        return finalValue;       
    }

    public void SetDefaultValue(int _value)
    {
        baseValue = _value;
    }

    public void AddModifier(int _modifier) => modifiers.Add(_modifier);     //Add to modifier list.

    public void RemoveModifier(int _modifier) => modifiers.Remove(_modifier);       //Remove from modifier list.
}
