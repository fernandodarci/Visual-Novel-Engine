using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class GSC_GameManager : GSC_Singleton<GSC_GameManager>
{
    private static GSC_DialogueManager Dialogues => GSC_DialogueManager.Instance;
    private static GSC_CommandManager Command => GSC_CommandManager.Instance;
    private static GSC_ScriptManager Script => GSC_ScriptManager.Instance;
    private static GSC_DataManager Data => GSC_DataManager.Instance;

    public bool DeveloperMode { get; private set; }

    [SerializeField] private Canvas MainCanvas;
    [SerializeField] private Canvas OverlayCanvas;
    [Header("Story")]
    [SerializeField] private TextAsset Intro;
    [SerializeField] private GSC_StoryUnit MainStory;
    [Header("Screen Controllers")]
    [SerializeField] private GSC_ScreenInput screenInput;
    [SerializeField] private GSC_GameMenuController gameMenu;
    private bool NavigationModeInput = false;

    private void Start()
    {
        screenInput.RegisterEvent(OnLeftInput, OnRightInput);
        Dialogues.HideAllMessages();
        Command.InitializeCommandHandling();
        Data.InitializeData();
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

    public void ToStartScreen()
    {
        GSC_GameMenuManager.Instance.HideAllMenus();
        OverlayCanvas.gameObject.SetActive(true);
        MainCanvas.gameObject.SetActive(false);
        //Solução porca para o problema. Ele deve usar uma chamada que vai passar pelo GSC_CommandManager
        //com os parametros corretos.
        GSC_GameMenuManager.Instance.ShowMainMenu(2f,() => false,() => false);
    }
    ////This will process string from the game database.
    //public string ProcessString(string text)
    //{
    //    string pattern1 = @"\{\{(.*?)\}\}";
    //    string pattern2 = @"\[\[(.*?)\]\]";
    //    string pattern3 = @"\<\<(.*?)\>\>";

    //    var groups = Regex.Matches(text, pattern1);

    //    for (int i = 0; i < groups.Count; i++)
    //    {
    //        string value = Data.GetAsString(groups[i].Value[2..^2], true);
    //        text.Replace(groups[i].Value, value);
    //    }

    //    groups = Regex.Matches(text, pattern2);

    //    for (int i = 0; i < groups.Count; i++)
    //    {
    //        string value = Data.GetAsString(groups[i].Value[2..^2], false);
    //        text.Replace(groups[i].Value, value);
    //    }

    //    groups = Regex.Matches(text, pattern3);

    //    for (int i = 0; i < groups.Count; i++)
    //    {
    //        string value = GSC_SystemInfo.GetAsString(groups[i].Value[2..^2]);
    //        text.Replace(groups[i].Value, value);
    //    }

    //    return text;
    //}

   
   #region Story Methods

    public void LoadStory()
    {
        
    }

   
    internal void HandleInput(bool systemParam)
    {
        throw new NotImplementedException();
    }

    internal void HandleChoice(bool v)
    {
        throw new NotImplementedException();
    }

  

    public void StartStory()
    {
        GSC_GameMenuManager.Instance.HideMainMenu();
        OverlayCanvas.gameObject.SetActive(false);
        MainCanvas.gameObject.SetActive(true);
        GSC_ScriptManager.Instance.RunStory(MainStory);
    }

    public void ActivateNavigationMode()
    {
        NavigationModeInput = true;
        Script.StartNavigation();
    }



    #endregion
}
