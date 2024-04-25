using UnityEngine;

public class DropDown : MonoBehaviour
{
    public SceneLoader sceneLoader;

    public void HandleInputData(int val)
    {
        Debug.Log("Seleccionaste la opción número " + (val + 1));
        switch (val)
        {
            case 0:
                sceneLoader.sceneName = "Level2";
                break;
            case 1:
                sceneLoader.sceneName = "Level3";
                break;
            case 2:
                sceneLoader.sceneName = "Level4";
                break;
        }
    }
}
