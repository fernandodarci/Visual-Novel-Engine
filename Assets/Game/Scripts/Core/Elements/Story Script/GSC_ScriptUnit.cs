using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GSC_ScriptUnit
{
    public List<GSC_ScriptAction> Actions = new List<GSC_ScriptAction>();
    public List<string> JsonActions = new List<string>();

    // Generate JSON representing this script unit.
    public string ToJson()
    {
        JsonActions.Clear();
        if (Actions != null && Actions.Count > 0)
        {
            foreach (var action in Actions)
            {
                if (action == null)
                    continue;

                var compiled = action.Compile();
                var actionJson = compiled?.ToJson();
                if (!string.IsNullOrEmpty(actionJson))
                    JsonActions.Add(actionJson);
            }
        }
        return JsonUtility.ToJson(this);
    }

    // Populate this unit from a JSON string. Returns true if successful.
    public bool FromJson(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("Cannot deserialize script unit: JSON string is null or empty.");
            return false;
        }

        try
        {
            var tempUnit = JsonUtility.FromJson<GSC_ScriptUnit>(json);
            if (tempUnit == null || tempUnit.JsonActions == null)
            {
                Debug.LogError("Deserialized script unit is invalid.");
                return false;
            }

            Actions = new List<GSC_ScriptAction>();
            foreach (var actionJson in tempUnit.JsonActions)
            {
                if (string.IsNullOrEmpty(actionJson))
                    continue;

                var container = GSC_ContainerUnit.FromJson(actionJson);
                if (container == null)
                {
                    Debug.LogError("Failed to decompile action container.");
                    continue;
                }

                GSC_ScriptAction actionInstance = null;
                if (new GSC_ChangeBackgroundAction().Validate(container))
                {
                    var bgAction = new GSC_ChangeBackgroundAction();
                    bgAction.Decompile(container);
                    actionInstance = bgAction;
                }
                else if (new GSC_ShowDialogueBoxAction().Validate(container))
                {
                    var dialogueAction = new GSC_ShowDialogueBoxAction();
                    dialogueAction.Decompile(container);
                    actionInstance = dialogueAction;
                }
                // Add more action types here as needed.

                if (actionInstance != null)
                    Actions.Add(actionInstance);
                else
                    Debug.LogError("Unknown action type in script unit.");
            }
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception deserializing script unit JSON: {ex.Message}");
            return false;
        }
    }
}

