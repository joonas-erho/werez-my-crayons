using System;
using UnityEngine;
using UnityEngine.UI;

public class CrayonPackController : MonoBehaviour
{
    [SerializeField] private Image greenCrayon;
    [SerializeField] private Image pinkCrayon;
    [SerializeField] private Image redCrayon;
    [SerializeField] private Image blueCrayon;
    [SerializeField] private Image yellowCrayon;

    [SerializeField] private Sprite[] sprites;
    
    private Image[] images = new Image[5];

    private void Awake()
    {
        images[0] = greenCrayon;
        images[1] = pinkCrayon;
        images[2] = redCrayon;
        images[3] = blueCrayon;
        images[4] = yellowCrayon;
    }

    public void UpdateTextures(int[] values)
    {
        for (int i = 0; i < 5; i++)
        {
            GetSprite(values[i], images[i]);
        }
    }

    public void SelectCrayon(int value)
    {
        for (int i = 0; i < 5; i++)
        {
            var pos = images[i].gameObject.transform.transform.localPosition;
            pos.y = i == value ? 125 : 50;
            images[i].gameObject.transform.transform.localPosition = pos;
        }
    }

    private void GetSprite(int value, Image image)
    {
        if (value == 0)
        {
            image.gameObject.SetActive(false);
        }
        float valueAsFloat = value;
        valueAsFloat /= 5.9f; // Magic number, sorry! Max value is 100 and there are 17 sprites.
        var sprite = sprites[(int)valueAsFloat];

        image.sprite = sprite;
    }
}
