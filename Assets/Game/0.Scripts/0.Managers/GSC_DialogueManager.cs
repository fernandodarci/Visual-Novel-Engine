using System;
using System.Collections.Generic;
using UnityEngine;

public class GSC_DialogueManager : GSC_Singleton<GSC_DialogueManager>
{
    [Header("Dialogue Controllers")]
    [SerializeField] private GSC_DialoguePanelController DialogueController;
    [SerializeField] private GSC_InputPanelController InputPanelController;
    [SerializeField] private GSC_OptionsPanelController OptionsPanelController;
    [SerializeField] private GSC_ScreenMessageController PrefabController;
    private List<GSC_ScreenMessageController> Controllers;

    public GSC_ScreenMessageController CurrentController { get; private set; }

    public void GetScreenMessageController(string name, GSC_PositionSize posSize)
    {
        // Inicializa a lista se for nula
        if (Controllers == null)
            Controllers = new List<GSC_ScreenMessageController>();

        GSC_ScreenMessageController instance = null;

        if (!string.IsNullOrEmpty(name))
        {
            // Procura instância com o nome informado
            instance = Controllers.Find(x => x.name == name);
            if (instance == null)
            {
                // Se não existir, instancia e define o nome
                instance = Instantiate(PrefabController);
                instance.name = name;
                Controllers.Add(instance);
            }
        }
        else
        {
            // Se name for nulo ou vazio, utiliza a primeira instância da lista ou instancia uma nova
            if (Controllers.Count > 0)
                instance = Controllers[0];
            else
            {
                instance = Instantiate(PrefabController);
                instance.name = "FullScreenController";
                Controllers.Add(instance);
            }
        }

        RectTransform rt = instance.GetComponent<RectTransform>();
        if (rt != null)
        {
            // Reseta pivot para o centro
            rt.pivot = new Vector2(0.5f, 0.5f);

            if (posSize != null)
            {
                // Define anchors fixos no centro e ajusta posição e tamanho
                rt.anchorMin = new Vector2(0.5f, 0.5f);
                rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.anchoredPosition = posSize.Position;
                rt.sizeDelta = posSize.Size;
            }
            else
            {
                // Configura para tela cheia com offset de 100 em cada lado
                rt.anchorMin = new Vector2(0, 0);
                rt.anchorMax = new Vector2(1, 1);
                rt.offsetMin = new Vector2(100, 100);
                rt.offsetMax = new Vector2(-100, -100);
            }
        }
        CurrentController = instance;
    }

    public void HideAllMessages()
    {
        InputPanelController.Hide();
        DialogueController.Hide();
        OptionsPanelController.Hide();
        if (Controllers != null)
        {
            foreach (var controller in Controllers)
                controller.Hide();
        }
    }

    public GSC_DialoguePanelController GetDialogueController() => DialogueController;
    public GSC_OptionsPanelController GetOptionController() => OptionsPanelController;
    public GSC_InputPanelController GetInputController() => InputPanelController;
}

