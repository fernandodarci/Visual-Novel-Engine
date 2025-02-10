using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Story Units/Scene Unit")]
public class GSC_SceneUnit : ScriptableObject
{
    [SerializeField] private List<GSC_ScriptContainer> SceneContainers;

    public List<GSC_ContainerUnit> GetContainers()
    {
        if (SceneContainers == null || SceneContainers.Count == 0) return new List<GSC_ContainerUnit>();
        else
        {
            List<GSC_ContainerUnit> result = new List<GSC_ContainerUnit>();
            foreach (GSC_ScriptContainer container in SceneContainers)
            {
                result.Add(container.Compile());
            }
            return result.ToList();
        }
    }
}
