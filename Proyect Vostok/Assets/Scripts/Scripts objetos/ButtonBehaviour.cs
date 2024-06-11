using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    private Animator anim;

    public enum Object
    {
        box,player,copy
    }

    void Start()
    {
      anim = GetComponent<Animator>();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*switch(collision)
            case Object.box:
                    if(collision.gameObject.layer == 5)
                      {

                      }
                    break;
            case Object.player:
            if(collision.gameObject.CompareTag("Player"))
            {

            }
            break;
            case Object.copy:
            break;*/
    }
}
