using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCopia : MonoBehaviour
{
    private List<Vector3> listOfPositions;
    private int posCounter = 0;
    private bool enableCollision = true;
    private bool fastForward = false;


    // Start is called before the first frame update
    void Start()
    {
        posCounter = 0;
    }
    private void Awake()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!fastForward)
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

    IEnumerator activateCollision(float delay, BoxCollider2D collider2D)
    {
        yield return new WaitForSeconds(delay);
        
        collider2D.enabled = true;
    }

    private void updateCopyPos()
    {
        if (posCounter < listOfPositions.Count)
        {
            gameObject.transform.position = listOfPositions[posCounter];
            posCounter++;

        }
        //if (posCounter == listOfPositions.Count)
        //{
        //    gameObject.SetActive(false);
        //}
    }
}
