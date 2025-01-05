using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image buttonImage; 
    public Sprite normalSprite; 
    public Sprite hoverSprite;  
    public new AudioSource audio;
    public void playButton()
    {
        audio.Play();
        DontDestroyOnLoad(audio.gameObject);
    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void VSPlayer()
    {
        SceneManager.LoadSceneAsync(15);
    }
    public void Practice()
    {
        SceneManager.LoadSceneAsync(16);
    }
    public void Menu()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void Option()
    {
        SceneManager.LoadSceneAsync(5);
    }
    public void ControlSettings()
    {
        SceneManager.LoadSceneAsync(6);
    }
    public void OnPointerEnter(PointerEventData eventData)
{
    if (buttonImage != null && hoverSprite != null)
    {
        buttonImage.sprite = hoverSprite;
    }
}

public void Exit() 
{
    Application.Quit();
}

public void OnPointerExit(PointerEventData eventData)
{
    if (buttonImage != null && normalSprite != null)
    {
        buttonImage.sprite = normalSprite;
    }
}
}


