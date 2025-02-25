using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GSC_NavigationUserInput
{
    NoInput, PreviousScene, NextScene, EndNavigation
}

public class GSC_ScriptManager : GSC_Singleton<GSC_ScriptManager>
{
    private bool NavigationMode;
    private GSC_StoryUnit story;
    private int CurrentChapter;
    private int CurrentSequence;
    private int CurrentScene;
    private int NavigationScene;
    private GSC_NavigationUserInput userInput;
   
    public bool SetupStoryPoint(int chapter,int sequence,int scene)
    {
        if (story == null) return false;
        if(chapter < 0 || story.Chapters.Count <= chapter) return false;
        if(sequence < 0 || story.Chapters[chapter].Sequences.Count <= sequence) return false;
        if(scene < 0 || story.Chapters[chapter].Sequences[sequence].Scenes.Count <= scene) return false;
        CurrentChapter = chapter;
        CurrentSequence = sequence;
        CurrentScene = scene;
        NavigationScene = scene;
        return true;
    }

    public bool Pass()
    {
        CurrentScene++;
        if (story.Chapters[CurrentChapter].Sequences[CurrentSequence].Scenes.Count == CurrentScene)
        {
            CurrentScene = 0;
            CurrentSequence++;
            if (story.Chapters[CurrentChapter].Sequences.Count == CurrentSequence)
            {
                CurrentSequence = 0;
                CurrentChapter++;
                if (story.Chapters.Count == CurrentChapter) return false;
            }
        }
        return true;
    }

    public GSC_ContainerUnit[] GetScene()
    {
        if (story == null) return new GSC_ContainerUnit[0];
        
        int Scene = NavigationMode ? NavigationScene : CurrentScene;

        GSC_SceneAction[] actions = story
            .Chapters[CurrentChapter]
            .Sequences[CurrentSequence]
            .Scenes[Scene].Actions.ToArray();
        Debug.LogWarning($"Scene: {Scene}");
        

        if (actions != null && actions.Length > 0)
        {
            List<GSC_ContainerUnit> units = new List<GSC_ContainerUnit>();
            foreach (var action in actions) units.Add(action.Compile());
            return units.ToArray();
        }
        
        return new GSC_ContainerUnit[0];
    }

    public IEnumerator RunScene()
    {
        GSC_ContainerUnit[] containers = GetScene();
        if (containers != null && containers.Length > 0)
        {
            foreach (var command in containers)
                GSC_CommandManager.Instance.EnqueueCommand(command);
        }

        while (!GSC_CommandManager.Instance.IsIdle())
        {
            yield return null;
        }

    }

    public IEnumerator RunStorySequence(GSC_StoryUnit MainStory, int chapterIndex, int sequenceIndex, int sceneIndex)
    {
        if (MainStory == null) yield break;
        story = MainStory;

        bool runStory = SetupStoryPoint(chapterIndex, sequenceIndex, sceneIndex);
        if(!runStory) yield break;

        Debug.Log("Iniciando a execução do script");
        GSC_GraphicsManager.Instance.RemoveAll();
        GSC_CommandManager.Instance.Wipe();

        while (runStory)
        {
            yield return RunScene();        
            runStory = Pass();
        }
        // Opcionalmente, redireciona para outra cena ou estado
        // GSC_GameManager.Instance.ToStartScreen();
    }

    public void RunStory(GSC_StoryUnit MainStory,
        int chapterIndex = 0, int blockIndex = 0, int sceneIndex = 0)
    {
        StartCoroutine(RunStorySequence(MainStory, chapterIndex, blockIndex, sceneIndex));
    }

    public void StartNavigation()
    {
        StopAllCoroutines();
        NavigationMode = true;
        NavigationScene = CurrentScene;
        StartCoroutine(RunNavigation());
    }

    public void PreviousScene() => userInput = GSC_NavigationUserInput.PreviousScene;
    public void NextScene() => userInput = GSC_NavigationUserInput.NextScene;
    public void EndNavigation() => userInput = GSC_NavigationUserInput.EndNavigation;

    private IEnumerator RunNavigation()
    {
        while (true)
        {
            GSC_NavigationUserInput input = GetUserInput();

            if (input == GSC_NavigationUserInput.PreviousScene)
            {
                if (Decrement())
                {
                    GSC_CommandManager.Instance.Wipe();
                    yield return RunScene();
                }
            }
            else if (input == GSC_NavigationUserInput.NextScene)
            {
                if (Increment())
                {
                    GSC_CommandManager.Instance.Wipe();
                    yield return RunScene();
                }
                else break;
                
            }
            else if (input == GSC_NavigationUserInput.EndNavigation)
            {
                break;
            }

            yield return null;
        }
        ContinueStory();
    }

    private bool Increment()
    {
        NavigationScene++;
        if (NavigationScene > CurrentScene) return false;
        return true;
    }

    private bool Decrement()
    {
        NavigationScene--;
        if(NavigationScene < 0) return false;
        return true;
    }

    private void ContinueStory()
    {
        NavigationMode = false;
        StopAllCoroutines();
        RunStory(story, CurrentChapter, CurrentSequence, CurrentScene);
    }

    private GSC_NavigationUserInput GetUserInput()
    {
        GSC_NavigationUserInput currentInput = userInput;
        userInput = GSC_NavigationUserInput.NoInput;
        return currentInput;
    }
}

