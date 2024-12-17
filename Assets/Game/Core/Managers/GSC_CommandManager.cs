using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class GSC_CommandManager : GSC_Singleton<GSC_CommandManager>
{
    public GSC_CommandDatabase Command { get; private set; }
    public bool IsExecuting { get; internal set; }

    private bool IsGamePaused;
    private Queue<GSC_CommandHandlerBase> Coroutines = new Queue<GSC_CommandHandlerBase>();
    private GSC_CommandHandlerBase CurrentHandler;
    private bool RequestToTerminate;
    private bool RequestToWipe;
    
    #region Command Handler

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
                    // Dequeue a handler and set it as the current one
                    CurrentHandler = Coroutines.Dequeue();
                    //If request to wipe is set, we don't want this to yield, but jump to end.
                    if (RequestToWipe == true) RequestToTerminate = true;
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

    private IEnumerator WrapCoroutine(GSC_CommandHandlerBase handler)
    {
        if (handler.IsCoroutine())
        {
            while (handler.MoveNext())
            {
                // If the game is paused, wait until it resumes
                while (IsGamePaused)
                {
                    yield return null;
                }

                if (RequestToTerminate == true)
                {
                    RequestToTerminate = false;
                    handler.TerminateHandler();
                    yield break;
                }

                yield return handler.GetCurrent();
            }
        }
        // Invoke the terminate callback after execution
        handler.TerminateHandler();
    }

   
    public void Terminate() => RequestToTerminate = true;
    public void Wipe() => RequestToWipe = true;
    public void PauseGame() => IsGamePaused = true;
    public void ResumeGame() => IsGamePaused = false;

    #endregion

    public void Start()
    {
        Command = GSC_CommandDatabase.New();
        Assembly assembly = Assembly.GetExecutingAssembly();
        Type[] Types = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(GSC_CommandRegister))).ToArray();
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

    public void Execute(GSC_ContainerUnit unit)
    {
        GSC_CommandHandlerBase handler = Command.GetHandler(unit);
        if (handler != null) Coroutines.Enqueue(handler);
    }
    public void Execute<TParam>(GSC_ContainerUnit<TParam> unit)
    {
        GSC_CommandHandlerBase handler = Command.GetHandler(unit);
        if (handler != null) Coroutines.Enqueue(handler);
    }
}

public abstract class GSC_CommandRegister
{
    public static void AddCommands(GSC_CommandDatabase database) { }
}

public class GSC_BasicCommandRegister : GSC_CommandRegister
{
    public new static void AddCommands(GSC_CommandDatabase database)
    {
        database.AddCommand("DebugMessage", new Action<GSC_ContainerUnit>(DebugMessage));
        database.AddCommand("TestCoroutine", new Func<GSC_ContainerUnit, IEnumerator>(TestCoroutine), null)
            .With("Counter",GSC_ParameterType.NonZero_Positive_Integer);
    }

    public static void DebugMessage(GSC_ContainerUnit unit)
    {
        Debug.Log("This is a message");
    }

    public static IEnumerator TestCoroutine(GSC_ContainerUnit unit)
    {
        int count = unit.GetInteger("Counter");
        for(int i = 0; i < count; i++)
        {
            Debug.Log($"This will repeat {count - i} more times");
            yield return new WaitForSeconds(2f);
        }
    }
}