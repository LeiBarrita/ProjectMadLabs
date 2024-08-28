using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class JoinMenuUI : MonoBehaviour
{
    public Action GoBack { set => _backBtn.clicked += value; }

    private Button _joinBtn;
    private Button _backBtn;

    public JoinMenuUI(VisualElement root)
    {
        _joinBtn = root.Q<Button>("JoinBtn");
        _backBtn = root.Q<Button>("BackBtn");

        AddListeners();
    }

    private void AddListeners()
    {
        _joinBtn.clicked += () => Debug.Log("Join Game");
        _backBtn.clicked += () => Debug.Log("Go Back");
    }
}
