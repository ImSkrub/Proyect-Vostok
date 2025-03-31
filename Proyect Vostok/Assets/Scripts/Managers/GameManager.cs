using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Copia")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject copiaPlayerPrefab;
    [SerializeField] int copyLimit = 10;
   
    //Beta cambiar a queue
    public static List<Vector3> playerPositions = new List<Vector3>();
  
    public static Queue<List<Vector3>> _listPlayerPositions = new Queue<List<Vector3>>();
    public static Queue<GameObject> _copiaPlayers = new Queue<GameObject>();

 
    
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
        player.GetComponent<PlayerLife>().OnDeath += FinishGame;
        
    }
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
            ResetList();
            
        }
    }

    public void UpdateQueueOfPositions(List<CopyDataModel> listPos)
    {
        var listPositions = new List<Vector3>();
        if (listPos != null)
        {
            foreach (var item in listPos)
            {
                listPositions.Add(item.Pos);
            }
        }
        if(_listPlayerPositions.Count < copyLimit)
        {
            _listPlayerPositions.Enqueue(listPositions);
            _copiaPlayers.Enqueue(new GameObject());
        }
        if(_listPlayerPositions.Count== copyLimit)
        {
            _listPlayerPositions.Dequeue();
        }
    }

    //public void UpdateListOfPositions(List<CopyDataModel> listPos)
    //{
    //    var listPositions = new List<Vector3>();
    //    if (listPos != null)
    //    {
    //        foreach (var item in listPos)
    //        {
    //            listPositions.Add(item.Pos);
    //        }
    //    }
    //    listPlayerPositions.Add(listPositions);
    //    print(listPlayerPositions.Count);
    //    copiaPlayers.Add(new GameObject("copia" + counter.ToString()));
    //    print(copiaPlayers.Count);
    //    counter++;
    //}

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
        print(listOfPositions.Count);
        print(_copiaPlayers.Count);
        player = Instantiate(copiaPlayerPrefab);
        player.TryGetComponent<PlayerCopia>(out PlayerCopia copiaPlayer);
        copiaPlayer.setListOfPositions(listOfPositions);
    }

    public void instantiateListOfObjects()
    {
        print(_copiaPlayers.Count);
        print(_listPlayerPositions.Count);
        for (int i = 0; i < _copiaPlayers.Count; i++)
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
        playerPositions.Clear();
        PowerUpManager.Instance.powerUpDisabled.Clear();
        PowerUpManager.Instance.activePowerUps.Clear();
    }

    public Queue<GameObject> getCopiaPlayers() { return _copiaPlayers; }

    void FinishGame()
    {
        SceneManager.LoadScene("Win");
        ResetList();
    }
    public void LoseGame()
    {
        SceneManager.LoadScene("Lose");
        ResetList();
    }

    public void CopyNumberChange(int newCopyLimit)
    {
        if (LevelManager.instance.currentLevel >= 1)
        {
            copyLimit = newCopyLimit;
        }
              
    }
}
