using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitZone : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        //layer 10 = layer copia
        if(collision.gameObject.layer == 10)
        {
            collision.GetComponentInParent<PlayerCopia>().GetComponent<Collider2D>().enabled = true;
        }
    }
}
