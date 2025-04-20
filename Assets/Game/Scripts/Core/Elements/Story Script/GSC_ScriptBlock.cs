using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GSC_ScriptBlock
{
    public string Name;
    public string SequenceToFollow;
    public List<GSC_ScriptUnit> Units = new List<GSC_ScriptUnit>();
    public List<string> JsonUnits = new List<string>();

    // Generate JSON representing this script block.
    public string ToJson()
    {
        JsonUnits.Clear();
        if (Units != null && Units.Count > 0)
        {
            foreach (var unit in Units)
            {
                if (unit != null)
                {
                    var unitJson = unit.ToJson();
                    if (!string.IsNullOrEmpty(unitJson))
                        JsonUnits.Add(unitJson);
                }
            }
        }
        return JsonUtility.ToJson(this);
    }

    // Populate this block from a JSON string. Returns true if successful.
    public bool FromJson(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("Cannot deserialize block: JSON string is null or empty.");
            return false;
        }

        try
        {
            var tempBlock = JsonUtility.FromJson<GSC_ScriptBlock>(json);
            if (tempBlock == null)
            {
                Debug.LogError("Deserialized block is null.");
                return false;
            }

            Name = tempBlock.Name;
            SequenceToFollow = tempBlock.SequenceToFollow;

            Units = new List<GSC_ScriptUnit>();
            if (tempBlock.JsonUnits != null && tempBlock.JsonUnits.Count > 0)
            {
                foreach (var unitJson in tempBlock.JsonUnits)
                {
                    if (string.IsNullOrEmpty(unitJson))
                        continue;

                    var scriptUnit = new GSC_ScriptUnit();
                    if (scriptUnit.FromJson(unitJson))
                        Units.Add(scriptUnit);
                    else
                        Debug.LogError("Failed to deserialize a script unit.");
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception deserializing block JSON: {ex.Message}");
            return false;
        }
    }
}

