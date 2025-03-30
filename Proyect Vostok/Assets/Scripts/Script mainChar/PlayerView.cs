using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private Animator anim;
   

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void UpdateMovementAnimation(float velocityX)
    {
        anim.SetBool("IsRunning", Mathf.Abs(velocityX) > 0.1f); // Activa la animación de correr
    }

    public void SetJetpackState(bool isActive)
    {
        anim.SetBool("JetpackOn", isActive); // Activa/desactiva el jetpack
    }

    public void TriggerDeathAnimation()
    {
        anim.SetTrigger("Dead"); // Activa la animación de muerte
    }
}
