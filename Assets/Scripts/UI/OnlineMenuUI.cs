using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OnlineMenuUI : MonoBehaviour
{
    public Action OpenJoinOnline { set => _joinBtn.clicked += value; }

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
        _hostBtn.clicked += () => Debug.Log("Host Game");
        _joinBtn.clicked += () => Debug.Log("Join Game Menu");
    }
}
