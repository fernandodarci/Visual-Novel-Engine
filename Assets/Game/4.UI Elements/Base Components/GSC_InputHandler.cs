using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

public class GSC_InputHandler : GSC_CanvasGroupController
{
    [SerializeField] private TMP_InputField InputField;
    
    public GSC_Parameter InputData { get; private set; }
    
    public bool IsInput { get; private set; }
   
    public void InitializeInput(GSC_Parameter input)
    {
        InputData = input;
        InputField.onSubmit.AddListener(UpdateOutput);
        InputField.onEndEdit.AddListener(UpdateOutput);
        InputField.textComponent.text = string.Empty;
        IsInput = false;
        UpdateInputMode();
        InputField.ActivateInputField();
    }

    private void UpdateOutput(string val)
    {
        if (!string.IsNullOrEmpty(val))
        {
            switch (InputData)
            {
                case GSC_IntegerParameter i:
                    int ival = int.Parse(val);
                    InputData = i.OnValid(GSC_IntegerParameter.WithValue(i.ParameterName,ival));
                    break;
                case GSC_FloatParameter f:
                    float fval = float.Parse(val, CultureInfo.InvariantCulture);
                    InputData = f.OnValid(GSC_FloatParameter.WithValue(f.ParameterName,fval));
                    break;
                case GSC_StringParameter s:
                    GSC_StringParameter sval = new GSC_StringParameter(s.ParameterName, val, false);
                    InputData = s.OnValid(sval);
                    break;
            };
            IsInput = true;
        }
    }

    public IEnumerator WaitingForInput()
    {
        while(IsInput == false)
        {
            yield return null;
        }
    }

    public void UpdateInputMode()
    {
        switch (InputData.ParameterType)
        {
            case GSC_ParameterType.Integer:
                InputField.keyboardType = TouchScreenKeyboardType.NumberPad;
                InputField.contentType = TMP_InputField.ContentType.IntegerNumber;
                InputField.lineType = TMP_InputField.LineType.MultiLineNewline;
                InputField.textComponent.alignment = TextAlignmentOptions.Right;
                InputData = new GSC_FloatParameter(InputData.ParameterName);
                break;
            case GSC_ParameterType.Float:
                InputField.keyboardType = TouchScreenKeyboardType.DecimalPad;
                InputField.contentType = TMP_InputField.ContentType.DecimalNumber;
                InputField.textComponent.alignment = TextAlignmentOptions.Right;
                InputData = new GSC_FloatParameter(InputData.ParameterName);
                break;
            default:
                InputField.keyboardType = TouchScreenKeyboardType.Default;
                InputField.contentType = TMP_InputField.ContentType.Standard;
                InputField.textComponent.textWrappingMode = TextWrappingModes.PreserveWhitespace;
                InputField.textComponent.overflowMode = TextOverflowModes.Truncate;
                InputField.textComponent.alignment = TextAlignmentOptions.TopLeft;
                InputField.textComponent.verticalMapping = TextureMappingOptions.Line;
                InputData = new GSC_StringParameter(InputData.ParameterName);
                break;
        }
    }
    
    public void Fit(RectTransform rectTransform)
    {
        if (rectTransform != null)
        {
            RectTransform inputFieldRect = InputField.GetComponent<RectTransform>();

            inputFieldRect.anchorMin = rectTransform.anchorMin;
            inputFieldRect.anchorMax = rectTransform.anchorMax;
            inputFieldRect.pivot = rectTransform.pivot;
            inputFieldRect.offsetMin = rectTransform.offsetMin;
            inputFieldRect.offsetMax = rectTransform.offsetMax;
            
        }
    }
}


