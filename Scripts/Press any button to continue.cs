using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PressAnyButtonEffect : MonoBehaviour
{
    public TMP_Text text; 
    public float shineSpeed = 1.0f;
    public float fastShineSpeed = 3.0f;
    public float fastShineDuration = 2.0f; 
    public float delayBeforeMenu = 1.0f;
    public AudioSource clickSound; 

    private bool isClicked = false; 
    private Coroutine shineCoroutine; 

    void Start()
    {
        shineCoroutine = StartCoroutine(ShineText()); 
    }
    void Update()
    {
        if (!isClicked && (Input.anyKeyDown || Input.GetMouseButtonDown(0)))
        {
            isClicked = true;
            StartCoroutine(HandleClick()); 
        }
    }
    IEnumerator ShineText()
    {
        while (true)
        {
            float currentShineSpeed = isClicked ? fastShineSpeed : shineSpeed;
            for (float alpha = 0f; alpha <= 1f; alpha += Time.deltaTime * currentShineSpeed)
            {
                SetTextAlpha(alpha);
                yield return null;
            }
            for (float alpha = 1f; alpha >= 0f; alpha -= Time.deltaTime * currentShineSpeed)
            {
                SetTextAlpha(alpha);
                yield return null;
            }
            if (isClicked) break;
        }
    }
    IEnumerator FastShine()
    {
        float startTime = Time.time;
        while (Time.time < startTime + fastShineDuration)
        {
            float currentShineSpeed = fastShineSpeed;
            for (float alpha = 0f; alpha <= 1f; alpha += Time.deltaTime * currentShineSpeed)
            {
                SetTextAlpha(alpha);
                yield return null;
            }
            for (float alpha = 1f; alpha >= 0f; alpha -= Time.deltaTime * currentShineSpeed)
            {
                SetTextAlpha(alpha);
                yield return null;
            }
        }
        shineCoroutine = StartCoroutine(ShineText());
    }
    void SetTextAlpha(float alpha)
    {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }
    IEnumerator HandleClick()
    {
        if (clickSound != null)
        {
            clickSound.Play();
        }
        if (shineCoroutine != null)
        {
            StopCoroutine(shineCoroutine);
        }
        yield return StartCoroutine(FastShine());
        yield return new WaitForSeconds(delayBeforeMenu); 
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
        while (!asyncLoad.isDone)
        {
            yield return null; 
        }
    }
}
