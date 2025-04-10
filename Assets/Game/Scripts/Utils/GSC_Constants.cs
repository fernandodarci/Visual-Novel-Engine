using System;
using System.Collections;
using UnityEngine;

public static class GSC_Constants
{
    public static string GameSavePath => $"{Application.persistentDataPath}\\Saves";

    public static IEnumerator WaitForComplete(Func<bool> complete)
    {
        while (!complete())
        {
            yield return null;
        }
    }

    public static IEnumerator WaitForSeconds(float seconds, Func<bool> pause, Func<bool> ends)
    {
        float elapsedTime = 0f;
        while (elapsedTime < seconds)
        {
            if (ends()) yield break;
            if (!pause())
            {
                elapsedTime += Time.deltaTime;
            }

            yield return null;
        }
    }
}
