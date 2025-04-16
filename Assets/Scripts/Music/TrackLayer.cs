using System.Collections;
using System.Collections.Generic;
using UnityEngine; using Game; using Music; using Player;
using UnityEngine.Audio;
namespace Music
{

[System.Serializable]
public class TrackLayer
{
    public string layerName;
    public AudioClip layerTrack;
    public enum EnableTrigger { Scene, Magnetism, Goal, Button, Toggle, Movement, ConstantForce, EndOfLevel, ElectromagneticPulse, Collectible };
    public EnableTrigger enableTrigger = EnableTrigger.Scene;

    public List<string> layerScenes;
    public string triggerName;
}
}