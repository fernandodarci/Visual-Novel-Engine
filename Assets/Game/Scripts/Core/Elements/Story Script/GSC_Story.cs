using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GSC_Story
{
    public List<GSC_ScriptBlock> Blocks = new List<GSC_ScriptBlock>();
    public List<string> JsonBlocks = new List<string>();

    // Generate JSON representing this story.
    public string ToJson()
    {
        JsonBlocks.Clear();
        if (Blocks != null && Blocks.Count > 0)
        {
            foreach (var block in Blocks)
            {
                if (block != null)
                {
                    var blockJson = block.ToJson();
                    if (!string.IsNullOrEmpty(blockJson))
                        JsonBlocks.Add(blockJson);
                }
            }
        }
        return JsonUtility.ToJson(this);
    }

    // Populate this story from a JSON string. Returns true if successful.
    public bool FromJson(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("Cannot deserialize story: JSON string is null or empty.");
            return false;
        }

        try
        {
            var tempStory = JsonUtility.FromJson<GSC_Story>(json);
            if (tempStory?.Blocks == null)
            {
                Debug.LogError("Deserialized story has no blocks.");
                return false;
            }

            Blocks = new List<GSC_ScriptBlock>();
            foreach (var block in tempStory.Blocks)
            {
                if (block == null)
                    continue;

                var newBlock = new GSC_ScriptBlock();
                if (newBlock.FromJson(block.ToJson()))
                    Blocks.Add(newBlock);
                else
                    Debug.LogError($"Failed to load block '{block.Name}'.");
            }
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception deserializing story JSON: {ex.Message}");
            return false;
        }
    }
}

