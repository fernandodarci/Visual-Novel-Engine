using UnityEngine;
using UnityEngine.UI;

public class GSC_InGameMenuController : GSC_CanvasGroupController
{
    [SerializeField] private Button Auto;
    [SerializeField] private Button Navigate;

    private void Awake()
    {
        Auto.onClick.AddListener(ToggleAuto);
        Navigate.onClick.AddListener(OnNavigate);
    }

    private void ToggleAuto()
    {
        /*
         TO DO: Make Auto button a Toggle that changes color if On/Off.
         Report AutoMode change to GameManager. A AutoMode game is a game
         where dialogues wait 1s instead of player input to pass.
         */
    }

    private void OnNavigate()
    {
        GSC_GameManager.Instance.ActivateNavigationMode();
    }

}
