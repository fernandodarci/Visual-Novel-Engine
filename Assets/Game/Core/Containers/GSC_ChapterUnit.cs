using System;
using System.Collections.Generic;

[Serializable]
public class GSC_ChapterUnit
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