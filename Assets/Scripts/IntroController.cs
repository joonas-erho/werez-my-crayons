using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
    private string string1 = "Snot really likes his crayonz";
    private string string2 = "Like reallly LIKES";
    private string string3 = "Butt now he loses them? Espesially his favorites, the Vhite and Blacke ones! So sad";
    private string string4 = "Crayonz-packs are all over the backed-yard! Oh wel, better grab them all to find the real best ones";

    [SerializeField] private Text text;

    [SerializeField] private float time0; 
    [SerializeField] private float time1;
    [SerializeField] private float time2;
    [SerializeField] private float time3;
    [SerializeField] private float time4;

    private void Start()
    {
        StartCoroutine(AnimateText());
    }

    private IEnumerator AnimateText()
    {
        yield return new WaitForSeconds(time0);
        text.text = string1;
        yield return new WaitForSeconds(time1);
        text.text = string2;
        yield return new WaitForSeconds(time2);
        text.text = string3;
        yield return new WaitForSeconds(time3);
        text.text = string4;
        yield return new WaitForSeconds(time4);
        SceneController.Instance.LoadScene("Level_01");
    }
}
