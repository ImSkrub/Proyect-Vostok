using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionItems : MonoBehaviour
{
    //Lista.
    [SerializeField] List<ItemData> items;
    private GameObject playerGameObject;
    public int indexList;
    
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

    private void Awake()
    {
        PowerUpManager.Instance.powerUp.Add(this.gameObject);
        indexList = PowerUpManager.Instance.powerUp.Count - 1;
    }
    private void OnCollisionEnter2D(Collision2D collision)
     { 
        if (collision.gameObject.CompareTag("Player"))
        {
            playerGameObject = collision.gameObject;
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
                this.gameObject.SetActive(false);
                break;
            case StatType.dash:
                _player.GetComponent<player>().activateDash();
                this.gameObject.SetActive(false);

                break;
            case StatType.jetpack:
                _player.GetComponent<player>().jetPackFuel += item.amount;
                _player.GetComponent<player>().activateJetPack();
                this.gameObject.SetActive(false);


                break;
            
        }
    }
    public void Initialize()
    {
        PowerUpManager.Instance.powerUp.Add(this.gameObject);
        indexList = PowerUpManager.Instance.powerUp.Count - 1;
    }
}
