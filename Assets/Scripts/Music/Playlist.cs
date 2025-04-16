using System.Collections;
using System.Collections.Generic;
using UnityEngine; using Game; using Music; using Player;
using UnityEngine.Audio;
namespace Music
{

[System.Serializable]
public class Playlist
{
    public string trackName;
    public List<string> trackScenes;
    public List<AudioClip> songs;
    public float shuffleTime;
    public float volume;
}
}