using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private Texture2D cursor;
    
    [SerializeField] private Animator animator;

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
        LoadScene("MainMenu");
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

    public void ToggleMusicMute()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void LoadScene(string name)
    {
        StartCoroutine(TransitionToScene(name));
    }
    
    
    public IEnumerator TransitionToScene(string name)
    {
        animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(name);
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("test");
        animator.SetTrigger("FadeOut");
    }
}
