using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathCounter : MonoBehaviour
{
       public static DeathCounter instance;
    // Start is called before the first frame update

    
    public int contadorMuertes = 0;

    private void Awake()
    {
        

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void ReiniciarContadorMuertes()
    {
        contadorMuertes = 0;
        
    }

}
