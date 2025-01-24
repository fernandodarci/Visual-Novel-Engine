using System;

//public class GSC_StoryFactory
//{
//    // Main story unit
//    private GSC_StoryUnit MainStory = new GSC_StoryUnit();
//    // Currently active chapter, scene, and container
//    private GSC_ChapterUnit CurrentChapter = null;
//    private GSC_SceneUnit CurrentScene = null;
//    private GSC_ContainerUnit CurrentContainer = null;

//    public static GSC_StoryFactory Create()
//    {
//        return new GSC_StoryFactory();
//    }

//    // Adds a new chapter with title and subtitle
//    public GSC_StoryFactory WithChapter(string chapterTitle, string chapterSubtitle = "")
//    {
//        CurrentChapter = new GSC_ChapterUnit(chapterTitle, chapterSubtitle);
//        MainStory.Chapters.Add(CurrentChapter);
//        return this;
//    }

//    // Adds a new scene with a specific index
//    public GSC_StoryFactory WithScene(int index)
//    {
//        if (CurrentChapter == null)
//        {
//            throw new InvalidOperationException("No chapter available. Add a chapter before adding a scene.");
//        }

//        CurrentScene = new GSC_SceneUnit(index);
//        CurrentChapter.Scenes.Add(CurrentScene);
//        return this;
//    }

//    // Adds a new container with a calling name
//    public GSC_StoryFactory WithContainer(string calling)
//    {
//        if (CurrentScene == null)
//        {
//            throw new InvalidOperationException("No scene available. Add a scene before adding a container.");
//        }

//        CurrentContainer = new GSC_ContainerUnit(calling);
//        CurrentScene.Containers.Add(CurrentContainer);
//        return this;
//    }

//    public GSC_StoryFactory WithContainer<T>(string calling, T parameter)
//    {
//        if (CurrentScene == null)
//        {
//            throw new InvalidOperationException("No scene available. Add a scene before adding a container.");
//        }

//        if (parameter is GSC_Parameter param)
//        {
//            var container = new GSC_ContainerUnit<GSC_Parameter>(calling);
//            container.Set(param);
//            CurrentContainer = container;
//        }
//        else
//        {
//            var container = new GSC_ContainerUnit<T>(calling);
//            container.Set(parameter);
//            CurrentContainer = container;
//        }

//        CurrentScene.Containers.Add(CurrentContainer);
//        return this;
//    }

//    // Adds a boolean parameter to the current container
//    public GSC_StoryFactory WithParameter(string parameterName, bool parameterValue)
//    {
//        CurrentContainer?.Set(parameterName, parameterValue);
//        return this;
//    }

//    // Adds an integer parameter to the current container
//    public GSC_StoryFactory WithParameter(string parameterName, int parameterValue)
//    {
//        CurrentContainer?.Set(parameterName, parameterValue);
//        return this;
//    }

//    // Adds a float parameter to the current container
//    public GSC_StoryFactory WithParameter(string parameterName, float parameterValue)
//    {
//        CurrentContainer?.Set(parameterName, parameterValue);
//        return this;
//    }

//    // Adds a string parameter to the current container
//    public GSC_StoryFactory WithParameter(string parameterName, string parameterValue)
//    {
//        CurrentContainer?.Set(parameterName, parameterValue);
//        return this;
//    }

//    // Shortcuts for common containers
//    #region Special Containers
//    public GSC_StoryFactory WithShowImage(string group, string image, string controller,
//        int layer, float fadeTime, float waitTime)
//    {
//        return WithContainer("ShowImage")
//            .WithParameter("SpriteGroup", group)
//            .WithParameter("SpriteName", image)
//            .WithParameter("Controller", controller)
//            .WithParameter("Layer", layer)
//            .WithParameter("Duration", fadeTime)
//            .WithParameter("HideAfter", waitTime);
//    }

//    #endregion

//    // Finalizes and gets the main story
//    public GSC_StoryUnit Build()
//    {
//        return MainStory;
//    }
//}
