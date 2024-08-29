using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUI : MonoBehaviour
{
    private Label _codeField;

    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        _codeField = root.Q<Label>("JoinCode");
        if (_codeField != null) _codeField.text = GameStateManager.joinCode;
    }
}
