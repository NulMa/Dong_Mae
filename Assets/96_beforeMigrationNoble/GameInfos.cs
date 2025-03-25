using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public static class TagQuery
{
    public enum tag
    {
        Interactive,
        Tilemaps,
        Gate,
        MassObj,
        MoveableLimit,
        Rope,
        Moveable,
        Water,
        Ground,
    }

    public static bool Matching(string target, TagQuery.tag _tag)
    {
        if (string.Equals(target, _tag.ToString()))
        {
            return true;
        }

        return false;
    }


}


[System.Serializable]
public enum AtkType { normal, water, fire, earth, metal }




[System.Serializable]
public static class SceneLoader
{ 
    public enum SceneList
    { 
        MainMenu,
        NewGameLoader,  // MainMenu -> NewGameLoader -> Game
        ContinueLoader, // MainMenu -> ConitnueLoader -> Game
        Game,
        // EndingLoader
        // Ending
    }

    public static void SceneLoad(SceneList _sceneName)
    {
        switch (_sceneName)
        { 
            case SceneList.MainMenu:
                break;
            case SceneList.NewGameLoader:
                break;
            case SceneList.ContinueLoader:
                break;
            case SceneList.Game:
                break;
            //case SceneList.Ending / EndingLoader:
            //    break;
        }
    }

    public static void SceneLoad(int _sceneIndex)
    {
        SceneLoad((SceneList)_sceneIndex);
    }
}


[System.Serializable]
public enum enumLayer
{ 
    Ground,
    UnderGround,
}


public static class LayerManager
{
    public static string GetLayer(enumLayer _layer)
    { 
        return _layer.ToString();
    }

    public static int GetLayer(string _layer)
    { 
        return LayerMask.NameToLayer(_layer);
    }
}
