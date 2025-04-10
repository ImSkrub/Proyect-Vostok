using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header ("Variables")]
  
    [SerializeField] GameObject follow; //Seguir al player como objeto
    public Transform player; //Ubiacion del jugador.
    [SerializeField] float distToAttack; //Dist para empezar a disparar
    [SerializeField] float attackVel; //Velocidad del disparo
    [SerializeField] float closestDist; //Dist que tan cerca llega a disparar
    [SerializeField] float lerpSpeedRotation;
    public float attackDelay = 1f;
    
    Rigidbody2D rb;
   // Animator anim;
    Vector3 enemyDirection;
    private float currentTime;

    [Header("Bullet")]
    public Transform weaponsSlot;//posicion que dispara
    public GameObject bulletPrefab; //bala
    public Transform weaponSlot; //posicion en la que dispara 
    


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
       // anim = GetComponent<Animator>();
    }

   void Update()
   {
       currentTime += Time.deltaTime;
       enemyDirection= follow.transform.position - transform.position;
       if(enemyDirection.magnitude < distToAttack && enemyDirection.magnitude > closestDist)
       {
            if(currentTime >= attackDelay)
            {
                AudioManager.instance.PlaySFX("gun");
                //anim.SetBool("Shooting",true);
                Instantiate(bulletPrefab, weaponSlot.position, transform.rotation);
                
                transform.right = Vector3.Lerp(transform.right, enemyDirection, lerpSpeedRotation * Time.deltaTime);
                currentTime = 0f;
            }
            //}else
            //{
            //    anim.SetBool("Shooting", false);
            //}
       }
       
   }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,distToAttack);
    }

}
