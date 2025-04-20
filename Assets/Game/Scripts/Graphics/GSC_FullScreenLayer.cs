using System;
using System.Collections;
using UnityEngine;

public class GSC_FullScreenLayer : MonoBehaviour
{
    public GSC_GraphicElement Base;
    public GSC_GraphicElement Overlay;

    public IEnumerator Dissolve(GSC_GraphicContainer grp, float duration, Func<bool> pause, Func<bool> ends)
    {
        Overlay.GetGraphicElement(Base);
        Overlay.Show();
        Base.GetGraphicFromContainer(grp);
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            if (ends()) break;
            if (!pause())
            {
                elapsedTime += Time.deltaTime;
                Overlay.SetVisibility(Mathf.Lerp(1f, 0f, elapsedTime / duration));
            }
            yield return null;
        }
        Overlay.Hide();
    }

    public IEnumerator Emerge(GSC_GraphicContainer grp, float duration, Func<bool> pause, Func<bool> ends)
    {
        Overlay.GetGraphicFromContainer(grp);

        float elapsedTime = 0;

        while(elapsedTime < duration)
        {
            if (ends()) break;
            if (!pause())
            {
                elapsedTime += Time.deltaTime;
                Overlay.SetVisibility(Mathf.Lerp(0f, 1f, elapsedTime / duration));
            }
            yield return null;
        }
        Base.GetGraphicFromContainer(grp);
        Overlay.Hide();
    }

    public IEnumerator CrossFade(GSC_GraphicContainer grp, float duration, Func<bool> pause, Func<bool> ends)
    {
        Overlay.GetGraphicFromContainer(grp);
        Overlay.Hide();
        
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            if (ends()) break;
            if (!pause())
            {
                elapsedTime += Time.deltaTime;
                Overlay.SetVisibility(Mathf.Lerp(0f, 1f, elapsedTime / duration));
                Base.SetVisibility(Mathf.Lerp(1f, 0f, elapsedTime / duration));
            }
            yield return null;
        }
        
        Base.GetGraphicFromContainer(grp);
        Base.Show();
        Overlay.Hide();
    }

    public IEnumerator ReverseCrossFade(GSC_GraphicContainer grp, float duration, Func<bool> pause, Func<bool> ends)
    {
        Overlay.GetGraphicElement(Base);
        Overlay.Show();
        Base.GetGraphicFromContainer(grp);
        Base.Hide();

        float elapsedTime = 0;
        
        while (elapsedTime < duration)
        {
            if (ends()) break;
            if (!pause())
            {
                elapsedTime += Time.deltaTime;
                Overlay.SetVisibility(Mathf.Lerp(0f, 1f, elapsedTime / duration));
                Base.SetVisibility(Mathf.Lerp(1f, 0f, elapsedTime / duration));
            }
            yield return null;
        }
        
        Base.Show();
        Overlay.Hide();
    }

    public IEnumerator SetGraphic(GSC_GraphicContainer grp, float fadeTime, Func<bool> paused, Func<bool> ends)
    {
        Base.Hide();
        Overlay.Hide();
        Base.GetGraphicFromContainer(grp);
        yield return Base.FadeIn(fadeTime, false, paused, ends);
    }
}
