using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GSC_GalleryMenuController : GSC_CanvasGroupController
{
    [SerializeField] private TextMeshProUGUI Title;
    [SerializeField] private Button Characters;
    [SerializeField] private Button Images;
    [SerializeField] private Button Videos;
    [SerializeField] private Button DataFiles;
    [SerializeField] private Button Back;
    public bool IsClicked { get; private set; }

    private void Start()
    {
        IsClicked = false;
        Characters.onClick.AddListener(OnCharacters);
        Images.onClick.AddListener(OnImages);
        Videos.onClick.AddListener(OnVideos);
        DataFiles.onClick.AddListener(OnDataFiles);
        Back.onClick.AddListener(OnBack);
    }

    private void OnCharacters() { }
    private void OnImages() { }
    private void OnVideos() { }
    private void OnDataFiles() { }
    private void OnBack() { }


}
