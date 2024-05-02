using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject prefabBullet; //Objeto a usar.
    [Header ("Bullet")]
    public int damage = 10; //Daño
    public float speed = 15f; //Vel bola
    public float lifeTime = 1f; //Vida bola
    public Transform spawnpoint; //Donde se genera la bola
    public float fireRate = 0.01f; //Cada cuanto se disparan
    private float currentTime;

    [Header("Rango")]
    public Transform player;
    public float range = 10f;


    private Animator anim;


    private void Start()
    {
            anim = GetComponent<Animator>();
    }

   void Update()
   {
        RaycastHit2D inRange = Physics2D.Raycast(transform.position, player.position - transform.position, range);
        if(inRange.collider!=null&& inRange.collider.tag=="Player")
        {
            currentTime += Time.deltaTime;
            Debug.Log("Jugador encontrado");
            if (currentTime >= fireRate)
            {
                anim.SetTrigger("disparar");
                GameObject newFireBall = Instantiate(prefabBullet, spawnpoint.position, transform.rotation);
                currentTime = 0;
            }
        }

    }
}
