using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GSC_StoryUnitCompiler : MonoBehaviour
{
    [SerializeField] private GSC_StoryUnit Unit;
    [SerializeField] private TextAsset CompiledUnit;

    public void ClickToCompile()
    {
        if(Unit != null) OnCompile();
        else if(CompiledUnit != null) OnRestore();
    }

    public void OnCompile()
    {
        if (Unit == null) return;

        string saveFile = Path.Combine(GSC_Constants.SavePath, $"{Unit.name}.json");
        var data = new GSC_StoryUnitData
        {
            Name = Unit.name,
            Chapters = new List<GSC_ChapterUnitData>()
        };

        if (Unit.Chapters != null)
        {
            foreach (var chapter in Unit.Chapters)
            {
                var chapterData = CompileChapter(chapter);
                if (chapterData != null)
                    data.Chapters.Add(chapterData);
            }
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFile, json);
    }

    public void OnRestore()
    {
        if (CompiledUnit == null) return;

        string json = CompiledUnit.text;
        var data = JsonUtility.FromJson<GSC_StoryUnitData>(json);

        if (Unit != null) Destroy(Unit.gameObject);
        GameObject go = new GameObject(data.Name);
        Unit = go.AddComponent<GSC_StoryUnit>();
        Unit.transform.SetParent(transform);
        Unit.Chapters = new List<GSC_ChapterUnit>();

        foreach (var chapterData in data.Chapters)
        {
            var chapter = RestoreChapter(chapterData);
            if (chapter != null)
            {
                chapter.transform.SetParent(Unit.transform);
                Unit.Chapters.Add(chapter);
            }
        }

#if UNITY_EDITOR
        string relativePath = Path.Combine("Assets/Story", $"{Unit.name}.prefab").Replace("\\", "/");
        PrefabUtility.SaveAsPrefabAsset(Unit.gameObject,relativePath);
#endif

    }

    //----- Compilation Helpers -----
    private GSC_ChapterUnitData CompileChapter(GSC_ChapterUnit chapter)
    {
        if (chapter == null) return null;
        var chapterData = new GSC_ChapterUnitData
        {
            Name = chapter.name,
            Sequences = new List<GSC_SequenceUnitData>()
        };

        if (chapter.Sequences != null)
        {
            foreach (var sequence in chapter.Sequences)
            {
                var seqData = CompileSequence(sequence);
                if (seqData != null)
                    chapterData.Sequences.Add(seqData);
            }
        }
        return chapterData;
    }

    private GSC_SequenceUnitData CompileSequence(GSC_SequenceUnit sequence)
    {
        if (sequence == null) return null;
        var sequenceData = new GSC_SequenceUnitData
        {
            Name = sequence.name,
            Scenes = new List<GSC_SceneUnitData>()
        };

        if (sequence.Scenes != null)
        {
            foreach (var scene in sequence.Scenes)
            {
                var sceneData = CompileScene(scene);
                if (sceneData != null)
                    sequenceData.Scenes.Add(sceneData);
            }
        }
        return sequenceData;
    }

    private GSC_SceneUnitData CompileScene(GSC_SceneUnit scene)
    {
        if (scene == null) return null;
        var sceneData = new GSC_SceneUnitData
        {
            Name = scene.name,
            Actions = new List<GSC_SceneActionData>()
        };

        if (scene.Actions != null)
        {
            foreach (var action in scene.Actions)
            {
                var actionData = CompileAction(action);
                if (actionData != null)
                    sceneData.Actions.Add(actionData);
            }
        }
        return sceneData;
    }

    private GSC_SceneActionData CompileAction(GSC_SceneAction action)
    {
        if (action == null) return null;
        return new GSC_SceneActionData
        {
            Name = action.name,
            Type = action.GetID(),
            Data = action.Compile().ToJson()
        };
    }

    //----- Restoration Helpers -----
    private GSC_ChapterUnit RestoreChapter(GSC_ChapterUnitData data)
    {
        GameObject chapterGO = new GameObject(data.Name);
        var chapter = chapterGO.AddComponent<GSC_ChapterUnit>();
        chapter.name = data.Name;
        chapter.Sequences = new List<GSC_SequenceUnit>();

        if (data.Sequences != null)
        {
            foreach (var seqData in data.Sequences)
            {
                var sequence = RestoreSequence(seqData);
                if (sequence != null)
                {
                    sequence.transform.SetParent(chapter.transform);
                    chapter.Sequences.Add(sequence);
                }
            }
        }
        return chapter;
    }

    private GSC_SequenceUnit RestoreSequence(GSC_SequenceUnitData data)
    {
        GameObject sequenceGO = new GameObject(data.Name);
        var sequence = sequenceGO.AddComponent<GSC_SequenceUnit>();
        sequence.name = data.Name;
        sequence.Scenes = new List<GSC_SceneUnit>();

        if (data.Scenes != null)
        {
            foreach (var sceneData in data.Scenes)
            {
                var scene = RestoreScene(sceneData);
                if (scene != null)
                {
                    scene.transform.SetParent(sequence.transform);
                    sequence.Scenes.Add(scene);
                }
            }
        }
        return sequence;
    }

    private GSC_SceneUnit RestoreScene(GSC_SceneUnitData data)
    {
        GameObject sceneGO = new GameObject(data.Name);
        var scene = sceneGO.AddComponent<GSC_SceneUnit>();
        scene.name = data.Name;
        scene.Actions = new List<GSC_SceneAction>();

        if (data.Actions != null)
        {
            foreach (var actionData in data.Actions)
            {
                RestoreAction(actionData,ref scene);
            }
        }
        return scene;
    }

    private void RestoreAction(GSC_SceneActionData data,ref GSC_SceneUnit scene)
    {
        GameObject actionGO = new GameObject(data.Name);
        actionGO.transform.SetParent(scene.transform);

        switch (data.Type)
        {
            case GSC_ChangeBackgroundAction.ID:
                var ref1 = actionGO.AddComponent<GSC_ChangeBackgroundAction>();
                ref1.Decompile(data.Data);
                scene.SetActionReference(ref1);
                break;
            case GSC_ShowDialogueAction.ID:
                var ref2 = actionGO.AddComponent<GSC_ShowDialogueAction>();
                ref2.Decompile(data.Data);
                scene.SetActionReference(ref2);
                break;
            case GSC_HideDialogueAction.ID:
                var ref3 = actionGO.AddComponent<GSC_HideDialogueAction>();
                ref3.Decompile(data.Data);
                scene.SetActionReference(ref3);
                break;
            case GSC_ShowFullMessageAction.ID:
                var ref4 = actionGO.AddComponent<GSC_ShowFullMessageAction>();
                ref4.Decompile(data.Data);
                scene.SetActionReference(ref4);
                break;
            default:
                Destroy(actionGO.gameObject);
                break;
        }

        
    }

    //----- Data Classes -----
    [Serializable]
    public class GSC_StoryUnitData
    {
        public string Name;
        public List<GSC_ChapterUnitData> Chapters;
    }

    [Serializable]
    public class GSC_ChapterUnitData
    {
        public string Name;
        public List<GSC_SequenceUnitData> Sequences;
    }

    [Serializable]
    public class GSC_SequenceUnitData
    {
        public string Name;
        public List<GSC_SceneUnitData> Scenes;
    }

    [Serializable]
    public class GSC_SceneUnitData
    {
        public string Name;
        public List<GSC_SceneActionData> Actions;
    }

    [Serializable]
    public class GSC_SceneActionData
    {
        public string Name;
        public string Type;
        public string Data;
    }
}
