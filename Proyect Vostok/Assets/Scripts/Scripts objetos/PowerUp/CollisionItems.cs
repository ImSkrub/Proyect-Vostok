using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    life, dash, jetpack
}

public class CollisionItems : MonoBehaviour
{
    [SerializeField] private List<ItemData> items;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        Player player = collision.gameObject.GetComponent<Player>();

        foreach (ItemData item in items)
        {
            IItemEffect effect = item.CreateEffect();
            effect?.ApplyEffect(player);
        }

        PowerUpManager.Instance.powerUpDisabled.Add(gameObject); // Notifica al PowerUpManager
        this.gameObject.SetActive(false); // Desactiva el objeto
    }

}
