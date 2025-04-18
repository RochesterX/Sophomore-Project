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
    /// Represents a playlist of music tracks.
    /// Contains information about the tracks, their associated scenes, and playback settings.
    /// </summary>
    [System.Serializable]
    public class Playlist
    {
        /// <summary>
        /// The name of the playlist.
        /// </summary>
        public string trackName;

        /// <summary>
        /// A list of scenes where this playlist is used.
        /// </summary>
        public List<string> trackScenes;

        /// <summary>
        /// A list of audio clips included in this playlist.
        /// </summary>
        public List<AudioClip> songs;

        /// <summary>
        /// The time interval (in seconds) between shuffling tracks in the playlist.
        /// </summary>
        public float shuffleTime;

        /// <summary>
        /// The volume level for the playlist.
        /// </summary>
        public float volume;
    }
}