using UnityEngine;

public class GSC_GameManager : GSC_Singleton<GSC_GameManager>
{
    private static GSC_ScriptManager Scripts => GSC_ScriptManager.Instance;
    private static GSC_CommandManager Commands => GSC_CommandManager.Instance;

    [SerializeField] private GSC_Story _MainStory;
    [SerializeField] private GSC_ScreenInputController _ScreenInputController;
    [SerializeField] private GSC_LineCommandInterpreter _InterpretCommand;
    private bool _NormalMode = true;

    public GSC_Story Story => _MainStory;
    
    public void Start()
    {
       // Scripts.StartStory(_MainStory);
        _ScreenInputController.Initialize(OnScreenInputLeft, OnScreenInputRight);
    }

    public void OnScreenInputLeft()
    {
        if (_NormalMode) Commands.RequestEnd();
    }

    public void OnScreenInputRight()
    {
        if (_NormalMode) Commands.RequestEnd();
    }
}
