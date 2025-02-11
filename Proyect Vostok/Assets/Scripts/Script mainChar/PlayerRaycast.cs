using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    private bool rightWall = false;
    private bool leftWall = false;
    [SerializeField] private float sideDistance = 1.0f;
    private void Update()
    {        
        var rightHit = Physics2D.Raycast(gameObject.transform.position, Vector2.right,sideDistance);
        var downHit = Physics2D.Raycast(gameObject.transform.position, Vector2.down, 1);
        var leftHit = Physics2D.Raycast(gameObject.transform.position, Vector2.left, sideDistance);

        if (rightHit && rightHit.transform.gameObject.layer == 6)
        {
            Debug.Log("toque pared derecha");
            rightWall = true;
        } else
        {
            Debug.Log("no toque pared derecha");
            rightWall = false;
        }

        if (leftHit && leftHit.transform.gameObject.layer == 6)
        {
            Debug.Log("toque pared izquierda");
            leftWall = true;
        }
        else
        {
            Debug.Log("no toque pared izquierda");
            leftWall = false;
        }

        if (downHit && !rightWall && !leftWall && downHit.transform.gameObject.layer == 10 && downHit.normal == Vector2.up)
        {
            downHit.transform.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }

    }
}
