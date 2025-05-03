using UnityEngine;

public class GSC_FileCommandInterpreter : MonoBehaviour
{
    [SerializeField] private TextAsset _File;

    private void Start()
    {
        string[] commands = GSC_Constants.ReadTextAsset(_File, true);
        if (!commands.IsNullOrEmpty())
        {
            foreach (string command in commands)
            {
                if (command.IsNullOrEmpty()) continue;
                if (GSC_CommandInterpreter.Instance.TryToDecodeCommand(command))
                {
                    Debug.Log("Command decoded successfully.");
                }
                else
                {
                    Debug.LogError($"Failed to decode command: {command} ");
                }
            }
        }
    }
}
