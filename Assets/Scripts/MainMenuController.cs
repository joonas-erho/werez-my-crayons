using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] UIDocument uiDocument;

    private void Awake()
    {
        var root = uiDocument.rootVisualElement;

        root.Q<Button>("PlayButton").clicked += Play;
        root.Q<Button>("LevelSelectButton").clicked += OpenLevelSelect;
        root.Q<Button>("SettingsButton").clicked += OpenSettings;
        root.Q<Button>("QuitButton").clicked += Quit;
    }

    private void Play()
    {
        
    }

    private void OpenLevelSelect()
    {
        
    }

    private void OpenSettings()
    {
        SceneController.Instance.ToggleMusicMute();
    }

    private void Quit()
    {
        Application.Quit();
    }
}
