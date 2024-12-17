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

    public void Interrupt()
    {
        ContinueToBuild = false;
    }

    // Method to build text based on the provided dialogue line
    public IEnumerator BuildText(GSC_ContainerUnit line)
    {
        switch (Method)
        {
            case GSC_TextBuildMethod.Typewriter:
                yield return BuildTypewriter(line);
                break;

            case GSC_TextBuildMethod.Fade:
                yield return BuildFade(line);
                break;

            default:
                BuildInstant(line);
                break;
        }

        OnComplete();
    }

    private void BuildInstant(GSC_ContainerUnit line)
    {
        if (line.GetBoolean("Append") == false) Textline.text = string.Empty;
        Textline.text += line.GetString("Dialogue");
        OnComplete();

    }

    // Finalize the text-building process
    private void OnComplete()
    {
        Textline.color = Textline.color;
        Textline.maxVisibleCharacters = Textline.textInfo.characterCount;
        Textline.ForceMeshUpdate();
        IsBuilding = false;
    }

    private IEnumerator BuildTypewriter(GSC_ContainerUnit Line)
    {
        IsBuilding = true;
        ContinueToBuild = true;

        if (Line.GetBoolean("Append") == false)
        {
            Textline.text = string.Empty;
            Textline.ForceMeshUpdate();
        }

        Debug.Log("Typewriter started: " + Line.GetString("Dialogue"));

        Textline.color = Textline.color;
        string dialogueText = Line.GetString("Dialogue");
        Textline.text += dialogueText;
        Textline.ForceMeshUpdate();

        int totalCharacters = Textline.textInfo.characterCount;
        float duration = Line.GetFloat("Duration");
        float delay = duration / dialogueText.Length;
        Debug.Log("Total Characters: " + totalCharacters);
        Debug.Log("Delay: " + delay);

        Textline.maxVisibleCharacters = 0;

        for (int i = 0; i < dialogueText.Length && ContinueToBuild; i++)
        {
            Textline.maxVisibleCharacters = i + 1;
            Debug.Log("Visible Characters: " + Textline.maxVisibleCharacters);
            yield return new WaitForSeconds(delay);
        }

        OnComplete();
        Debug.Log("Typewriter completed.");
    }

    private IEnumerator BuildFade(GSC_ContainerUnit Line)
    {
        IsBuilding = true;
        ContinueToBuild = true;

        if (Line.GetBoolean("Append") == false)
        {
            Textline.text = string.Empty;
            Textline.ForceMeshUpdate();
        }

        PreTextLength = Textline.textInfo.characterCount;
        string Text = Line.GetString("Dialogue");
        Textline.text += Text;
        Textline.ForceMeshUpdate();

        var info = Textline.textInfo;
        Color32[] VertexColors = info.meshInfo[info.characterInfo[0].materialReferenceIndex].colors32;

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
        float totalDuration = Line.GetFloat("Duration");

        float[] Alphas = new float[info.characterCount];
        float FadeSpeed = 255f / totalDuration;

        Debug.Log("Total Characters: " + totalCharacters);
        Debug.Log("Fade Speed: " + FadeSpeed);

        while (ContinueToBuild)
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
                else ContinueToBuild = false;
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
}
    




