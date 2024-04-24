using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCopia : MonoBehaviour
{
    private List<Vector3> listOfPositions;
    private int posCounter = 0;
    private bool enableCollision = true;


    // Start is called before the first frame update
    void Start()
    {

    }
    private void Awake()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        
        if (posCounter < listOfPositions.Count) 
        { 
            gameObject.transform.position = listOfPositions[posCounter];
            posCounter++;
            
        }
        if (posCounter == listOfPositions.Count)
        {
            gameObject.SetActive(false);
        }

    }
    private void Update()
    {
        if (enableCollision)
        {
            this.TryGetComponent<BoxCollider2D>(out BoxCollider2D colliderComponent);
            activateCollision(0.2f, colliderComponent);
            enableCollision = false;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameObject.SetActive(false);
            Destroy(this.gameObject);
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
}
