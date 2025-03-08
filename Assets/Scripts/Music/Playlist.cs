using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Playlist
{
    public string trackName;
    public List<string> trackScenes;
    public List<AudioClip> songs;
    public float shuffleTime;
    public float volume;
}
