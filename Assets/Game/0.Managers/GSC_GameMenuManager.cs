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
    
    public void OnStartGame()
    {
        GSC_ContainerUnit unit = new GSC_ContainerUnit("OnStartGame");
        unit.Set("FadeTime", 2f);
        GSC_CommandManager.Instance.EnqueueCommand(unit);
    }

    #endregion

    #region Menu Fades
    public IEnumerator ShowMainMenu(float fadeTime, Func<bool> pause, Func<bool> end)
    {
        yield return MainMenu.FadeIn(fadeTime,pause,end);
        MainMenu.Enable();
    }

    public IEnumerator HideMenu(float fadeTime, Func<bool> pause, Func<bool> end)
    {
        yield return MainMenu.FadeOut(fadeTime,pause,end);
        MainMenu.Hide();
    }

    public void HideMainMenu()
    {
        MainMenu.Hide();
    }
    #endregion
}