using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GSC_MainMenuController : GSC_CanvasGroupController
{
    [SerializeField] private TextMeshProUGUI Title;
    [SerializeField] private Button StartButton;
    [SerializeField] private Button LoadButton;
    [SerializeField] private Button GalleryButton;
    [SerializeField] private Button OptionsButton;
    [SerializeField] private Button ExitButton;
    [SerializeField] private GSC_StoryUnit PreMain;
    [SerializeField] private GSC_StoryUnit PostMain;
    public bool IsClicked { get; private set; }

    public static GSC_ContainerUnit MainMenu
    {
        get
        {
            GSC_ContainerUnit mainMenu = new GSC_ContainerUnit("ShowMainMenu");
            mainMenu.Set("Fade", 2f);
            return mainMenu;
        }
    }

    private void Start()
    {
        IsClicked = false;
        StartButton.onClick.AddListener(OnStart);
        LoadButton.onClick.AddListener(OnLoad);
        GalleryButton.onClick.AddListener(OnGallery);
        OptionsButton.onClick.AddListener(OnOptions);
        ExitButton.onClick.AddListener(OnExit);
    }

    public void RunIntro()
    {
        GSC_ScriptManager.Instance.RunStory(PreMain, 0, 0);
    }

    private void OnStart()
    {
        IsClicked = true;
        GSC_ScriptManager.Instance.RunStory(PostMain, 0, 0);
    }

    private void OnLoad() { }
    private void OnGallery() { }
    private void OnOptions() { }
    private void OnExit() { }
    public void ResetClick() => IsClicked = false;

    public void LoadStoryFrom(int chapter = 0, int scene = 0)
    {
        GSC_ScriptManager.Instance.RunStory(PostMain, chapter, scene);
    }
}
