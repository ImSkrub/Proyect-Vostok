using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet")]
    public float speed = 15f; //Vel bola
    public float lifeTime = 5f; //Vida bola
    private float currentTime;
    //public AudioClip sonido;
    //private AudioSource audioSource;
    
    Rigidbody2D rb;

    [SerializeField] float lerpSpeedRotation;
    [SerializeField] int damage;




    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        /*audioSource = GetComponent<AudioSource>(); 
         * 
         */
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
        currentTime += Time.deltaTime;
        if (currentTime > lifeTime)
        {
            Destroy(gameObject);
            currentTime = 0f;
        }
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Life>().GetDamage(damage);
            Destroy(gameObject);
        }
    }
}
