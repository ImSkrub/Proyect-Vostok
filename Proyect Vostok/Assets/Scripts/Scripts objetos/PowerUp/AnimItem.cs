using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimItem : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    void Start()
    {
        animator=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Active",true);
    }
}
