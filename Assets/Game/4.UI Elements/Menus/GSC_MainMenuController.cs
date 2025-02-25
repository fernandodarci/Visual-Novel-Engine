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

    public bool IsClicked { get; private set; }

    private void Start()
    {
        IsClicked = false;
        StartButton.onClick.AddListener(OnStart);
        LoadButton.onClick.AddListener(OnLoad);
        GalleryButton.onClick.AddListener(OnGallery);
        OptionsButton.onClick.AddListener(OnOptions);
        ExitButton.onClick.AddListener(OnExit);
    }

    private void OnStart() 
    {
        Debug.Log("Start is clicked");
        GSC_GameManager.Instance.StartStory();
    }
    private void OnLoad() { }
    private void OnGallery() { }
    private void OnOptions() { }
    private void OnExit() { }
    public void ResetClick() => IsClicked = false;

}
