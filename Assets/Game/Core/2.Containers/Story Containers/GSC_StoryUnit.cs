using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Story Units/Story Unit")]
public class GSC_StoryUnit : ScriptableObject
{
    public string ID;
    public List<GSC_ChapterUnit> Chapters;

    public GSC_StoryUnit()
    {
        Chapters = new List<GSC_ChapterUnit>();
    }
}
