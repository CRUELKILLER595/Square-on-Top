using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class FadeInFadeOut : MonoBehaviour
{ public Image FadeImage;
public float FadeDuration=0.5f;
public float DisplayDuration=1.5f;
public string NextScreen="TitleScreen";

    void Start()
    {
        StartCoroutine(FadeSequence());
    }
    IEnumerator FadeSequence()
    { yield return StartCoroutine(Fade(0,1));
        yield return StartCoroutine(Fade(1,0));
      yield return new WaitForSeconds(DisplayDuration);
        SceneManager.LoadScene(NextScreen);
    }
    IEnumerator Fade(float StartAlpha,float EndAlpha)
    {
        float time=0;
        Color color=FadeImage.color;
        while (time < FadeDuration)
        {
            float alpha= Mathf.Lerp(StartAlpha,EndAlpha,time/FadeDuration);
            FadeImage.color=new Color(color.r,color.g,color.b,alpha);
            time+=Time.deltaTime;
            yield return null;
        }
        FadeImage.color=new Color(color.r,color.g,color.b,EndAlpha);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
