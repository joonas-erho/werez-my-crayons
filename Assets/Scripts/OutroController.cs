using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OutroController : MonoBehaviour
{
    private string string1 = "Snot really likes his crayonz";
    private string string2 = "And now he founded his favorites!";
    private string string3 = "The sweet.. delectable, vhite crayon, with a hint of vanilla and titanium dioxide";
    private string string4 = "Not to forget the ever tasty, blacke crayon, with its enticing licorice aroma and an aftertaste\nof carbonized wood";
    private string string5 = "Crayonz are yummy";

    [SerializeField] private Text text;

    [SerializeField] private float time0; 
    [SerializeField] private float time1;
    [SerializeField] private float time2;
    [SerializeField] private float time3;
    [SerializeField] private float time4;
    [SerializeField] private float time5;

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
        text.text = string5;
        yield return new WaitForSeconds(time5);
        SceneController.Instance.LoadScene("MainMenu");
    }
}