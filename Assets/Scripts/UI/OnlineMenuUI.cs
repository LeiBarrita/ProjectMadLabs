using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class OnlineMenuUI
{
    public Action OpenJoinOnline { set => _joinBtn.clicked += value; }
    private readonly int maxPlayers = 4;
    private Button _hostBtn;
    private Button _joinBtn;

    public OnlineMenuUI(VisualElement root)
    {
        _hostBtn = root.Q<Button>("HostGameBtn");
        _joinBtn = root.Q<Button>("JoinGameBtn");

        AddListeners();
    }

    private void AddListeners()
    {
        _hostBtn.clicked += () => StartHostedGame();
        _joinBtn.clicked += () => Debug.Log("Join Game Menu");
    }

    private void StartHostedGame()
    {
        Debug.Log("Starting hosted game");
        AsyncOperation loadGame = SceneManager.LoadSceneAsync("TestWorld");
        loadGame.completed += OnLoadCompleted;
    }

    private async void OnLoadCompleted(AsyncOperation operation)
    {
        string joinCode = await OnlineServices.CreateRelay(maxPlayers);
        GameStateManager.joinCode = joinCode;
    }
}
