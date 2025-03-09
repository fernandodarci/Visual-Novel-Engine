using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSC_ButtonBar : GSC_CanvasGroupController
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private GSC_ButtonCaption ButtonPrefab;
    public string OptionChoosed { get; private set; }
    public bool MadeChoice { get; internal set; }

    private List<GSC_ButtonCaption> ButtonList = new();

    public void SetButtons(params string[] parameterValues)
    {
        if (parameterValues.Length > 0)
        {
            for (int i = 0; i < parameterValues.Length; i++)
            {
                GSC_ButtonCaption newButton = Instantiate(ButtonPrefab,rectTransform);
                newButton.transform.localScale = Vector3.one;
                newButton.transform.localPosition = Vector3.zero;
                newButton.Initialize(parameterValues[i]);
                newButton.OnBtnClick = Choosed;
                ButtonList.Add(newButton);
            }
        }
    }

    private void Choosed(string value)
    {
        OptionChoosed = value;
        MadeChoice = true;
    }

    public IEnumerator WaitChooseOptions()
    {
        OptionChoosed = string.Empty;
        MadeChoice = false;
        while (MadeChoice == false)
        {
            yield return null;
        }
        Debug.Log($"Choose made {OptionChoosed}");
        yield return null;
    }

    public void ClearButtons()
    {
        while (ButtonList.Count > 0)
        {
            var btn = ButtonList[0];
            ButtonList.RemoveAt(0);
            Destroy(btn.gameObject);
        }
        ButtonList.Clear();
    }
}
