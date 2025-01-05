using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("----------- Audio Source ------------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("----------- Audio Clip ------------")]
    public AudioClip background;
    public AudioClip ClickButton;
    public AudioClip Back;
    public AudioClip ChooseYourCharacter;
    public AudioClip Round1;
    public AudioClip Round2;
    public AudioClip FinalRound;

    private void Start()
    {
        // Play background music when the game starts
        musicSource.clip = background;
        musicSource.Play();

        // Check and play specific sound for the current scene
        PlaySceneSpecificSound();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    private void PlaySceneSpecificSound()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        switch (currentSceneName)
        {
            case "Character Select":
                PlaySFX(ChooseYourCharacter);
                break;

            case "Round1Scene":
                PlaySFX(Round1);
                break;

            case "Round2Scene":
                PlaySFX(Round2);
                break;

            case "FinalRoundScene":
                PlaySFX(FinalRound);
                break;

            default:
                Debug.Log($"No specific sound set for scene: {currentSceneName}");
                break;
        }
    }
}
