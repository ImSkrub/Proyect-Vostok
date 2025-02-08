using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMemento
{
    public Vector3 position { get; private set; }
    public float health { get; private set; }
    public PlayerMemento(Vector3 position, float health)
    {
        this.position = position;
        this.health = health;
    }

}
