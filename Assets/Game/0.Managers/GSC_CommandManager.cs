using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public enum GSC_CommandState
{
    Idle, Paused, Executing
}

public class GSC_CommandManager : GSC_Singleton<GSC_CommandManager>
{
    private GSC_CommandDatabase commandDatabase;
    private Queue<GSC_ContainerUnit> commandQueue = new Queue<GSC_ContainerUnit>();
    private GSC_CommandHandler currentHandler;

    private GSC_CommandState CurrentState;
    
    public bool IsIdle() => CurrentState == GSC_CommandState.Idle;
    private bool IsPaused() => CurrentState == GSC_CommandState.Paused;
    private bool endCurrentCoroutine;
    private bool requestEnd;

    private bool IsEndRequested()
    {
        if (requestEnd)
        {
            requestEnd = false;
            return true;
        }
        return false;
    }
    
    public void InitializeCommandHandling()
    {
        if (commandDatabase != null) return;

        commandDatabase = GSC_CommandDatabase.New();
        Assembly assembly = Assembly.GetExecutingAssembly();
        Type[] types = assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(GSC_CommandRegister)))
            .ToArray();

        foreach (var type in types)
        {
            MethodInfo method = type.GetMethod("AddCommands");
            if (method != null)
            {
                try
                {
                    method.Invoke(null, new object[] { commandDatabase });
                    Debug.Log($"Successfully invoked AddCommands on {type.Name}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error invoking AddCommands on {type.Name}: {ex.Message}");
                }
            }
            else
            {
                Debug.LogWarning($"Method AddCommands not found on {type.Name}");
            }
        }

        // Inicia a execução contínua dos comandos
        StartCoroutine(CommandExecutionLoop());
    }

    private IEnumerator CommandExecutionLoop()
    {
        while (true)
        {
            if (CurrentState == GSC_CommandState.Paused)
            {
                yield return null;
            }
            else if (commandQueue.Count == 0)
            {
                CurrentState = GSC_CommandState.Idle;
                yield return null;
            }
            else
            {
                GSC_ContainerUnit unit = commandQueue.Dequeue();
                if (unit != null)
                {
                    GSC_CommandHandler handler = commandDatabase.GetHandler(unit);
                    if (handler != null)
                    {
                        currentHandler = handler;
                        CurrentState = GSC_CommandState.Executing;
                        yield return WrapCoroutine(currentHandler);
                        currentHandler = null;
                        Debug.Log("Action End");
                        if (commandQueue.Count == 0) CurrentState = GSC_CommandState.Idle;
                        else Debug.Log($"Remaining {commandQueue.Count} actions to end");
                    }
                }
            }
            yield return null;
        }
    }

    public void EnqueueCommand(GSC_ContainerUnit commandUnit)
    {
        commandQueue.Enqueue(commandUnit);
    }


    private IEnumerator WrapCoroutine(GSC_CommandHandler handler)
    {
        if (handler.IsCoroutine())
        {
            endCurrentCoroutine = false;
            while (!endCurrentCoroutine)
            {
                yield return handler.Run(IsPaused,IsEndRequested);
            }
        }
        handler?.OnEnd();
    }

    public void EndCurrentCoroutine() => endCurrentCoroutine = true;
    public void Pause() => CurrentState = GSC_CommandState.Paused;
    public void Resume()
    {
        CurrentState = currentHandler != null ? GSC_CommandState.Executing : GSC_CommandState.Idle;
    }
    public void Wipe()
    {
        commandQueue.Clear();
        endCurrentCoroutine = true;
    }
    public void RequestEnd() => requestEnd = true;
    public void Ends() => endCurrentCoroutine = true;

   
}
