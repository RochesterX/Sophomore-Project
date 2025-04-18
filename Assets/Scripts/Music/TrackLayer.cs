using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Music;
using Player;
using UnityEngine.Audio;

namespace Music
{
    /// <summary>
    /// Represents a single layer of a music track.
    /// Each layer can be triggered and controlled independently based on game events or conditions.
    /// </summary>
    [System.Serializable]
    public class TrackLayer
    {
        /// <summary>
        /// The name of the music layer.
        /// </summary>
        public string layerName;

        /// <summary>
        /// The audio clip associated with this music layer.
        /// </summary>
        public AudioClip layerTrack;

        /// <summary>
        /// Defines the conditions under which this layer is enabled.
        /// </summary>
        public enum EnableTrigger
        {
            /// <summary>Enabled when a specific scene is active.</summary>
            Scene,
            /// <summary>Enabled when the player is magnetized.</summary>
            Magnetism,
            /// <summary>Enabled when a goal is activated.</summary>
            Goal,
            /// <summary>Enabled when a button is pressed.</summary>
            Button,
            /// <summary>Enabled when a toggle is active.</summary>
            Toggle,
            /// <summary>Enabled when the player is moving.</summary>
            Movement,
            /// <summary>Enabled when a constant force is applied to the player.</summary>
            ConstantForce,
            /// <summary>Enabled at the end of a level.</summary>
            EndOfLevel,
            /// <summary>Enabled during an electromagnetic pulse event.</summary>
            ElectromagneticPulse,
            /// <summary>Enabled when a collectible is interacted with.</summary>
            Collectible
        }

        /// <summary>
        /// The trigger condition for enabling this layer.
        /// </summary>
        public EnableTrigger enableTrigger = EnableTrigger.Scene;

        /// <summary>
        /// A list of scenes where this layer is active.
        /// If empty, the layer is active in all scenes.
        /// </summary>
        public List<string> layerScenes;

        /// <summary>
        /// The name of the object that triggers this layer.
        /// </summary>
        public string triggerName;
    }
}