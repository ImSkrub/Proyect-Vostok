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
            PlayerController _player = collision.gameObject.GetComponent<PlayerController>();
            foreach (ItemData item in items)
            {
                ApplyItemEffect(_player, item);
            }
            this.gameObject.SetActive(false); // Desactiva el objeto
        }
    }

    private void ApplyItemEffect(PlayerController _player, ItemData item)
    {
        switch (item.type)
        {
            case StatType.life:
                _player.GetComponent<PlayerLife>().currentHealth += item.amount;
                PowerUpManager.Instance.powerUpDisabled.Add(this.gameObject); // Notifica al PowerUpManager
                AudioManager.instance.PlaySFX("heal");
                break;
            case StatType.jetpack:
                Player player = _player.GetComponent<Player>();
                player.jetpackFuel += item.amount;
                player.activateJetPack();
                PowerUpManager.Instance.powerUpDisabled.Add(this.gameObject); // Notifica al PowerUpManager
                AudioManager.instance.PlaySFX("pickup");
                break;
        }
    }

}
