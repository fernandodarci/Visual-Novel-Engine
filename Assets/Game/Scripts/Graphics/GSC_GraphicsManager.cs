using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSC_GraphicsManager : GSC_Singleton<GSC_GraphicsManager>
{
    [SerializeField] private GSC_FullScreenLayer Prefab;
    [SerializeField] private RectTransform Background;
    
    private Dictionary<int, GSC_FullScreenLayer> Layers = new Dictionary<int, GSC_FullScreenLayer>();
    
    public void CreateLayer(int layer)
    {
        if (Layers.ContainsKey(layer)) return;

        GSC_FullScreenLayer newLayer = Instantiate(Prefab, Background);
        newLayer.gameObject.name = $"Layer {layer}";
        Layers.Add(layer, newLayer);
    }

    public IEnumerator ChangeScene(GSC_GraphicContainer grp, int layer, float fadeTime, GSC_ChangeEffectType effectType, Func<bool> paused, Func<bool> ends, Action onEnd)
    {
        if (!Layers.ContainsKey(layer))
        {
            CreateLayer(layer);
            Debug.Log("Changing scene");
            yield return Layers[layer].SetGraphic(grp, fadeTime, paused, ends);
        }
        else
        {
            switch (effectType)
            {
                case GSC_ChangeEffectType.None:
                    yield return Layers[layer].SetGraphic(grp, fadeTime, paused, ends);
                    break;
                case GSC_ChangeEffectType.CrossFade:
                    yield return Layers[layer].CrossFade(grp, fadeTime, paused, ends);
                    break;
                case GSC_ChangeEffectType.ReverseCrossFade:
                    yield return Layers[layer].ReverseCrossFade(grp, fadeTime, paused, ends);
                    break;
                    case GSC_ChangeEffectType.Emerge:
                    yield return Layers[layer].Emerge(grp, fadeTime, paused, ends);
                    break;
                case GSC_ChangeEffectType.Dissolve:
                    yield return Layers[layer].Dissolve(grp, fadeTime, paused, ends);
                    break;
            }
        }
    }
    
    public IEnumerator HideLayer(int layer, float fadeTime, Func<bool> paused, Func<bool> ends)
    {
        yield return null;
    }
}
