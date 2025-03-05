using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public static class GSC_Constants
{
    public static string SavePath => $"{Environment.CurrentDirectory}\\Assets\\Story";
    public static string GameSavePath => $"{Application.persistentDataPath}\\Saves";

    public const string Disclaimer1 = "This is a work of fiction. Any resemblance to real people, places, or events is purely coincidental.";
    public const string Disclaimer2 = "This story explores mature themes and contains content that may be disturbing or otherwise unsettling for some viewers. Viewer discretion is strongly advised.";
    public const string Disclaimer3 = "All characters are digital representations of people, which implies that they are all over the age of consent and that all depicted acts are consensual.";
    public const string Disclaimer4 = "By continuing, you confirm that you fully understand the above and that you are legally responsible for any ethical, legal, or personal consequences of viewing this content.";

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
