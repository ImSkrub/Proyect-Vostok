using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackItemEffect : IItemEffect
{
    private int fuelAmount;

    public JetpackItemEffect(int fuelAmount)
    {
        this.fuelAmount = fuelAmount;
    }

    public void ApplyEffect(Player player)
    {
        player.AddJetpackFuel(fuelAmount);
        AudioManager.instance.PlaySFX("pickup");
    }
}
