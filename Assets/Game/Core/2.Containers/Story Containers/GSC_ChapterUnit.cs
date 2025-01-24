using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Story Units/Chapter Unit")]
public class GSC_ChapterUnit : ScriptableObject
{
    public string ChapterTitle;
    public string ChapterSubtitle;
    public List<GSC_SceneUnit> Scenes;

    public GSC_ChapterUnit(string chapterTitle, string chapterSubtitle)
    {
        ChapterTitle = chapterTitle;
        ChapterSubtitle = chapterSubtitle;
        Scenes = new List<GSC_SceneUnit>();
    }
}