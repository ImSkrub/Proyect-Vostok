using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonManager : MonoBehaviour
{
    // Menú principal
    public void GoToMenu()
    {
        GameManager.Instance.ResetList(); // Limpieza general
        LevelManager.instance.LoadMainMenu();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ButtonPlay()
    {
        GameManager.Instance.ResetList();
        LevelManager.instance.LoadLevel(1); // Primer nivel jugable
    }

    public void LevelSelect()
    {
        LevelManager.instance.LoadLevel(8); // Suponiendo que la escena 8 es el selector
    }

    public void RestartLevel()
    {
        GameManager.Instance.ResetList(); // Reinicia copias y powerups
        LevelManager.instance.RestartLevel();
    }

    // Carga directa de niveles
    public void Level1() => CargarNivel(1);
    public void Level2() => CargarNivel(2);
    public void Level3() => CargarNivel(3);
    public void Level4() => CargarNivel(4);
    public void Level5() => CargarNivel(5);

    private void CargarNivel(int index)
    {
        GameManager.Instance.ResetList();
        LevelManager.instance.LoadLevel(index);
    }

}