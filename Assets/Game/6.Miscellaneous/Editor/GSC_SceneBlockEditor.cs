//using UnityEditor;
//using UnityEditorInternal;
//using UnityEngine;
//using System.Collections.Generic;
//using System.IO;
//using System;

//[CustomEditor(typeof(GSC_SceneBlockUnit))]
//public class GSC_SceneBlockEditor : Editor
//{
//    private ReorderableList sceneList;
//    private Dictionary<string, ReorderableList> actionsLists = new Dictionary<string, ReorderableList>();
//    private Dictionary<string, bool> foldouts = new Dictionary<string, bool>();
//    private SerializedProperty scenesProp;

    

//    private void OnEnable()
//    {
//        scenesProp = serializedObject.FindProperty("Scenes");
//        sceneList = new ReorderableList(serializedObject, scenesProp, true, true, true, true);

//        sceneList.drawHeaderCallback = (Rect rect) =>
//        {
//            EditorGUI.LabelField(rect, "Scenes");
//        };

//        sceneList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
//        {
//            SerializedProperty sceneProp = scenesProp.GetArrayElementAtIndex(index);
//            rect.y += 2;

//            // Define scene name based on its position
//            string sceneName = "Scene " + (index + 1);
//            // Força o nome da cena
//            sceneProp.FindPropertyRelative("Name").stringValue = sceneName;
//            EditorGUI.LabelField(
//                new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
//                sceneName);

//            // Foldout para exibir a lista de Actions
//            string key = sceneProp.propertyPath;
//            if (!foldouts.ContainsKey(key))
//                foldouts[key] = false;
//            Rect foldoutRect = new Rect(rect.x + 20, rect.y + EditorGUIUtility.singleLineHeight + 2, rect.width, EditorGUIUtility.singleLineHeight);
//            foldouts[key] = EditorGUI.Foldout(foldoutRect, foldouts[key], "Actions", true);

//            if (foldouts[key])
//            {
//                SerializedProperty actionsProp = sceneProp.FindPropertyRelative("Actions");

//                // Cria a ReorderableList interna para as Actions se ainda não existir
//                if (!actionsLists.TryGetValue(key, out ReorderableList actionsList))
//                {
//                    actionsList = new ReorderableList(sceneProp.serializedObject, actionsProp, true, true, true, true);

//                    actionsList.drawHeaderCallback = (Rect r) =>
//                    {
//                        EditorGUI.LabelField(r, "Scene Actions");
//                    };

//                    actionsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
//                    {
//                        var actionProp = actionsProp.GetArrayElementAtIndex(index);
//                        var actionObj = actionProp.managedReferenceValue as GSC_SceneAction;
//                        string label = actionObj != null ? actionObj.ID : "Null Action";

//                        // Espaço para o “handle” de arraste (cerca de 20px)
//                        Rect propertyRect = new Rect(rect.x + 20, rect.y, rect.width - 20, EditorGUIUtility.singleLineHeight);

//                        // Desenha o campo usando o ID como label
//                        EditorGUI.PropertyField(propertyRect, actionProp, new GUIContent(label), true);
//                    };

//                    actionsList.elementHeightCallback = (int index) =>
//                    {
//                        var actionProp = actionsProp.GetArrayElementAtIndex(index);
//                        return EditorGUI.GetPropertyHeight(actionProp, true) + 4;
//                    };

//                    // Substitui o botão de adicionar para exibir um menu com as opções
//                    actionsList.onAddDropdownCallback = (Rect buttonRect, ReorderableList list) =>
//                    {
//                        GenericMenu menu = new GenericMenu();
//                        // Captura a propriedade localmente para uso no lambda
//                        SerializedProperty actionsForScene = actionsProp.Copy();
//                        menu.AddItem(new GUIContent("Show Dialogue"), false, () => OnAddAction(actionsForScene, "ShowDialogue"));
//                        menu.AddItem(new GUIContent("Hide Dialogue"), false, () => OnAddAction(actionsForScene, "HideDialogue"));
//                        menu.AddItem(new GUIContent("Change Background"), false, () => OnAddAction(actionsForScene, "ChangeBackground"));
//                        menu.AddItem(new GUIContent("Play Sound"), false, () => OnAddAction(actionsForScene, "PlaySound"));
//                        menu.AddItem(new GUIContent("Fullscreen Message"), false, () => OnAddAction(actionsForScene, "FullscreenMessage"));
//                        menu.DropDown(buttonRect);
//                    };

