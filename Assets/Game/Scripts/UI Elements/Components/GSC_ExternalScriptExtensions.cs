using System;
using System.Text.RegularExpressions;
using UnityEngine;

public static class GSC_ExternalScriptExtensions
{
    private const string FadeRegexPattern = @"fade\((-?\d+(\.\d+)?)\)";
    private const string WaitRegexPattern = @"wait\((-?\d+(\.\d+)?)\)";

    public static bool AddDialogue(this GSC_ScriptUnit scriptUnit, string line)
    {
        if (string.IsNullOrEmpty(line))
        {
            Debug.LogError("Input line is null or empty.");
            return false;
        }

        int start = -1;
        int end = -1;
        bool isQuoteEscaped = false;

        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] == '\\') isQuoteEscaped = !isQuoteEscaped;
            else if (line[i] == '"' && !isQuoteEscaped)
            {
                if (start == -1) start = i;
                else if (end == -1)
                {
                    end = i;
                    break;
                }
            }
            else isQuoteEscaped = false;
        }

        string beforeQuotes = string.Empty;
        string onQuotes = string.Empty;
        string afterQuotes = end < line.Length ? line[(end + 1)..].Trim() : string.Empty;

        //If end is less than or equal start, is not a dialogue line valid format.
        if (end <= start)
        {
            Debug.LogError("Invalid input string. Quotes not found or improperly placed.");
            return false;
        }

        if (start == -1 && end == -1)
        {
            // No quotes found, only character name
            beforeQuotes = line.Trim();
        }
        else if (start == 0)
        {
            // No character name, only dialogue
            onQuotes = line[(start + 1)..end].Trim();
        }
        else if (start > 0)
        {
            // Both character name and dialogue found
            beforeQuotes = line[..start].Trim();
            onQuotes = line[(start + 1)..end].Trim();
        }

        // For testing
        Debug.Log($"Before Quotes: {beforeQuotes}");
        Debug.Log($"On Quotes: {onQuotes}");
        Debug.Log($"After Quotes: {afterQuotes}");

        string characterName = string.Empty;
        string nameToShow = string.Empty;

        if (!string.IsNullOrEmpty(beforeQuotes))
        {
            // Process beforeQuotes to get the character name, and the nickname if it has one.
            string[] nameParts = beforeQuotes.Split(" [as] ", StringSplitOptions.RemoveEmptyEntries);

            characterName = nameParts[0];
            nameToShow = nameParts.Length == 2 ? nameParts[1] : string.Empty;
        }

        float fadeTime = 1f;
        float waitTime = 0f;
        bool hideAfterEnd = false;

        // Process afterQuotes to get some additional parameters
        if (!string.IsNullOrEmpty(afterQuotes))
        {
            string[] afterParts = afterQuotes.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            foreach (string part in afterParts)
            {
                if (Regex.IsMatch(part.Trim().ToLower(), FadeRegexPattern))
                {
                    Match match = Regex.Match(part, FadeRegexPattern);
                    if (match.Success)
                    {
                        fadeTime = float.Parse(match.Groups[1].Value);
                    }
                }
                else if (Regex.IsMatch(part.Trim().ToLower(), WaitRegexPattern))
                {
                    Match match = Regex.Match(part, WaitRegexPattern);
                    if (match.Success)
                    {
                        waitTime = float.Parse(match.Groups[1].Value);
                    }
                }
                else if (part.Trim().ToLower() == "hide")
                {
                    hideAfterEnd = true;
                }
            }
        }

        // Create a new GSC_ShowDialogueBoxAction to get the result
        GSC_ShowDialogueBoxAction action = new()
        {
            Character = characterName,
            NameToShow = nameToShow,
            Dialogue = onQuotes,
            FadeTime = fadeTime,
            WaitTime = waitTime,
            HideAfterEnd = hideAfterEnd
        };

        scriptUnit.AddOrCompleteDialogueAction(action);
        return true;
    }

    public static bool IsHead(this GSC_ShowDialogueBoxAction action)
        => action.Character != string.Empty && action.Dialogue == string.Empty;
    public static bool IsTail(this GSC_ShowDialogueBoxAction action)
        => action.Character == string.Empty && action.Dialogue != string.Empty;

    public static bool IsDefault(this GSC_ShowDialogueBoxAction action)
        => action.FadeTime == 1f && action.WaitTime == 0f && action.HideAfterEnd == false;

}

