using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public enum Crayon
{
    Green,
    Pink,
    Red,
    Blue,
    Yellow
}

public class LevelController : MonoBehaviour
{
    [SerializeField] private int levelNumber;
    [SerializeField] private string nextScene;
    
    [SerializeField] private CrayonPackController crayonPackController;

    [SerializeField] private Text text;

    [SerializeField] private Image thoughtBg;
    [SerializeField] private Text thoughtText;
    [SerializeField] private string thoughtContent;
    
    private Crayon _selectedCrayon;

    [Tooltip("Regular Crayon")]
    [SerializeField]
    [Min(0)]
    private int greenCrayonLimit;
    
    [Tooltip("Floating Crayon")]
    [SerializeField]
    [Min(0)]
    private int pinkCrayonLimit;
    
    [Tooltip("Water-Resistant Crayon")]
    [SerializeField]
    [Min(0)]
    private int redCrayonLimit;
    
    [Tooltip("Sticky Crayon")]
    [SerializeField]
    [Min(0)]
    private int blueCrayonLimit;
    
    [Tooltip("Bouncy Crayon")]
    [SerializeField]
    [Min(0)]
    private int yellowCrayonLimit;

    [SerializeField] private int[] remainingCrayonValues = new int[5];

    [SerializeField] private float crayonPerPoint;

    private void Start()
    {
        remainingCrayonValues[0] = greenCrayonLimit;
        remainingCrayonValues[1] = pinkCrayonLimit;
        remainingCrayonValues[2] = redCrayonLimit;
        remainingCrayonValues[3] = blueCrayonLimit;
        remainingCrayonValues[4] = yellowCrayonLimit;

        for (int i = 0; i < 5; i++)
        {
            if (remainingCrayonValues[i] != 0)
            {
                _selectedCrayon = (Crayon)i;
                break;
            }
        }
        
        crayonPackController.UpdateTextures(remainingCrayonValues);
        crayonPackController.SelectCrayon((int)_selectedCrayon);
        
        text.text = "Level " + levelNumber;
        thoughtText.text = thoughtContent;
        StartCoroutine(FadeOutPanel());
    }

    public int GetCrayonId()
    {
        return (int)_selectedCrayon;
    }

    public void LoadScene([CanBeNull] string sceneName)
    {
        SceneController.Instance.LoadScene(sceneName ?? nextScene);
    }

    public bool IsEnoughCrayonCharge(int pointCount)
    {
        return RemainingCrayon(_selectedCrayon) >= (pointCount * crayonPerPoint);
    }

    public bool RemoveCrayonCharge(int pointCount)
    {
        UseCrayon((int)(pointCount * crayonPerPoint));
        return RemainingCrayon(_selectedCrayon) <= 0;
    }

    private int RemainingCrayon(Crayon selectedCrayon) => selectedCrayon switch
    {
        Crayon.Green => remainingCrayonValues[0],
        Crayon.Pink => remainingCrayonValues[1],
        Crayon.Red => remainingCrayonValues[2],
        Crayon.Blue => remainingCrayonValues[3],
        Crayon.Yellow => remainingCrayonValues[4],
        _ => 0
    };

    private void UseCrayon(int pointCount)
    {
        switch (_selectedCrayon)
        {
            case Crayon.Green:
                remainingCrayonValues[0] -= pointCount;
                break;

            case Crayon.Pink:
                remainingCrayonValues[1] -= pointCount;
                break;

            case Crayon.Red:
                remainingCrayonValues[2] -= pointCount;
                break;

            case Crayon.Blue:
                remainingCrayonValues[3] -= pointCount;
                break;

            case Crayon.Yellow:
                remainingCrayonValues[4] -= pointCount;
                break;
        }

        if (remainingCrayonValues[(int)_selectedCrayon] <= 0)
        {
            SwitchCrayon(1);
        }
        
        crayonPackController.UpdateTextures(remainingCrayonValues);
    }

    public void SwitchCrayon(int dir)
    {
        var nextIndex = (int)_selectedCrayon;

        for (int i = 0; i < 5; i++)
        {
            nextIndex += dir;
            if (nextIndex < 0)
            {
                nextIndex = 4;
            }
            else if (nextIndex > 4)
            {
                nextIndex = 0;
            }
            if (RemainingCrayon((Crayon)nextIndex) <= 0)
            {
                continue;
            }

            _selectedCrayon = (Crayon)nextIndex;
            break;
        }
        
        crayonPackController.SelectCrayon((int)_selectedCrayon);
    }
    
    private IEnumerator FadeOutPanel()
    {
        yield return new WaitForSeconds(5f);

        const float fadeDuration = 2f;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / fadeDuration);

            var bgColor = thoughtBg.color;
            bgColor.a = Mathf.Lerp(1f, 0f, progress);
            thoughtBg.color = bgColor;

            var textColor = text.color;
            textColor.a = Mathf.Lerp(1f, 0f, progress);
            text.color = textColor;

            yield return null;
        }

        thoughtBg.gameObject.SetActive(false);
    }
}
