using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GSC_NavigationPointsController : GSC_CanvasGroupController
{
    public Button ButtonPrefab;
    public List<Button> ClickableAreas = new List<Button>();
    public bool IsClicked { get; private set; }

    public void InitializeClickableArea(List<GSC_ClickableArea> targets)
    {
        if (targets.IsNullOrEmpty()) return;
        
        if (ClickableAreas.Count > 0)
        {
            foreach (Button button in ClickableAreas)
            {
                Destroy(button.gameObject);
            }
            ClickableAreas.Clear();
        }

        foreach (var target in targets)
        {
            Button area = Instantiate(ButtonPrefab, transform);
            RectTransform rt = area.GetComponent<RectTransform>();
            if (rt != null)
            {
                // Define anchors para posicionamento absoluto
                rt.anchorMin = new Vector2(0, 0);
                rt.anchorMax = new Vector2(0, 0);
                rt.anchoredPosition = target.Position;
                rt.sizeDelta = target.Size;
            }

            // Configura o onClick para marcar o clique e atualizar o valor
            area.onClick.AddListener(() =>
            {
                IsClicked = true;
                GSC_DataManager.Instance.AddOrChangeValue("NavPoint", target.ValueIfClicked);
            });

            ClickableAreas.Add(area);
        }
    }

    public IEnumerator WaitForClick(Func<bool> paused, Func<bool> ends)
    {
        IsClicked = false;
        while (!IsClicked)
        {
            if(paused() && IsClicked) IsClicked = false;
            if(ends()) break;
            yield return null;
        }
    }
}
