using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GSC_CommandState
{
    Idle, Preparing, Paused, Executing
}

public class GSC_CommandManager : GSC_Singleton<GSC_CommandManager>
{
    public delegate Func<Func<bool>, Func<bool>, Action, IEnumerator> GSC_CommandAction();
    private Queue<GSC_CommandAction> commandQueue = new Queue<GSC_CommandAction>();
    private GSC_CommandAction CurrentAction;
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
                CurrentAction = commandQueue.Dequeue();
                if (CurrentAction != null)
                {
                    CurrentState = GSC_CommandState.Executing;
                    yield return WrapCoroutine(CurrentAction);
                    CurrentAction = null;
                    Debug.Log("Action End");
                    if (commandQueue.Count == 0) CurrentState = GSC_CommandState.Idle;
                    else Debug.Log($"Remaining {commandQueue.Count} actions to end");
                }
            }
            yield return null;
        }
    
    }


    public void EnqueueCommand(GSC_CommandAction commandUnit)
    {
        commandQueue.Enqueue(commandUnit);
    }

    public void EnqueueAction(GSC_ScriptAction action)
    {
        commandQueue.Enqueue(action.GetAction());
    }

    private IEnumerator WrapCoroutine(GSC_CommandAction handler)
    {
        Func<Func<bool>, Func<bool>, Action, IEnumerator> executor = handler();
        if (executor != null)
        {
            endCurrentCoroutine = false;
            Debug.Log("Start handler");
            while (!endCurrentCoroutine)
            {
                yield return executor(IsPaused, IsEndRequested, Ends);
                if (!endCurrentCoroutine) Ends();
            }
        }
        else
        {
            Debug.Log("Empty handler");
            Ends();
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
    public void PrepareToAction()
    {
        CurrentState = GSC_CommandState.Preparing;
    }


}
