using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesDamage : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Puedes mantener el código original si también deseas causar daño al jugador al colisionar
        PlayerLife Jugador = collision.collider.GetComponent<PlayerLife>();
        if (Jugador != null)
        {
            Jugador.GetDamage(damage);
        }
    }
}