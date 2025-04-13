using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Jugador y Copia")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject copiaPlayerPrefab;
    [SerializeField] int copyLimit = 10;
   
    public static Queue<List<Vector3>> _listPlayerPositions = new Queue<List<Vector3>>();
    public static Queue<GameObject> _copiaPlayers = new Queue<GameObject>();

    public event Action<int, int> OnCopyCountChanged;
    public static string pendingMusicToPlay = "";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        if (player != null && player.TryGetComponent(out PlayerLife playerLife))
        {
            playerLife.OnDeath += FinishGame;
        }

    }
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           LevelManager.instance.LoadMainMenu();
            ResetList();
            
        }
    }


    public void ClearListOfPositions()
    {
        _listPlayerPositions.Clear();
    }

    public void ClearCopiaPlayerList()
    {
        _copiaPlayers.Clear();
    }

    public void UpdateQueueOfPositions(List<CopyDataModel> listPos)
    {
        if (_copiaPlayers.Count >= copyLimit)
        {
            Debug.Log("Límite de copias alcanzado, no se puede crear más.");
            return;
        }
        var listPositions = new List<Vector3>();
        if (listPos != null)
        {
            foreach (var item in listPos)
            {
                listPositions.Add(item.Pos);
            }
        }

        if (_listPlayerPositions.Count <= copyLimit)
        {
            _listPlayerPositions.Enqueue(listPositions);
            _copiaPlayers.Enqueue(new GameObject());

            // 🚨 Notificar cambio de cantidad
            OnCopyCountChanged?.Invoke(_listPlayerPositions.Count, copyLimit);
        }

        if (_listPlayerPositions.Count == copyLimit)
        {
            _listPlayerPositions.Dequeue();
        }
    }

   

    public void _Reset()
    {
        foreach (var copia in _copiaPlayers)
        {
            Destroy(copia);
        }
        instantiateListOfObjects();
    }

    private void instantiateCopiaPlayer(GameObject player, List<Vector3> listOfPositions)
    {
        var copia = Instantiate(copiaPlayerPrefab);
        
        if (copia.TryGetComponent(out PlayerCopia copiaPlayer))
        {
            copiaPlayer.setListOfPositions(listOfPositions);
        }
    }

    public void instantiateListOfObjects()
    {
        int count = _copiaPlayers.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject copia = _copiaPlayers.Dequeue();
            List<Vector3> listPos = _listPlayerPositions.Dequeue();

            instantiateCopiaPlayer(copia, listPos);
            
            _copiaPlayers.Enqueue(copia);
            _listPlayerPositions.Enqueue(listPos);
        }
    }

    public void ResetList()
    {
        _listPlayerPositions.Clear();
        
        foreach (var copia in _copiaPlayers)
        {
            Destroy(copia);
        }
        _copiaPlayers.Clear();

        PowerUpManager.Instance.powerUpDisabled.Clear();
        PowerUpManager.Instance.activePowerUps.Clear();
    }

    public Queue<GameObject> getCopiaPlayers() => _copiaPlayers;

    void FinishGame()
    {
        Debug.Log("Gane");
        AudioManager.instance.StopAll();
        AudioManager.instance.PlayMusic("win");
        ResetList();
        SceneManager.LoadScene("Win");
    }
    public void LoseGame()
    {
        Debug.Log("Perdiste");
        AudioManager.instance.StopAll();
        SceneManager.LoadScene("Lose");
        AudioManager.instance.PlayMusic("lose");
        ResetList();
        
    }

    public void CopyNumberChange(int newCopyLimit)
    {
        if (LevelManager.instance.currentLevel >= 1)
        {
            copyLimit = newCopyLimit;
        }         

    }
    public int getCopyLimit()=> copyLimit;
    public int GetUsedCopies()=> _listPlayerPositions.Count;
}
