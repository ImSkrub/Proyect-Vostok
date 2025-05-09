using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCopia : MonoBehaviour
{
    private List<Vector3> listOfPositions;
    private int posCounter = 0;
    private bool enableCollision = true;
    private bool fastForward = false;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    [SerializeField] private int fastForwardMultiplier = 3;

    void Start()
    {
        posCounter = 0;
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
  
    void FixedUpdate()
    {
        if (!fastForward && listOfPositions != null)
        {
            updateCopyPos();
        }
    }
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
        if (Input.GetKeyDown(KeyCode.B)) 
        {
            fastForward = true;
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            fastForward = false;
        }
        if (fastForward)
        {
            updateCopyPos();
        }
    }

    public void setListOfPositions(List<Vector3> list)
    {
        print(list.Count);    
        listOfPositions = list;
    }

    //IEnumerator activateCollision(float delay, BoxCollider2D collider2D)
    //{
    //    yield return new WaitForSeconds(delay);

    //    collider2D.isTrigger = false;
    //}

    private void updateCopyPos()
    {
        if (listOfPositions == null || listOfPositions.Count == 0) return;
        int steps = fastForward ? fastForwardMultiplier : 1;
        for (int i = 0; i < steps && posCounter < listOfPositions.Count; i++)
        {
            transform.position = listOfPositions[posCounter];
            posCounter++;
        }
        if (posCounter >= listOfPositions.Count)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            boxCollider2D.isTrigger = false;
            gameObject.layer = 11;
        }
    }
}
