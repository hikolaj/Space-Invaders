using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoreManager : MonoBehaviour
{
    public static CoreManager Instance { get; private set; }

    public GameState State;
    public Scenes ActiveScene;

    public PlayerDataManager PlayerData;

    private const string _menuSceneName = "Menu";
    private const string _gameplaySceneName = "Gameplay";
    private const string _endscreenSceneName = "EndScreen";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        UnloadAllScenes();
    }

    private void Start()
    {
        OpenMenu();
    }


    public void StartGameplay()
    {
        UnloadAllScenes(Scenes.Gameplay);
        LoadScene(Scenes.Gameplay);
        ActiveScene = Scenes.Gameplay;
    }

    public void OpenMenu()
    {
        UnloadAllScenes(Scenes.Menu);
        LoadScene(Scenes.Menu);
        ActiveScene = Scenes.Menu;
    }

    public void OpenEndScreen()
    {
        LoadScene(Scenes.EndScreen);
        ActiveScene = Scenes.EndScreen;
    }

    public void UnloadAllScenes(Scenes without = Scenes.none)
    {
        foreach (string name in Enum.GetNames(typeof(Scenes)))
        {
            if (name != without.ToString() && SceneManager.GetSceneByName(name).IsValid())
                SceneManager.UnloadSceneAsync(name);
        }
    }

    public void LoadScene(Scenes scene, LoadSceneMode mode = LoadSceneMode.Additive)
    {
        string sceneName = scene.ToString();
        if (!SceneManager.GetSceneByName(sceneName).IsValid())
            SceneManager.LoadScene(sceneName, mode);
        
        Debug.Log("Load: " + sceneName);
    }

}
