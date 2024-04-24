using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesDamage : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Puedes mantener el c�digo original si tambi�n deseas causar da�o al jugador al colisionar
        Life Jugador = collision.collider.GetComponent<Life>();
        if (Jugador != null)
        {
            Jugador.GetDamage(damage);
        }
    }
}