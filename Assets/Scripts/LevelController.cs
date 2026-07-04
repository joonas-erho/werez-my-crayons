using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private int levelNumber;
    [SerializeField] private Scene nextScene;

    [Tooltip("Regular Crayon")]
    [SerializeField]
    [Min(0)]
    private int greenCrayonLimit;

    [SerializeField] private int remainingGreenCrayon;

    [SerializeField] private float crayonPerPoint;

    private void Start()
    {
        remainingGreenCrayon = greenCrayonLimit;
    }

    public void Advance()
    {
        SceneManager.LoadScene(nextScene.name);
    }

    public bool IsEnoughCrayonCharge(int pointCount)
    {
        return remainingGreenCrayon >= (pointCount * crayonPerPoint);
    }

    public bool RemoveCrayonCharge(int pointCount)
    {
        remainingGreenCrayon -= (int)(pointCount * crayonPerPoint);
        return remainingGreenCrayon <= 0;
    }
}
