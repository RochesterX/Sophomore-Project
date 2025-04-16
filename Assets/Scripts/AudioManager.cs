using System.Collections.Generic;
using UnityEngine; using Game; using Music; using Player;

namespace Music
{

public class AudioManager : MonoBehaviour
{
    public List<SoundEffect> soundEffects = new List<SoundEffect>();

    public static AudioManager Instance;

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

        foreach (Transform child in transform)
        {
            var soundEffect = new SoundEffect(child.name, child.GetComponent<AudioSource>());
            if (soundEffect != null)
            {
                soundEffects.Add(soundEffect);
            }
        }

        print(soundEffects);
    }

    public void PlaySound(string soundName)
    {
        if (soundName == "Punch")
        {
            soundEffects.Find(x => x.name == "Punch").audioSource.Play();
            soundEffects.Find(x => x.name == "Punch 2").audioSource.Play();
            soundEffects.Find(x => x.name == "Punch 3").audioSource.Play();

            return;
        }

        foreach (var soundEffect in soundEffects)
        {
            if (soundEffect.name == soundName)
            {
                soundEffect.audioSource.Play();
                return;
            }
        }
        Debug.LogWarning($"Sound '{soundName}' not found!");
    }
}

public class SoundEffect
{
    public string name;
    public AudioSource audioSource;

    public SoundEffect(string name, AudioSource audioSource)
    {
        this.name = name;
        this.audioSource = audioSource;
    }
}
}