using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashItemEffect : IItemEffect
{
    private int amount;
    public DashItemEffect(int amount)
    {
        this.amount = amount; 
    }

    public void ApplyEffect(Player player)
    {
        player.AddDashTime(amount);
    }
}
