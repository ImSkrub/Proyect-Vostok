using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviourInverse : MonoBehaviour
{
    public Animator button;
    public GameObject door;
    //Por si queremos mas puertas activadas
    //[SerializeField] List<GameObject> doors = new List<GameObject>();

    private void Start()
    {
        button.SetBool("On", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            button.SetBool("On", true);
            door.SetActive(true);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            button.SetBool("On", true);
            door.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        door.SetActive(false);
        button.SetBool("On", false);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        door.SetActive(false);
        button.SetBool("On", false);
    }
}
