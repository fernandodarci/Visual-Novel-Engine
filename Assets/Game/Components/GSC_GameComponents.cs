
using UnityEngine;

public class GSC_GameComponents : MonoBehaviour
{
    [SerializeField] private GSC_ScreenInput ScreenInputController;
    [SerializeField] private GSC_DialogueController DialogueController;
    [SerializeField] private GSC_InputPanelController InputPanelController;
    [SerializeField] private GSC_ChoicePanelController ChoicePanelController;
    [SerializeField] private GSC_ImageLayerController BackgroundController;
    [SerializeField] private GSC_ImageLayerController CharactersController;
    [SerializeField] private GSC_ImageLayerController ForegroundController;
    
    public GSC_ScreenInput ScreenInput => ScreenInputController;
    public GSC_DialogueController Dialogue => DialogueController;
    public GSC_InputPanelController InputPanel => InputPanelController;
    public GSC_ChoicePanelController ChoicePanel => ChoicePanelController;
    public GSC_ImageLayerController Background => BackgroundController;
    public GSC_ImageLayerController Characters => CharactersController;
    public GSC_ImageLayerController Foreground => ForegroundController;
    
}