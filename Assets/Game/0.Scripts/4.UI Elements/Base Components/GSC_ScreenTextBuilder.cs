using System;
using System.Collections;
using TMPro;
using UnityEngine;

public enum GSC_TextBuildMethod { Instant, Typewriter, Fade }

public class GSC_ScreenTextBuilder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Textline;
    public GSC_TextBuildMethod Method;
    
    public bool IsBuilding { get; private set; } = false;
    private bool ContinueToBuild;
    private int PreTextLength;
    
    private byte alphaThreshold => 15;

    public bool IsRunning { get; private set; }
   
    // Method to build text based on the provided dialogue line
    public IEnumerator BuildText(string dialogue, float duration, bool append, 
        Func<bool> pause, Func<bool> ends)
    {
        if(duration <= 0.01f)
        {
            BuildInstant(dialogue, append);
            yield break;
        }

        switch (Method)
        {
            case GSC_TextBuildMethod.Typewriter:
                yield return BuildTypewriter(dialogue, duration, append, pause, ends);
                break;

            case GSC_TextBuildMethod.Fade:
                yield return BuildFade(dialogue, duration, append, pause, ends);
                break;

            default:
                BuildInstant(dialogue, append);
                break;
        }

        OnComplete();
    }

    private void BuildInstant(string dialogue, bool append)
    {
        if (append == false) Textline.text = string.Empty;
        Textline.text += dialogue;
        OnComplete();
    }

    // Finalize the text-building process
    private void OnComplete()
    {
        Textline.color = Textline.color;
        Textline.maxVisibleCharacters = int.MaxValue;
        Textline.ForceMeshUpdate();
        IsBuilding = false;
    }

    private IEnumerator BuildTypewriter(string dialogue, float duration, bool append,
        Func<bool> pause, Func<bool> ends)
    {
        IsBuilding = true;
        ContinueToBuild = true;

        if (append == false)
        {
            Textline.text = string.Empty;
            Textline.ForceMeshUpdate();
        }

        Debug.Log("Typewriter started: " + dialogue);

        Textline.color = Textline.color;
        Textline.text += dialogue;
        Textline.ForceMeshUpdate();

        int totalCharacters = Textline.textInfo.characterCount;
        float delay = duration / dialogue.Length;
        Debug.Log("Total Characters: " + totalCharacters);
        Debug.Log("Delay: " + delay);

        Textline.maxVisibleCharacters = 0;

        for (int i = 0; i < dialogue.Length; i++)
        {
            if (ends()) break;
            if (!pause())
            {
                Textline.maxVisibleCharacters = i + 1;
                Debug.Log("Visible Characters: " + Textline.maxVisibleCharacters);
                yield return new WaitForSeconds(delay);
            }
            else yield return null;
        }

        OnComplete();
        Debug.Log("Typewriter completed.");
    }

    private IEnumerator BuildFade(string dialogue, float duration, bool append, 
        Func<bool> pause, Func<bool> ends)
    {
        IsBuilding = true;
        
        if (append == false)
        {
            Textline.text = string.Empty;
            Textline.ForceMeshUpdate();
        }

        PreTextLength = Textline.textInfo.characterCount;
        Textline.text += dialogue;
        Textline.ForceMeshUpdate();

        var info = Textline.textInfo;
        Color32[] VertexColors = info.meshInfo[info.characterInfo[0].materialReferenceIndex].colors32;
        
        if (ends()) yield break;
        
        for (int i = PreTextLength; i < info.characterCount; i++)
        {
            var charInfo = info.characterInfo[i];
            if (!charInfo.isVisible) continue;
            for (int v = 0; v < 4; v++)
                VertexColors[charInfo.vertexIndex + v].a = 0;
        }

        Textline.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        int minRange = PreTextLength;
        int maxRange = minRange + 1;
        float totalCharacters = info.characterCount - PreTextLength;
        

        float[] Alphas = new float[info.characterCount];
        float FadeSpeed = 255f / duration;

        Debug.Log("Total Characters: " + totalCharacters);
        Debug.Log("Fade Speed: " + FadeSpeed);

        while (!ends())
        {
            if (!pause())
            {
                for (int i = minRange; i < maxRange; i++)
                {
                    TMP_CharacterInfo characterInfo = info.characterInfo[i];
                    if (!characterInfo.isVisible) continue;

                    int vertexIndex = characterInfo.vertexIndex;
                    Alphas[i] = Mathf.MoveTowards(Alphas[i], 255, FadeSpeed * Time.deltaTime);

                    for (int v = 0; v < 4; v++)
                        VertexColors[vertexIndex + v].a = (byte)Alphas[i];

                    if (Alphas[i] >= 255) minRange++;
                }

                Textline.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);


                if (Alphas[maxRange - 1] > alphaThreshold || !info.characterInfo[maxRange - 1].isVisible)
                {
                    if (maxRange < info.characterCount) maxRange++;
                    else if (minRange == maxRange && Alphas[maxRange - 1] >= 255f) break;
                }
            }
            yield return null;
        }

        OnComplete();
        Debug.Log("Fade completed.");
    }


    public void Clear()
    {
        Textline.text = string.Empty;
    }

    public void CompleteDialogue() => ContinueToBuild = false;
    
}
    




