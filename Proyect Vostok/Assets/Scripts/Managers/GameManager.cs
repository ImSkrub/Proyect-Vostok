using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Copia")]
    [SerializeField] GameObject player;
    //[SerializeField] GameObject copiaPlayer;
    [SerializeField] GameObject copiaPlayerPrefab;
   
    //Beta cambiar a queue
    public static List<Vector3> playerPositions = new List<Vector3>();
    public static List<List<Vector3>> listPlayerPositions = new List<List<Vector3>>();
    public static List<GameObject> copiaPlayers = new List<GameObject>();

    private static int counter;

    [Header("PowerUp")]
    public List<GameObject> powerUpDisabled = new List<GameObject>();
    
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
        player.GetComponent<Life>().OnDeath += FinishGame;
    }

    private void Start()
    {
        counter = 0;
    }
    private void FixedUpdate()
    {

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

    public void UpdateListOfPositions(List<CopyDataModel> listPos)
    {
        var listPositions = new List<Vector3>();
        if (listPos != null)
        {
            foreach (var item in listPos)
            {
                listPositions.Add(item.Pos);
            }
        }
        listPlayerPositions.Add(listPositions);
        print(listPlayerPositions.Count);
        copiaPlayers.Add(new GameObject("copia" + counter.ToString()));
        print(copiaPlayers.Count);
        counter++;
    }

    public void _Reset()
    {
        instantiateListOfObjects();
    }

    private void instantiateCopiaPlayer(GameObject player, List<Vector3> listOfPositions)
    {
        print(listOfPositions.Count);
        player = Instantiate(copiaPlayerPrefab);
        player.TryGetComponent<PlayerCopia>(out PlayerCopia copiaPlayer);
        copiaPlayer.setListOfPositions(listOfPositions);
    }

    public void instantiateListOfObjects()
    {
        foreach (var copia in copiaPlayers)
        {
            foreach (var listPos in listPlayerPositions)
            {
                instantiateCopiaPlayer(copia, listPos);
            }
        }
    }

    public void ActivatePowerUp()
    {
        foreach(var powerUp in powerUpDisabled)
        {
            powerUp.SetActive(true);
        }
    }
    

    public void ResetList()
    {
        listPlayerPositions.Clear();
        copiaPlayers.Clear();
        playerPositions.Clear();
        powerUpDisabled.Clear();
    }

    public List<GameObject> getCopiaPlayers() { return copiaPlayers; }

    void FinishGame()
    {
        SceneManager.LoadScene(3);
    }
    public void LoseGame()
    {
        SceneManager.LoadScene("Lose");
    }
}
