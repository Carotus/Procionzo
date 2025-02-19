using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance{ get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip, Vector3 position)
    {
        GameObject soundObject = new GameObject("Sound");
        soundObject.transform.position = position;
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(soundObject, clip.length);
    }

    public void PlaySound(AudioClip clip, GameObject targetObject)
    {
        AudioSource audioSource = targetObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = targetObject.AddComponent<AudioSource>();
        }
        audioSource.clip = clip;
        audioSource.PlayOneShot(clip);
    }

    public void PlaySoundOnRepeat(AudioClip clip, GameObject targetObject)
    {
        AudioSource audioSource = targetObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = targetObject.AddComponent<AudioSource>();
        }
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopSound(GameObject targetObject)
    {
        AudioSource audioSource = targetObject.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }
    }

    
}
    

