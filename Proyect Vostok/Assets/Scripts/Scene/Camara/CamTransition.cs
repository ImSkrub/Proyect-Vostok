using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTransition : MonoBehaviour
{

    public Camera cam;
   
    [SerializeField] List<Vector3> positions = new List<Vector3>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           
        }
    }


}
