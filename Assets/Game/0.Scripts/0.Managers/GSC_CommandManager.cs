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
    public delegate Func<GSC_ContainerUnit, Func<bool>, Func<bool>, Action, IEnumerator> CommandAction();

    private Queue<GSC_ContainerUnit> commandQueue = new Queue<GSC_ContainerUnit>();
    private CommandAction CurrentAction;
    private GSC_CommandState CurrentState;

    public bool IsIdle() => CurrentState == GSC_CommandState.Idle;
    private bool IsPaused() => CurrentState == GSC_CommandState.Paused;
    private bool endCurrentCoroutine;
    private bool requestEnd;

    private void Start()
    {
        StartCoroutine(RunOnce());
    }

    private bool IsEndRequested()
    {
        if (requestEnd)
        {
            requestEnd = false;
            return true;
        }
        return false;
    }


    private IEnumerator RunOnce()
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
                    CurrentAction = GetAction(unit);
                    if (CurrentAction != null)
                    {
                        CurrentState = GSC_CommandState.Executing;
                        yield return WrapCoroutine(CurrentAction, unit);
                        CurrentAction = null;
                        Debug.Log("Action End");
                        if (commandQueue.Count == 0) CurrentState = GSC_CommandState.Idle;
                        else Debug.Log($"Remaining {commandQueue.Count} actions to end");
                    }
                }
            }
            yield return null;
        }
    }

    private CommandAction GetAction(GSC_ContainerUnit unit)
    {
        CommandAction action = null;
        if(GSC_DialogueManager.Instance.TryGetAction(unit, out action)) return action;
        if(GSC_GraphicsManager.Instance.TryGetAction(unit,out action)) return action;
        return action;
    }

    public void EnqueueCommand(GSC_ContainerUnit commandUnit)
    {
        commandQueue.Enqueue(commandUnit);
    }

    private IEnumerator WrapCoroutine(CommandAction handler, GSC_ContainerUnit unit)
    {
        Func<GSC_ContainerUnit, Func<bool>, Func<bool>, Action, IEnumerator> executor = handler();
        if (executor != null)
        {
            endCurrentCoroutine = false;
            Debug.Log("Start handler");
            while (!endCurrentCoroutine)
            {
                yield return executor(unit, IsPaused, IsEndRequested, Ends);
            }
        }
    }

    public void Ends() => endCurrentCoroutine = true;
    public void Pause() => CurrentState = GSC_CommandState.Paused;
    public void Resume()
    {
        CurrentState = CurrentAction != null ? GSC_CommandState.Executing : GSC_CommandState.Idle;
    }

    public void Wipe()
    {
        commandQueue.Clear();
        endCurrentCoroutine = true;
    }

    public void RequestEnd() => requestEnd = true;
}

