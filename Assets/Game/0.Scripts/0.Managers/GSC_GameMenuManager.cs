using System;
using System.Collections;
using UnityEngine;

public class GSC_GameMenuManager : GSC_Singleton<GSC_GameMenuManager>
{
    
    [SerializeField] private GSC_MainMenuController MainMenu;

    public void HideAllMenus()
    {
        MainMenu.Hide();
    }


    #region MenuActions

    public void OnStartGame() => GSC_GameManager.Instance.StartStory();

    public IEnumerator ShowMainMenu(float fadeTime)
    {
        HideAllMenus();
        yield return MainMenu.FadeIn(fadeTime,() => false,() => false);
        MainMenu.Enable();
    }

    #endregion
}