//                    actionsLists[key] = actionsList;
//                }

//                // Calcula o retângulo para a lista interna
//                Rect listRect = new Rect(rect.x, foldoutRect.y + EditorGUIUtility.singleLineHeight + 2, rect.width, actionsLists[key].GetHeight());
//                actionsLists[key].DoList(listRect);
//            }
//        };

//        sceneList.elementHeightCallback = (int index) =>
//        {
//            SerializedProperty sceneProp = scenesProp.GetArrayElementAtIndex(index);
//            string key = sceneProp.propertyPath;
//            float height = EditorGUIUtility.singleLineHeight + 4; // Cena (nome fixo)
//            height += EditorGUIUtility.singleLineHeight + 2; // Foldout de Actions
//            if (foldouts.ContainsKey(key) && foldouts[key])
//            {
//                if (actionsLists.TryGetValue(key, out ReorderableList actionsList))
//                    height += actionsList.GetHeight();
//                else
//                    height += 60;
//            }
//            return height;
//        };
//    }

//    private void OnAddAction(SerializedProperty actionsProp, string actionType)
//    {
//        // Atualiza a propriedade em caso de cópia
//        actionsProp.serializedObject.Update();
//        int index = actionsProp.arraySize;
//        actionsProp.InsertArrayElementAtIndex(index);
//        SerializedProperty newElement = actionsProp.GetArrayElementAtIndex(index);

//        switch (actionType)
//        {
//            case "ShowDialogue":
//                newElement.managedReferenceValue = new GSC_ShowDialogueAction();
//                break;
//            case "HideDialogue":
//                newElement.managedReferenceValue = new GSC_HideDialogueAction();
//                break;
//            case "ChangeBackground":
//                newElement.managedReferenceValue = new GSC_ChangeBackgroundAction();
//                break;
//            case "PlaySound":
//                newElement.managedReferenceValue = new GSC_PlaySoundAction();
//                break;
//            case "FullscreenMessage":
//                newElement.managedReferenceValue = new GSC_ShowFullMessageAction();
//                break;
//        }
//        actionsProp.serializedObject.ApplyModifiedProperties();
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();

//        // Botões para salvar e carregar JSON
//        if (GUILayout.Button("Save to JSON"))
//            SaveToJson();
//        if (GUILayout.Button("Load from JSON"))
//            LoadFromJson();

//        EditorGUILayout.Space();
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("SavedSceneBlock"));
//        EditorGUILayout.Space();


//        sceneList.DoLayoutList();

//        serializedObject.ApplyModifiedProperties();
//    }

//    private void SaveToJson()
//    {
//        GSC_SceneBlockUnit chapter = (GSC_SceneBlockUnit)target;
//        string json = JsonUtility.ToJson(chapter, true);
//        File.WriteAllText(Path.Combine(SavePath,$"{chapter.name}.json"), json);
//        Debug.Log("Chapter data saved to: " + SavePath);
//    }

//    private void LoadFromJson()
//    {
//        GSC_SceneBlockUnit chapter = (GSC_SceneBlockUnit)target;
//        if (chapter.SavedSceneBlock != null)
//        {
//            JsonUtility.FromJsonOverwrite(chapter.SavedSceneBlock.text, target);
//            serializedObject.Update();
//            Debug.Log("Chapter data loaded from: " + SavePath);
//        }
//        else
//        {
//            Debug.LogWarning("No save file found at: " + SavePath);
//        }
//    }
//}
