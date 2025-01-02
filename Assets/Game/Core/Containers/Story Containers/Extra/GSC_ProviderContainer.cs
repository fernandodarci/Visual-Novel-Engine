using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(menuName = "Providers/Provider Container")]
public class GSC_ProviderContainer : ScriptableObject
{
    [SerializeField] private List<GSC_SpriteProvider> _sprites;
    [SerializeField] private List<GSC_TextureProvider> _textures;
    [SerializeField] private List<GSC_MaterialProvider> _materials;
    [SerializeField] private List<GSC_FontProvider> _fonts;
    [SerializeField] private List<GSC_GameObjectProvider> _gameObjects;
    [SerializeField] private List<GSC_ScriptableObjectProvider> _scriptableObjects;
    [SerializeField] private List<GSC_TextAssetProvider> _textAssets;
    [SerializeField] private List<GSC_AudioClipProvider> _audioClips;
    [SerializeField] private List<GSC_VideoClipProvider> _videoClips;

    // Method to get a Sprite from the Sprite provider
    public Sprite GetSprite(string group, string assetName)
    {
        GSC_SpriteProvider spriteProvider = _sprites.Find(x => x.name.ToLower().Trim() == group.ToLower().Trim());
        if (spriteProvider == null) return null;
        return spriteProvider.GetData(assetName);
    }

    // Method to get a Texture from the Texture provider
    public Texture GetTexture(string group, string assetName)
    {
        GSC_TextureProvider textureProvider = _textures.Find(x => x.name.ToLower().Trim() == group.ToLower().Trim());
        if (textureProvider == null) return null;
        return textureProvider.GetData(assetName);
    }

    // Method to get a Material from the Material provider
    public Material GetMaterial(string group, string assetName)
    {
        GSC_MaterialProvider materialProvider = _materials.Find(x => x.name.ToLower().Trim() == group.ToLower().Trim());
        if (materialProvider == null) return null;
        return materialProvider.GetData(assetName);
    }

    // Method to get a Font from the Font provider
    public TMP_FontAsset GetFont(string group, string assetName)
    {
        GSC_FontProvider fontProvider = _fonts.Find(x => x.name.ToLower().Trim() == group.ToLower().Trim());
        if (fontProvider == null) return null;
        return fontProvider.GetData(assetName);
    }

    // Method to get a GameObject from the GameObject provider
    public GameObject GetGameObject(string group, string assetName)
    {
        GSC_GameObjectProvider gameObjectProvider = _gameObjects.Find(x => x.name.ToLower().Trim() == group.ToLower().Trim());
        if (gameObjectProvider == null) return null;
        return gameObjectProvider.GetData(assetName);
    }

    // Method to get a ScriptableObject from the ScriptableObject provider
    public ScriptableObject GetScriptableObject(string group, string assetName)
    {
        GSC_ScriptableObjectProvider scriptableObjectProvider = _scriptableObjects.Find(x => x.name.ToLower().Trim() == group.ToLower().Trim());
        if (scriptableObjectProvider == null) return null;
        return scriptableObjectProvider.GetData(assetName);
    }

    // Method to get a TextAsset from the TextAsset provider
    public TextAsset GetTextAsset(string group, string assetName)
    {
        GSC_TextAssetProvider textAssetProvider = _textAssets.Find(x => x.name.ToLower().Trim() == group.ToLower().Trim());
        if (textAssetProvider == null) return null;
        return textAssetProvider.GetData(assetName);
    }

    // Method to get an AudioClip from the AudioClip provider
    public AudioClip GetAudioClip(string group, string assetName)
    {
        GSC_AudioClipProvider audioClipProvider = _audioClips.Find(x => x.name.ToLower().Trim() == group.ToLower().Trim());
        if (audioClipProvider == null) return null;
        return audioClipProvider.GetData(assetName);
    }

    // Method to get a VideoClip from the VideoClip provider
    public VideoClip GetVideoClip(string group, string assetName)
    {
        GSC_VideoClipProvider videoClipProvider = _videoClips.Find(x => x.name.ToLower().Trim() == group.ToLower().Trim());
        if (videoClipProvider == null) return null;
        return videoClipProvider.GetData(assetName);
    }

    // Method to get the list of asset names for a given group
    public string[] GetAssetList(string group)
    {
        // Search for the group in the SpriteProviders list
        GSC_SpriteProvider spriteProvider = _sprites.Find(x => x.name.ToLower().Trim() == group.ToLower().Trim());
        if (spriteProvider != null) return spriteProvider.GetNames;

        // Search for the group in the TextureProviders list
        GSC_TextureProvider textureProvider = _textures.Find(x => x.name.ToLower().Trim() == group.ToLower().Trim());
        if (textureProvider != null) return textureProvider.GetNames;

        // Search for the group in the MaterialProviders list
        GSC_MaterialProvider materialProvider = _materials.Find(x => x.name.ToLower().Trim() == group.ToLower().Trim());
        if (materialProvider != null) return materialProvider.GetNames;

        // Search for the group in the FontProviders list
        GSC_FontProvider fontProvider = _fonts.Find(x => x.name.ToLower().Trim() == group.ToLower().Trim());
        if (fontProvider != null) return fontProvider.GetNames;

        // Search for the group in the GameObjectProviders list
        GSC_GameObjectProvider gameObjectProvider = _gameObjects.Find(x => x.name.ToLower().Trim() == group.ToLower().Trim());
        if (gameObjectProvider != null) return gameObjectProvider.GetNames;

        // Search for the group in the ScriptableObjectProviders list
        GSC_ScriptableObjectProvider scriptableObjectProvider = _scriptableObjects.Find(x => x.name.ToLower().Trim() == group.ToLower().Trim());
        if (scriptableObjectProvider != null) return scriptableObjectProvider.GetNames;

        // Search for the group in the AudioClipProviders list
        GSC_AudioClipProvider audioClipProvider = _audioClips.Find(x => x.name.ToLower().Trim() == group.ToLower().Trim());
        if (audioClipProvider != null) return audioClipProvider.GetNames;

        // Search for the group in the VideoClipProviders list
        GSC_VideoClipProvider videoClipProvider = _videoClips.Find(x => x.name.ToLower().Trim() == group.ToLower().Trim());
        if (videoClipProvider != null) return videoClipProvider.GetNames;

        // If no group is found in any list, return null
        return null;
    }
}
