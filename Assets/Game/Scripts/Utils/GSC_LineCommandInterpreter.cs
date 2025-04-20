using TMPro;
using UnityEngine;

public class GSC_LineCommandInterpreter : GSC_ElementView
{
    public TMP_InputField InputField;

    private void Start()
    {
        InputField.onEndEdit.AddListener(OnInput);
        InputField.onSubmit.AddListener(OnInput);
    }

    private void OnInput(string arg0)
    {
        Debug.Log("Input received: " + arg0);
        Decode();
    }

    public void Decode()
    {
        string command = InputField.text;
        InputField.text = string.Empty; // Limpa o campo de entrada após receber o comando
        if (command.IsNullOrEmpty()) return;
  
        if(GSC_CommandInterpreter.Instance.TryToDecodeCommand(command))
        {
            Debug.Log("Command decoded successfully.");
        }
        else
        {
             Debug.LogError($"Failed to decode command: {command} ");
        }
    }


}
