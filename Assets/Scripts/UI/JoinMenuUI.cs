using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class JoinMenuUI
{
    public Action GoBack { set => _backBtn.clicked += value; }

    private Button _joinBtn;
    private Button _backBtn;
    private TextField _codeField;

    public JoinMenuUI(VisualElement root)
    {
        _joinBtn = root.Q<Button>("JoinBtn");
        _backBtn = root.Q<Button>("BackBtn");
        _codeField = root.Q<TextField>("JoinCodeField");

        AddListeners();
    }

    private void AddListeners()
    {
        _joinBtn.clicked += () => JoinToGame();
        _backBtn.clicked += () => Debug.Log("Go Back");
    }

    private void JoinToGame()
    {
        Debug.Log("Joining game");
        AsyncOperation loadGame = SceneManager.LoadSceneAsync("TestWorld", LoadSceneMode.Additive);
        loadGame.completed += OnLoadCompleted;
    }

    private async void OnLoadCompleted(AsyncOperation operation)
    {
        try
        {
            await OnlineServices.JoinRelay(_codeField.text);
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error during scene load: {e.Message}");
            SceneManager.UnloadSceneAsync("TestWorld");
        }
    }
}
