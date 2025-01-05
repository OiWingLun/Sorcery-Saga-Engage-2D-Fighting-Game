using UnityEngine;
using System;

[Serializable]
public class Sound {
    public string name;
    public AudioClip clip;
    public bool loop;
    private AudioSource Source;
    public void SetSource(AudioSource source) {
        Source = source;
        Source.clip = clip;
    }
    public void Play() {
        Source.loop = loop;
        Source.Play();
    }
    public void Stop() {
        Source.Stop();
    }
}
public class AudioController : MonoBehaviour
{
    public static AudioController audioController;
    [SerializeField]
    private Sound[] sounds;
    private void Awake() 
{
    if (audioController == null) {
        DontDestroyOnLoad(gameObject);
        audioController = this;
    }
    else if (audioController != this) {
        Destroy(gameObject);
        return; // Ensure the rest of the code doesn't execute for the duplicate object.
    }

    for (int i = 0; i < sounds.Length; i++) {
        GameObject go = new GameObject("sound_" + i + "_" + sounds[i].name);
        go.transform.SetParent(this.transform);

        // Move the GameObject off-screen
        go.transform.position = new Vector3(-9999, -9999, -9999);

        sounds[i].SetSource(go.AddComponent<AudioSource>());
    }
}
    public void PlaySound(string name) {
        for (int i = 0; i < sounds.Length; i++) {
            if(sounds[i].name == name) {
                sounds[i].Play();
                return;
            }
        }
    }
    public void PlayBGMSound(string name) {
        for (int i = 0; i < sounds.Length; i++) {
            if(sounds[i].name == name) {
                sounds[i].Play();
                return;
            }
        }
    }
    public void StopSound(string name) {
        for (int i = 0; i < sounds.Length; i++) {
            if(sounds[i].name == name) {
                sounds[i].Stop();
                return;
            }
        }
    }
    public void StopAllSound() {
        for (int i = 0; i < sounds.Length; i++) 
        {
            sounds[i].Stop();
        }
        }
   }

