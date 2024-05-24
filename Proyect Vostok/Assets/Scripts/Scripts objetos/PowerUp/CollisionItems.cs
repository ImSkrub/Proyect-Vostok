using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionItems : MonoBehaviour
{
    //Lista.
    [SerializeField] List<ItemData> items;
    public bool pickUpJetpack = false;
    [System.Serializable]
    public class ItemData
    {
        public StatType type;
        public int amount;
    }

    public enum StatType
    {
        life,dash,jetpack
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
                PowerUpManager.Instance.DeactivatePowerUp();
                break;
            case StatType.jetpack:
                _player.GetComponent<player>().jetPackFuel += item.amount;
                _player.GetComponent<player>().activateJetPack();
                PowerUpManager.Instance.DeactivatePowerUp();
                pickUpJetpack =true;
                break;
            
        }
    }
}
