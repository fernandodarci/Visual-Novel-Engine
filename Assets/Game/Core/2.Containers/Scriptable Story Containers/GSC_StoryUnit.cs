using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Story Units/Story Unit")]
public class GSC_StoryUnit : ScriptableObject
{
    public List<GSC_ChapterUnit> Chapters;
}
