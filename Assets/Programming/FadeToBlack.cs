using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    public Image screenFaderImage;
    public Color fadeColor;
    public Text moreToComeText;
    public float secondsUntilFadeStarts;
    public float fadeDuration;
    private float timeWhenFadeShouldBeFull;
    private float timeFadeStarted;

    public void Start()
    {
        StartCoroutine(WaitThenFadeToBlack(secondsUntilFadeStarts));
    }

    IEnumerator WaitThenFadeToBlack(float secondsToWait) {
        yield return new WaitForSeconds(secondsToWait);

        StartCoroutine(CofadeToColorInFrontFromClearInBack());
    }

    IEnumerator CofadeToColorInFrontFromClearInBack()
    {
        timeFadeStarted = Time.time;
        timeWhenFadeShouldBeFull = timeFadeStarted + fadeDuration;

        while (!screenFaderImage.color.Equals(fadeColor))
        {
            float fractionFaded = (Time.time - timeFadeStarted) / (timeWhenFadeShouldBeFull - timeFadeStarted);
            screenFaderImage.color = Color.Lerp(Color.clear, fadeColor, fractionFaded);
            yield return null;
        }

        moreToComeText.gameObject.SetActive(true);
    }

}
