using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeItemEffect : IItemEffect
{
    private int amount;

    public LifeItemEffect(int amount)
    {
        this.amount = amount;
    }

    public void ApplyEffect(Player player)
    {
        player.Health.AddHealth(amount);
        AudioManager.instance.PlaySFX("heal");
    }
}
