using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private Texture2D cursor;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        SceneManager.LoadScene("MainMenu");
        // Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

    public void ToggleMusicMute()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
