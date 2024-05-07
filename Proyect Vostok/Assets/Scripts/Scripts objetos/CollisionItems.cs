using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionItems : MonoBehaviour
{
    //Lista.
    [SerializeField] List<ItemData> items;

    [System.Serializable]
    public class ItemData
    {
        public StatType type;
        public int amount;
    }

    public enum StatType
    {
        life,dash
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            player _player = collision.gameObject.GetComponent<player>();

            foreach (ItemData item in items)
            {
                ApplyItemEffect(_player, item);
            }

            Destroy(gameObject,0.2f);
        }
    }

    private void ApplyItemEffect(player _player, ItemData item)
    {
        switch (item.type)
        {
            case StatType.life:
                _player.GetComponent<Life>().currentHealth += item.amount;
                break;
            case StatType.dash:
                _player.GetComponent<player>().activateDash();
                break;
        }
    }
}
