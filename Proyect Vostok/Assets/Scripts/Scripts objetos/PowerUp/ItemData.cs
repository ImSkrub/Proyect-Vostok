using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public StatType type;
    public int amount;

    public IItemEffect CreateEffect()
    {
        return type switch
        {
            StatType.life => new LifeItemEffect(amount),
            StatType.dash => new DashItemEffect(amount), 
            StatType.jetpack => new JetpackItemEffect(amount),
            _ => null
        };
    }
}