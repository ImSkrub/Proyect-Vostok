using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] GameObject player;
    private GameObject copiaPlayer;
    [SerializeField] GameObject copiaPlayerPrefab;

    public static List<Vector3> playerPositions = new List<Vector3>();
    public static List<List<Vector3>> listPlayerPositions = new List<List<Vector3>>();
    public static List<GameObject> copiaPlayers = new List<GameObject>();

    private int counter;

    //Variables globales

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
        }
    }
    
    public void UpdateListOfPositions(List<Vector3> listOfPositions)
    {
        print(listOfPositions.Count);
        var listPositions = new List<Vector3>();
        foreach (var item in listOfPositions)
        {
            listPositions.Add(item);
        }
        print(playerPositions.Count);
        listPlayerPositions.Add(listPositions);
        print(listPlayerPositions.Count);
        copiaPlayers.Add(new GameObject("copia" + counter.ToString()));
        counter++;
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
        foreach (var player in copiaPlayers)
        {
            foreach (var listPos in listPlayerPositions)
            {
                print(listPos.Count);
                instantiateCopiaPlayer(player, listPos);
            }
        }
    }

    public List<GameObject> getCopiaPlayers() { return copiaPlayers; }
    
    void FinishGame()
    {
        SceneManager.LoadScene(3);
    }

}
