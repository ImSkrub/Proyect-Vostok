using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    private bool rightWall = false;
    private bool leftWall = false;
    [SerializeField] private float sideDistance = 1.0f;
    [SerializeField] private Transform _rightWallCheckPoint;
    [SerializeField] private Transform _leftWallCheckPoint;
    [SerializeField] private LayerMask copyLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 _wallCheckSize = new Vector2(0.3f, 2.35f);
    //layer 10 = copia
    private void Update()
    {
        HandleCollisions();
    }
    public void HandleCollisions()
    {
        // Detectar si el personaje está tocando una pared
        rightWall = Physics2D.OverlapBox(_rightWallCheckPoint.position, _wallCheckSize, 0, copyLayer);
        leftWall = Physics2D.OverlapBox(_leftWallCheckPoint.position, _wallCheckSize, 0, copyLayer);

        // Detectar si hay un objeto debajo del personaje en la capa 10
        var downHit = Physics2D.Raycast(transform.position, Vector2.down, 1, groundLayer);

        // Si el personaje está tocando una pared, desactivar la colisión del objeto en la capa 10
        if (rightWall || leftWall)
        {
            Debug.Log("Tocando una pared, desactivar colisión de la capa 10");
            if (downHit.collider != null && downHit.transform.gameObject.layer == 10)
            {
                downHit.transform.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        // Si no está tocando una pared y hay un objeto debajo en la capa 10, activar la colisión
        else if (downHit.collider != null && downHit.transform.gameObject.layer == 10 && downHit.normal == Vector2.up)
        {
            Debug.Log("Objeto debajo, activar colisión de la capa 10");
            downHit.transform.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_rightWallCheckPoint.position, _wallCheckSize);
        Gizmos.DrawWireCube(_leftWallCheckPoint.position, _wallCheckSize);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 1);
    }
}
