using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Story Units/Chapter List Unit")]
public class GSC_ChapterUnit : ScriptableObject
{
    public string ChapterTitle;
    public string ChapterSubtitle;
    public List<GSC_SceneUnit> SceneList;

    public int GetScenesOnChapter() => SceneList.Count;

    public List<GSC_ContainerUnit> GetScene(int sceneIndex)
        => SceneList[sceneIndex].GetContainers();
}
