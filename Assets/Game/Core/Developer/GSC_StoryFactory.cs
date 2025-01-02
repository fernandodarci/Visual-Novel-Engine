using System;
using System.Collections;

public class GSC_StoryFactory
{
    // Main story unit
    public GSC_StoryUnit MainStory;
    // Currently active chapter, scene, and container
    public GSC_ChapterUnit CurrentChapter;
    public GSC_SceneUnit CurrentScene;
    public GSC_ContainerUnit CurrentContainer;

    // Initializes a new story
    public GSC_StoryFactory StartStory()
    {
        MainStory = new GSC_StoryUnit();
        return this;
    }

    // Adds a new chapter with title and subtitle
    public GSC_StoryFactory WithChapter(string chapterTitle, string chapterSubtitle)
    {
        if (CurrentScene != null)
            CurrentChapter.Scenes.Add(CurrentScene);

        if (CurrentChapter != null)
            MainStory.Chapters.Add(CurrentChapter);

        CurrentChapter = new GSC_ChapterUnit(chapterTitle, chapterSubtitle);
        CurrentScene = null;
        CurrentContainer = null;
        return this;
    }

    // Adds a new scene with a specific index
    public GSC_StoryFactory WithScene(int index, float seconds = 0f)
    {
        if (CurrentContainer != null)
            CurrentScene.Containers.Add(CurrentContainer);

        if (CurrentScene != null)
            CurrentChapter.Scenes.Add(CurrentScene);

        CurrentScene = new GSC_SceneUnit(index);
        CurrentContainer = null;
        return this;
    }

    // Adds a new container with a calling name
    public GSC_StoryFactory WithContainer(string calling)
    {
        if (CurrentContainer != null)
            CurrentScene.Containers.Add(CurrentContainer);

        CurrentContainer = new GSC_ContainerUnit(calling);
        return this;
    }

    public GSC_StoryFactory WithContainer<T>(string calling, T parameter)
    {
        if (CurrentContainer != null)
            CurrentScene.Containers.Add(CurrentContainer);
        if(parameter is GSC_Parameter param)
        {
            var container = new GSC_ContainerUnit<GSC_Parameter>(calling);
            container.Set(param);
            CurrentContainer = container;
        }
        else
        {
            GSC_ContainerUnit<T> container = new(calling);
            container.Set(parameter);
            CurrentContainer = container;
        }
        return this;
    }

    // Adds a boolean parameter to the current container
    public GSC_StoryFactory WithParameter(string parameterName, bool parameterValue)
    {
        CurrentContainer.Set(parameterName, parameterValue);
        return this;
    }

    // Adds an integer parameter to the current container
    public GSC_StoryFactory WithParameter(string parameterName, int parameterValue)
    {
        CurrentContainer.Set(parameterName, parameterValue);
        return this;
    }

    // Adds a float parameter to the current container
    public GSC_StoryFactory WithParameter(string parameterName, float parameterValue)
    {
        CurrentContainer.Set(parameterName, parameterValue);
        return this;
    }

    // Adds a string parameter to the current container
    public GSC_StoryFactory WithParameter(string parameterName, string parameterValue)
    {
        CurrentContainer.Set(parameterName, parameterValue);
        return this;
    }

    // Finalizes and adds the current chapter to the story
    public GSC_StoryFactory End()
    {
        if (CurrentContainer != null)
            CurrentScene.Containers.Add(CurrentContainer);

        if (CurrentScene != null)
            CurrentChapter.Scenes.Add(CurrentScene);

        if (CurrentChapter != null)
            MainStory.Chapters.Add(CurrentChapter);

        CurrentChapter = null;
        CurrentScene = null;
        CurrentContainer = null;
        return this;
    }

    
}
