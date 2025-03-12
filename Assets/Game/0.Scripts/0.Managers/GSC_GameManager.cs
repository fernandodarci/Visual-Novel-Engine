using System;
using System.Collections;
using UnityEngine;

public class GSC_GameManager : GSC_Singleton<GSC_GameManager>
{
    private static GSC_DialogueManager Dialogues => GSC_DialogueManager.Instance;
    private static GSC_CommandManager Command => GSC_CommandManager.Instance;
    private static GSC_ScriptManager Script => GSC_ScriptManager.Instance;
    private static GSC_DataManager Data => GSC_DataManager.Instance;

    private static GSC_GameMenuManager Menu => GSC_GameMenuManager.Instance;

    [SerializeField] private Canvas MainCanvas;
    [SerializeField] private Canvas OverlayCanvas;
    [Header("Story")]
    [SerializeField] private GSC_CanvasGroupController Logo;
    [SerializeField] private GSC_CanvasGroupController Disclaimer;
    [Header("Screen Controllers")]
    [SerializeField] private GSC_ScreenInput ScreenInputController;
    [SerializeField] private GSC_InGameMenuController InGameMenuController;
    [SerializeField] private GSC_NavigationPointsController NavigationController;
    [Header("Developer Option")]
    [SerializeField] private bool DeveloperMode;
    private bool NavigationModeInput = false;

    private void Start()
    {
        ScreenInputController.RegisterEvent(OnLeftInput, OnRightInput);
        Dialogues.HideAllMessages();
        Menu.HideAllMenus();
        Data.InitializeData();
        Logo.Hide();
        Disclaimer.Hide();
        StartCoroutine(ToStartGame());
    }

    private void OnLeftInput()
    {
        Debug.LogWarning("Press Left");
        if (NavigationModeInput == false) Command.RequestEnd();
        else Script.PreviousScene();
    }

    private void OnRightInput()
    {
        Debug.LogWarning("Press Right");
        if (NavigationModeInput == false) Command.RequestEnd();
        else Script.NextScene();
    }

    #region NAVIGATORS

    private IEnumerator ToStartGame()
    {
        if (DeveloperMode == false)
        {
            yield return Logo.FadeIn(3f, () => false, () => false);
            yield return new WaitForSeconds(2f);
            yield return Logo.FadeOut(3f, () => false, () => false);
            yield return Disclaimer.FadeIn(2f, () => false, () => false);
            yield return new WaitForSeconds(10f);
            yield return Disclaimer.FadeOut(2f, () => false, () => false);
        }
        yield return Menu.ShowMainMenu(2f);
    }

    public void ToStartScreen()
    {
        GSC_GameMenuManager.Instance.HideAllMenus();
        OverlayCanvas.gameObject.SetActive(true);
        MainCanvas.gameObject.SetActive(false);
        StartCoroutine(GSC_GameMenuManager.Instance.ShowMainMenu(2f));
    }
    
    public void ToPrepareInterfaces()
    {
        ScreenInputController.Hide();
        InGameMenuController.Hide();
        NavigationController.Hide();
    }

    public GSC_NavigationPointsController GetNavPoints()
    {
        return NavigationController;
    }



    #endregion

    #region Story Methods

    public void LoadStory()
    {
        
    }

    public void StartStory()
    {
        Debug.Log("Story Started");
        GSC_GameMenuManager.Instance.HideAllMenus();
        OverlayCanvas.gameObject.SetActive(false);
        MainCanvas.gameObject.SetActive(true);
        GSC_ScriptManager.Instance.StartStory();
    }

    public void ActivateNavigationMode()
    {
        NavigationModeInput = true;
        Script.StartNavigation();
    }

  

    #endregion
}
