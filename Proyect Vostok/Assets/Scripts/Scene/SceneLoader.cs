using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private class LoadingMonoBehaviour : MonoBehaviour { }
    public enum Scene
    {
        MainMenu, LoadingScene, StartMap, BattleScene1, BattleScene2, BattleScene4, BattleScene5
    }

    private static Action onLoaderCallback;
    private static AsyncOperation loadingAsyncOperation;

    public static void Load(Scene scene) 
    {
        //Set the loader callback action to load the target scene
        onLoaderCallback = () =>{
            GameObject loadingGameObject = new GameObject("LoadingGameObject");
            loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
            LoadSceneAsync(scene);
        };
        //Load the loading scene
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }
    private static IEnumerator LoadSceneAsync(Scene scene)
    {
        yield return null;
        loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
        while(!loadingAsyncOperation.isDone)
        {
            yield return null;
        }
    }
    public static float GetLoadingProgress()
    {
        if(loadingAsyncOperation != null)
        {
            return loadingAsyncOperation.progress;
        }
        else
        {
            return 1f;
        }
    }
    public static void LoaderCallback()
    {
        //triggered after ther first Update wich lets the screen refresh
        //Execute the loader callback, will load the target scene
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}
