using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GSC_GameManager : GSC_Singleton<GSC_GameManager>
{
    [SerializeField] private GSC_StoryData StoryData;
    [SerializeField] private GSC_GameComponents Components;
    [Header("Menus")]
    [SerializeField] private GSC_MainMenuController MainMenu;
    [SerializeField] private List<GSC_CanvasGroupController> Menus;
    
    private GSC_GameData Data;
    private GSC_ScreenMessageController Alert;
    private GSC_ScreenMessageController CurrentController;

    public bool InputRequested { get; private set; }
   
    public void RequestInput() => InputRequested = false;
    private void OnInput()
    {
        if(CurrentController != null && CurrentController.IsBuilding)
        {
            CurrentController.CompleteDialogue();
        }
        else InputRequested = true;
    }

    public void Start()
    {
        if (Menus.Count > 0)
        {
            foreach (GSC_CanvasGroupController menu in Menus) menu.Disable();
        }
        MainMenu.Disable();
        Data = new GSC_GameData();
        Data.Initialize();
        Alert = Components.GetController("Alert");
        Components.ScreenInput.RegisterEvent(OnInput, OnInput);
        GSC_ScriptManager.Instance.InitializeScriptHandling();
        MainMenu.RunIntro();
    }

    #region Component Acessors
    public GSC_ScreenMessageController GetMessageController(string messageType)
    {
        CurrentController = Components.GetController(messageType);
        return CurrentController;
    }
    
    public GSC_ImageLayerController Controller(string controllerName)
    {
        return controllerName switch
        {
            "Background" => Components.GetController(GSC_ImageControllers.BACKGROUND),
            "Characters" => Components.GetController(GSC_ImageControllers.CHARACTERS),
            "Foreground" => Components.GetController(GSC_ImageControllers.FOREGROUND),
            _ => null
        };
    }

    public void DisableScreenInput()
    {
        Components.ScreenInput.Disable();
        Alert.Disable();
    }
    
    public void EnableScreenInput()
    {
        Components.ScreenInput.Enable(true);
        Alert.Enable(true);
    }
    
    public void HandleInput(bool toSystem)
        => Data.Set(Components.InputPanel.Parameter, toSystem);
    public void HandleChoice(bool toSystem)
        => Data.Set(Components.ChoicePanel.GetOption(), toSystem);

    public Sprite GetSprite(string group, string spriteName)
        => StoryData.GetSprite(group, spriteName);
    
    //This will process string from the game database.
    public string ProcessString(string text)
    {
        string pattern1 = @"\{\{(.*?)\}\}";
        string pattern2 = @"\[\[(.*?)\]\]";
        string pattern3 = @"\<\<(.*?)\>\>";

        var groups = Regex.Matches(text, pattern1);

        for (int i = 0; i < groups.Count; i++)
        {
            string value = Data.GetAsString(groups[i].Value[2..^2],true);
            text.Replace(groups[i].Value, value);
        }

        groups = Regex.Matches(text, pattern2);

        for (int i = 0; i < groups.Count; i++)
        {
            string value = Data.GetAsString(groups[i].Value[2..^2],false);
            text.Replace(groups[i].Value, value);
        }

        groups = Regex.Matches(text, pattern3);

        for (int i = 0; i < groups.Count; i++)
        {
            string value = GSC_SystemInfo.GetAsString(groups[i].Value[2..^2]);
            text.Replace(groups[i].Value, value);
        }

        return text;
    }

    public IEnumerator WaitForComplete()
    {
        InputRequested = false;
        if (!Alert.IsVisible) Alert.Enable(false);
        yield return Alert.ShowMessage("Press on screen to continue...", 0.5f);
        while (InputRequested == false)
        {
            yield return null;
        }
        Alert.ClearText();
        InputRequested = false;
    }

    public IEnumerator WaitForSeconds(float seconds)
    {
        float elapsedTime = 0f;
        while (elapsedTime < seconds)
        {
            if (InputRequested == true)
            {
                yield break;
            }
            else 
            {
                elapsedTime += Time.deltaTime;
            }
            yield return null;
        }
    }

    #endregion

    #region Story Methods

    public void LoadStory(GSC_GameSave save)
    {
        GSC_Parameter chapterParameter = save.Database.Get("ChapterIndex");
        if (chapterParameter is GSC_IntegerParameter @chapter)
        {
            GSC_Parameter sceneParameter = save.Database.Get("SceneIndex");
            if(sceneParameter is GSC_IntegerParameter @scene)
            {
                MainMenu.LoadStoryFrom(chapter.Value,scene.Value);
            }
        }
    }

    //Adjust it to be better later.
    public IEnumerator ShowMainMenu(float fadetime = 0)
    {
        DisableScreenInput();
        yield return MainMenu.FadeIn(fadetime);
        MainMenu.Enable(true);
        while(MainMenu.IsClicked == false) yield return null;
        MainMenu.ResetClick();
        Components.DisableAll();
        yield return MainMenu.FadeOut(fadetime);
        MainMenu.Disable();
    }

    internal bool Callback()
    {
        throw new NotImplementedException();
    }
    #endregion
}
