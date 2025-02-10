using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class GSC_ScriptManager : GSC_Singleton<GSC_ScriptManager>
{
    private GSC_CommandDatabase Command;
    private bool IsGamePaused;
    private Queue<GSC_ContainerUnit> Coroutines = new Queue<GSC_ContainerUnit>();
    private GSC_CommandHandler CurrentHandler;
    private GSC_StoryUnit Story;
    public bool RequestToTerminate { get; private set; }
    private bool _alreadyRunning;
    private bool RequestToWipe;
    private bool EndCoroutine;
    private Coroutine storyRunner;
    private bool ToNextScene;

    public bool IsExecuting { get; internal set; }

    public void InitializeScriptHandling()
    {
        //This will guarantee that this method is called just once
        if (_alreadyRunning == true) return;
        _alreadyRunning = true;

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
        
    }

    public void RunSequence(GSC_StoryUnit unit, int chapterIndex, int sceneIndex)
    {
        if(storyRunner != null) StopCoroutine(storyRunner);
        ToNextScene = true;
        storyRunner = StartCoroutine(RunSequenceCoroutine(unit, chapterIndex, sceneIndex));
    }

    public void RunScene(GSC_StoryUnit unit, int chapterIndex, int sceneIndex)
    {
        Story = unit;
        if (chapterIndex < 0 || chapterIndex >= Story.Chapters.Count) return;
        GSC_ChapterUnit chapter = Story.Chapters[chapterIndex];
        if(sceneIndex < 0 || sceneIndex >= chapter.SceneList.Count) return;

        Debug.Log("Run single scene");
        Coroutines.Clear();
        GSC_GameManager.Instance.ClearAllImages();
        Wipe();

        foreach (var container in chapter.GetScene(sceneIndex))
        {
            Debug.Log($"Enqueue coroutine {container.Calling} in script. [{Coroutines.Count}]");
            Coroutines.Enqueue(container);
        }

    }

    private IEnumerator RunSequenceCoroutine(GSC_StoryUnit unit, int chapterIndex, int sceneIndex)
    { 
        Story = unit;
        Debug.Log("Start to run the story");
        Coroutines.Clear();
        GSC_GameManager.Instance.ClearAllImages();
        Wipe();

        for (int i = 0; i < Story.Chapters.Count; i++)
        {
            var chapter = Story.Chapters[i];
            int sceneCount = chapter.GetScenesOnChapter();
            int startSceneIndex = 0;

            for (int j = startSceneIndex; j < sceneCount; j++)
            {
                foreach (var container in chapter.GetScene(j))
                {
                    Debug.Log($"Enqueue coroutine {container.Calling} in script. [{Coroutines.Count}]");
                    Coroutines.Enqueue(container);
                }
                while (ToNextScene == false) yield return null;
                ToNextScene = false;
            }
        }
        Coroutines.Enqueue(GSC_MainMenuController.MainMenu);
    }

    private IEnumerator RunOnce()
    {
        while (true)
        {
            if ((Coroutines.Count == 0 && CurrentHandler == null) || IsGamePaused)
            {
                IsExecuting = false;
                ToNextScene = true;
                yield return null;
            }
            else
            {
                IsExecuting = true;

                if (CurrentHandler == null && Coroutines.Count > 0)
                {
                    Execute(Coroutines.Dequeue());
                    if (CurrentHandler != null)
                    {
                        Debug.Log($"Dequeue new command: {CurrentHandler.HandlerName}");
                    }
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

        if (handler != null) CurrentHandler = handler;
        if (handler.HandlerName == "ShowDialogue")
        {
            Debug.Log($"{handler.Unit.GetString("Dialogue")}");
        }
    }

    public void Ends()
    {
        Debug.Log("Ending current coroutine");
        EndCoroutine = true;
    }

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
}