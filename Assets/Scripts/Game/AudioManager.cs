using System.Collections.Generic;
using UnityEngine;
using Game;
using Music;
using Player;

namespace Music
{
    /// <summary>
    /// This class manages the playback of sound effects in the game.
    /// It provides functionality to play specific sounds by name and ensures a singleton instance for global access.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        /// <summary>
        /// A list of all sound effects managed by the AudioManager.
        /// </summary>
        public List<SoundEffect> soundEffects = new List<SoundEffect>();

        /// <summary>
        /// The singleton instance of the <see cref="AudioManager"/> class.
        /// </summary>
        public static AudioManager Instance;

        /// <summary>
        /// Initializes the singleton instance and loads all child AudioSources as sound effects.
        /// </summary>
        private void Awake()
        {
            // Ensure only one instance of the AudioManager exists
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Persist across scenes
            }
            else
            {
                Destroy(gameObject); // Destroy duplicate instances
            }

            // Load all child AudioSources into the soundEffects list
            foreach (Transform child in transform)
            {
                var soundEffect = new SoundEffect(child.name, child.GetComponent<AudioSource>());
                if (soundEffect != null)
                {
                    soundEffects.Add(soundEffect);
                }
            }
        }

        /// <summary>
        /// Plays a sound effect by its name.
        /// </summary>
        /// <param name="soundName">The name of the sound effect to play.</param>
        /// <remarks>
        /// If the sound name is "Punch," it plays multiple punch-related sound effects.
        /// If the sound is not found, a warning is logged to the console.
        /// </remarks>
        public void PlaySound(string soundName)
        {
            // Special case: Play multiple punch sound effects
            if (soundName == "Punch")
            {
                soundEffects.Find(x => x.name == "Punch").audioSource.Play();
                soundEffects.Find(x => x.name == "Punch 2").audioSource.Play();
                soundEffects.Find(x => x.name == "Punch 3").audioSource.Play();
                return;
            }

            // Find and play the sound effect by name
            foreach (var soundEffect in soundEffects)
            {
                if (soundEffect.name == soundName)
                {
                    soundEffect.audioSource.Play();
                    return;
                }
            }

            // Log a warning if the sound effect is not found
            Debug.LogWarning($"Sound '{soundName}' not found!");
        }
    }

    /// <summary>
    /// Represents a sound effect, including its name and associated AudioSource.
    /// </summary>
    public class SoundEffect
    {
        /// <summary>
        /// The name of the sound effect.
        /// </summary>
        public string name;

        /// <summary>
        /// The AudioSource component that plays the sound effect.
        /// </summary>
        public AudioSource audioSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundEffect"/> class.
        /// </summary>
        /// <param name="name">The name of the sound effect.</param>
        /// <param name="audioSource">The AudioSource component for the sound effect.</param>
        public SoundEffect(string name, AudioSource audioSource)
        {
            this.name = name;
            this.audioSource = audioSource;
        }
    }
}

