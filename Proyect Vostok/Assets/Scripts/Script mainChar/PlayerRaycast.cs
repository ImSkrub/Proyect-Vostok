using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    private bool rightWall = false;
    private bool leftWall = false;
    private bool rightCopy = false;
    private bool leftCopy = false;
    private bool timerEnded = false;
    private bool timerStarted = false;
    [SerializeField] private Transform _rightWallCheckPoint;
    [SerializeField] private Transform _leftWallCheckPoint;
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private LayerMask copyLayer;    // Capa 10: Copias
    [SerializeField] private LayerMask wallLayer;    // Capa 7: Muros
    [SerializeField] private Vector2 _wallCheckSize = new Vector2(0.3f, 2.35f);
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleCollisions();
    }

    public void HandleCollisions()
    {
        // 1. Detectar muros en los lados (usando wallLayer)
        rightWall = Physics2D.OverlapBox(_rightWallCheckPoint.position, _wallCheckSize, 0, wallLayer);
        leftWall = Physics2D.OverlapBox(_leftWallCheckPoint.position, _wallCheckSize, 0, wallLayer);
        rightCopy = Physics2D.OverlapBox(_rightWallCheckPoint.position, _wallCheckSize, 0, copyLayer);
        leftCopy = Physics2D.OverlapBox(_leftWallCheckPoint.position, _wallCheckSize, 0, copyLayer);

        // 2. Detectar copias en los lados y debajo (usando copyLayer)
        Collider2D[] rightCopies = Physics2D.OverlapBoxAll(_rightWallCheckPoint.position, _wallCheckSize, 0, copyLayer);
        Collider2D[] leftCopies = Physics2D.OverlapBoxAll(_leftWallCheckPoint.position, _wallCheckSize, 0, copyLayer);
        Collider2D[] belowCopies = Physics2D.OverlapBoxAll(_groundCheckPoint.position, _groundCheckSize, 0, copyLayer);

        if (timerEnded)
        {
            HandleCollisions(rightCopies, leftCopies, belowCopies);
        } else
        {
            if (!timerStarted) 
            {
                timerStarted = true;
                StartCoroutine(activateCollision(3f));
            }
        }
        

        //// Caso 1: Desactivar colisiones de copias en el lado opuesto al muro
        //HandleOppositeSideCollisions(rightCopies, leftCopies);

        //// Caso 2: Activar colisiones si el jugador está quieto en un muro
        //HandleStationaryWallCollisions(rightCopies, leftCopies, belowCopies);

        //// Caso 3: Activar colisiones si no hay muro en un lado
        //HandleNoWallCollisions(rightCopies, leftCopies);
    }

    private void HandleCollisions(Collider2D[] rightCopies, Collider2D[] leftCopies, Collider2D[] belowCopies)
    {
        if (rightWall || leftWall)
        {
            // Muro a la derecha → desactivar copias a la izquierda
            if (rightWall)
            {
                foreach (Collider2D copy in leftCopies)
                {
                    copy.GetComponent<BoxCollider2D>().isTrigger = true;
                }
            }

            // Muro a la izquierda → desactivar copias a la derecha
            if (leftWall)
            {
                foreach (Collider2D copy in rightCopies)
                {
                    copy.GetComponent<BoxCollider2D>().isTrigger = true;
                }
            }
        } 
        else if (leftCopy && rightCopy)
        {
            foreach (Collider2D copy in leftCopies)
            {
                copy.GetComponent<BoxCollider2D>().isTrigger = true;
            }
            foreach (Collider2D copy in rightCopies)
            {
                copy.GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }
        else 
        {
            if (!leftWall)
            {
                foreach (Collider2D copy in rightCopies)
                {
                    copy.GetComponent<BoxCollider2D>().isTrigger = false;
                }
            }

            // No hay muro a la izquierda → activar copias en ese lado
            if (!rightWall)
            {
                foreach (Collider2D copy in leftCopies)
                {
                    copy.GetComponent<BoxCollider2D>().isTrigger = false;
                }
            }
        }

        
        // Activar copias debajo del jugador
        foreach (Collider2D copy in belowCopies)
        {
            copy.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    IEnumerator activateCollision(float delay)
    {
        yield return new WaitForSeconds(delay);

        timerStarted = false;
        timerEnded = true;
    }

    public void ResetTimer()
    {
        timerEnded = false; 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_rightWallCheckPoint.position, _wallCheckSize);
        Gizmos.DrawWireCube(_leftWallCheckPoint.position, _wallCheckSize);
        Gizmos.DrawWireCube(transform.position, _groundCheckSize);
    }
}
