using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GSC_CommandManager;

public enum GSC_NavigationUserInput
{
    NoInput, PreviousScene, NextScene, EndNavigation
}

public class GSC_ScriptManager : GSC_Singleton<GSC_ScriptManager>
{
    [SerializeField] private GSC_Story Story;
    private GSC_SceneSequenceUnit Sequence;
    private bool NavigationMode;
    private int CurrentScene;
    private int NavigationScene;
    private GSC_NavigationUserInput userInput;

    public void StartStory()
    {
        Story.CreateStoryFromScript();
        CurrentScene = 0;
        Sequence = Story.Sequences[0];
        RunStory(Sequence);
    }

  
    public bool SetupStoryPoint(GSC_SceneSequenceUnit sequence, int scene = 0)
    {
        if (sequence == null) Debug.Log("A merda ta na sequencia");
        Sequence = sequence;
        CurrentScene = scene;
        NavigationScene = scene;
        return true;
    }

    public bool Pass()
    {
        CurrentScene++;
        if (Sequence == null) Debug.Log("Vai dar merda");
        if (Sequence.Scenes.Count == CurrentScene) return false;
        return true;
    }

    public GSC_CommandAction[] GetScene()
    {
        if (Sequence == null) return new GSC_CommandAction[0];
        
        int Scene = NavigationMode ? NavigationScene : CurrentScene;

        if (!NavigationMode) Sequence.Scenes[Scene].SetAllValues();

        GSC_ScriptAction[] actions = Sequence.Scenes[Scene].Actions.ToArray();
        Debug.LogWarning($"Scene: {Scene}");
        
        if (!actions.IsNullOrEmpty())
        {
            List<GSC_CommandAction> units 
                = new List<GSC_CommandAction>();
            
            foreach (var action in actions)
            {
                units.Add(action.GetAction());
            }
            return units.ToArray();
        }
        
        return new GSC_CommandAction[0];
    }

    public IEnumerator RunScene()
    {
        GSC_CommandAction[] commands = GetScene();
        if (!commands.IsNullOrEmpty())
        {
            GSC_CommandManager.Instance.PrepareToAction();
            foreach (var command in commands)
                GSC_CommandManager.Instance.EnqueueCommand(command);
        }

        // Aguarda até que o CommandManager esteja realmente Idle
        yield return new WaitUntil(() => GSC_CommandManager.Instance.IsIdle());
    }

    public IEnumerator RunStorySequence(GSC_SceneSequenceUnit sequence, int sceneIndex)
    {
        bool runStory = SetupStoryPoint(sequence,sceneIndex);
        if(!runStory) yield break;
        
        Debug.Log("Iniciando a execução do script");
        GSC_GraphicsManager.Instance.RemoveAll();
        GSC_CommandManager.Instance.Wipe();

        while (runStory)
        {
            yield return RunScene();        
            runStory = Pass();

            if (!runStory)
            {
                GSC_SceneSequenceUnit nextSequence = null;
                while (nextSequence == null)
                {
                    nextSequence = Story.GetSequence();
                    if (nextSequence != null)
                    {
                        SetupStoryPoint(nextSequence);
                        runStory = true;
                    }
                    yield return null;
                }
            }

        }
    }

    public void RunStory(GSC_SceneSequenceUnit sequence, int sceneIndex = 0)
    {
        StartCoroutine(RunStorySequence(sequence, sceneIndex));
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
        RunStory(Sequence, CurrentScene);
    }

    private GSC_NavigationUserInput GetUserInput()
    {
        GSC_NavigationUserInput currentInput = userInput;
        userInput = GSC_NavigationUserInput.NoInput;
        return currentInput;
    }
}
