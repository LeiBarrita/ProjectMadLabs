using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    // [SerializeField] private int maxPlayers = 4;

    private VisualElement _onlineUI;
    private VisualElement _joinUI;

    async void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        _onlineUI = root.Q("OnlineGameUI");
        _joinUI = root.Q("JoinOnlineUI");

        SetupStartMenu();
        SetupJoinMenu();
        await OnlineServices.StartAnonymousSession();
    }


    private void SetupStartMenu()
    {
        OnlineMenuUI onlineMenu = new(_onlineUI)
        {
            OpenJoinOnline = () => ToggleJoinMenu(true)
        };
    }

    private void SetupJoinMenu()
    {
        JoinMenuUI joinMenu = new(_joinUI)
        {
            GoBack = () => ToggleJoinMenu(false)
        };
    }

    private void ToggleJoinMenu(bool enabled)
    {
        _onlineUI.style.display = !enabled ? DisplayStyle.Flex : DisplayStyle.None;
        _joinUI.style.display = enabled ? DisplayStyle.Flex : DisplayStyle.None;
    }
}

