using System;
using System.Collections.Generic;

[Serializable]
public class GSC_StoryUnit
{
    public List<GSC_ChapterUnit> Chapters;

    public GSC_StoryUnit()
    {
        Chapters = new List<GSC_ChapterUnit>();
    }
}
