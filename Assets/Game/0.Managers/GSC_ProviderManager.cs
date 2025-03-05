using System;
using UnityEngine;

public class GSC_ProviderManager : GSC_Singleton<GSC_ProviderManager>
{
    [SerializeField] private GSC_CharactersList Characters;
    [SerializeField] private GSC_ScriptableObjectProvider StoryFrames;
    [SerializeField] private GSC_ScriptableObjectProvider StoryAssets;
    
    public string GetName(string Name, string As) => Characters.GetName(Name, As);
    
    public string[] GetStoryFrameGroups() => StoryFrames.GetNames();
    public string[] GetFramesFromGroup(string group)
    {
        ScriptableObject result = StoryFrames.LoadAsset(group);
        if(result != null && result is GSC_SpriteProvider @spr)
        {
            return spr.GetNames();
        }
        return null;
    }
   
    public Sprite GetStoryFrame(string group, string frame)
    {
        ScriptableObject result = StoryFrames.LoadAsset(group);
        if (result != null && result is GSC_SpriteProvider @spr)
        {
            return spr.LoadAsset(frame);
        }
        return null;
    }

    
}

