using System;
using UnityEngine;

public class GSC_ProviderManager : GSC_Singleton<GSC_ProviderManager>
{
    [SerializeField] private GSC_CharactersList Characters;
    [SerializeField] private GSC_ScriptableObjectProvider StoryFrames;
    [SerializeField] private GSC_ScriptableObjectProvider StoryAssets;
    
    public string GetCharacterName(string characterName, string nickName)
    {
        return Characters.GetName(characterName, nickName);
    }
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

