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
        life, dash, jetpack
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerGameObject = collision.gameObject;
            PlayerController _player = collision.gameObject.GetComponent<PlayerController>();

            foreach (ItemData item in items)
            {
                ApplyItemEffect(_player, item);
            }

        }
    }

    private void ApplyItemEffect(PlayerController _player, ItemData item)
    {
        switch (item.type)
        {
            case StatType.life:
                _player.GetComponent<PlayerLife>().currentHealth += item.amount;
                this.gameObject.SetActive(false);
                break;
            case StatType.dash:
                _player.GetComponent<PlayerController>().activateDash();
                this.gameObject.SetActive(false);

                break;
            case StatType.jetpack:
                _player.GetComponent<PlayerController>().jetPackFuel += item.amount;
                _player.GetComponent<PlayerController>().activateJetPack();
                this.gameObject.SetActive(false);


                break;

        }
    }
  
}
