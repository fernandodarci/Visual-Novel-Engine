using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

public class GSC_GameManager : GSC_Singleton<GSC_GameManager>
{
    [SerializeField] private GSC_StoryData StoryData;
    [SerializeField] private GSC_GameComponents Components;
    private GSC_StoryUnit Story;
    private GSC_GameData Data;
    public bool InputRequested { get; private set; }
    public GSC_CommandDatabase Command { get; private set; }
    public bool IsExecuting { get; internal set; }

    private bool IsGamePaused;
    private Queue<GSC_CommandHandler> Coroutines = new Queue<GSC_CommandHandler>();
    private GSC_CommandHandler CurrentHandler;
    public bool RequestToTerminate { get; private set; }
    private bool RequestToWipe;
    private bool EndCoroutine;

    public void RequestInput() => InputRequested = false;
    private void RequestComplete() => InputRequested = true;

   

    public void Start()
    {
        InitializeScriptHandling();
        Data = new GSC_GameData();
        Data.Initialize();
        Components.ScreenInput.RegisterEvent(RequestComplete, RequestComplete);
    }

    #region Component Acessors
    public GSC_DialogueController Dialogue => Components.Dialogue;
    public GSC_InputPanelController Input => Components.InputPanel;
    public GSC_ChoicePanelController Choice => Components.ChoicePanel;
    public void DisableScreenInput()
        => Components.ScreenInput.gameObject.SetActive(false);
    public void EnableScreenInput()
        => Components.ScreenInput.gameObject.SetActive(true);
    public void HandleInput(bool toSystem)
        => Data.Set(Components.InputPanel.Parameter, toSystem);
    public void HandleChoice(bool toSystem)
        => Data.Set(Components.ChoicePanel.GetOption(), toSystem);
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

    #endregion

    #region ScriptHandling

    private void InitializeScriptHandling()
    {
        Command = GSC_CommandDatabase.New();
        Assembly assembly = Assembly.GetExecutingAssembly();
        Type[] Types = assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(GSC_CommandRegister)))
            .ToArray();

        if (Types.Any())
        {
            foreach (var type in Types)
            {
                MethodInfo method = type.GetMethod("AddCommands");
                if (method != null)
                {
                    try
                    {
                        method.Invoke(null, new object[] { Command });
                        Debug.Log($"Successfully invoked AddCommands on {type.Name}");
                    }
                    catch (TargetException ex)
                    {
                        Debug.LogError($"TargetException invoking AddCommands on {type.Name}: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Exception invoking AddCommands on {type.Name}: {ex.Message}");
                    }
                }
                else
                {
                    Debug.Log("Method is null");
                }
            }
        }
        StartCoroutine(RunOnce());


        Story = GSC_StoryScript.GetScript();


        foreach (var chapter in Story.Chapters)
        {
            foreach (var scenes in chapter.Scenes)
            {
                foreach (var container in scenes.Containers)
                {
                    Execute(container);
                }
            }
        }
    }

    private IEnumerator RunOnce()
    {
        while (true)
        {
            if ((Coroutines.Count == 0 && CurrentHandler == null) || IsGamePaused)
            {
                IsExecuting = false;
                yield return null;
            }
            else
            {
                IsExecuting = true;

                if (CurrentHandler == null && Coroutines.Count > 0)
                {
                    CurrentHandler = Coroutines.Dequeue();
                    Debug.Log($"Dequeue new command: {CurrentHandler.HandlerName}");
                    //If request to wipe is set, we don't want this to yield, but jump to end.
                    RequestToTerminate = RequestToWipe;
                    //So if we don't have coroutines on queue, the work is done for it.
                    if (Coroutines.Count == 0) RequestToWipe = false;
                }
                else if (CurrentHandler != null)
                {
                    // Execute the current coroutine
                    yield return WrapCoroutine(CurrentHandler);
                    CurrentHandler = null; // Reset after execution
                }
            }
        }
    }

    private IEnumerator WrapCoroutine(GSC_CommandHandler handler)
    {
        if (handler.IsCoroutine())
        {
            EndCoroutine = false;

            while (!EndCoroutine)
            {
                // If the game is paused, wait until it resumes
                while (IsGamePaused)
                {
                    yield return null;
                }

                yield return handler.Run();
            }
        }
        // Invoke the terminate callback after execution
        handler?.OnEnd();
    }


    public void Terminate() => RequestToTerminate = true;
    public void Wipe() => RequestToWipe = true;
    public void PauseGame() => IsGamePaused = true;
    public void ResumeGame() => IsGamePaused = false;


    public void Execute(GSC_ContainerUnit unit)
    {
        GSC_CommandHandler handler = Command.GetHandler(unit);
        if (handler != null) Coroutines.Enqueue(handler);
    }

    public void Ends() => EndCoroutine = true;

    public IEnumerator Yields(float seconds)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < seconds)
        {
            if (RequestToTerminate) yield break;
            elapsedTime = Time.time - startTime;
            yield return null;
        }
    }

    public IEnumerator WaitForComplete()
    {
        InputRequested = false;
        yield return Dialogue.ShowAlert();
        while(InputRequested == false)
        {
            yield return null;
        }
        InputRequested = false;
    }

   
    #endregion
}
