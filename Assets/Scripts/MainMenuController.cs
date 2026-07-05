using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] UIDocument uiDocument;

    private void Awake()
    {
        var root = uiDocument.rootVisualElement;

        root.Q<Button>("PlayButton").clicked += Play;
        root.Q<Button>("SettingsButton").clicked += OpenSettings;
        root.Q<Button>("QuitButton").clicked += Quit;
    }

    private bool _clicked = false;

    private void Play()
    {
        if (_clicked) return;
        SceneController.Instance.LoadScene("Intro");
        _clicked = true;
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
