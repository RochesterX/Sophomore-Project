#if NO

using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackManager : MonoBehaviour
{
    public Playlist musicTrack;
    public GameObject layerPrefab;

    private List<TrackLayer> persistentLayers = new List<TrackLayer>();

    private Scene currentScene;

    private void Awake()
    {
        if (!GameManager.music)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (var layer in musicTrack.trackLayers)
        {
            if (layer.enableTrigger != TrackLayer.EnableTrigger.Scene)
            {
                persistentLayers.Add(layer);
            }
        }

        currentScene = GetActiveSceneNotStatistics();
        InitializeLayers();
        UpdateLayers(musicTrack.trackLayers);
    }

    private void Update()
    {
        CheckForRestartability();

        if (currentScene != GetActiveSceneNotStatistics())
        {
            currentScene = GetActiveSceneNotStatistics();
            UpdateLayers(musicTrack.trackLayers);
        }

        if (persistentLayers.Count != 0) UpdateLayers(persistentLayers);
    }

    private void InitializeLayers()
    {
        foreach (TrackLayer layer in musicTrack.trackLayers)
        {
            AudioSource layerSource = Instantiate(layerPrefab, transform).GetComponent<AudioSource>();
            layerSource.gameObject.name = layer.layerName;
            layerSource.clip = layer.layerTrack;
            layerSource.volume = 0;

            try
            {
                layerSource.outputAudioMixerGroup = musicTrack.defaultMixer.FindMatchingGroups("Master/" + layer.layerName)[0];
            }
            catch
            {
                layerSource.outputAudioMixerGroup = musicTrack.defaultMixer.FindMatchingGroups("Master")[0];
            }

            layerSource.Play();
        }
    }

    private void UpdateLayers(List<TrackLayer> layers)
    {
        if (StatisticsManager.PlayerPrefs.GetInt("settingMusic") == 1)
        {
            foreach (TrackLayer layer in layers)
            {
                DisableLayer(layer);

                if (layer.enableTrigger == TrackLayer.EnableTrigger.Magnetism)
                {
                    GameObject player = GameObject.Find(layer.triggerName);
                    try
                    {
                        if (player != null && (player.GetComponent<PlayerMovement>().magnetized/* || FindFirstObjectByType<LevelEnd>().ending*/))
                        {
                            if (layer.layerScenes.Count == 0)
                            {
                                EnableLayer(layer);
                            }
                            else
                            {
                                foreach (string scene in layer.layerScenes)
                                {
                                    if (scene == currentScene.name)
                                    {
                                        EnableLayer(layer);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch (System.Exception e)
                    {
                        print(e.ToString());
                    }
                }
                else if (layer.enableTrigger == TrackLayer.EnableTrigger.Movement)
                {
                    GameObject player = GameObject.Find(layer.triggerName);
                    try
                    {
                        if (player != null && player.GetComponent<Rigidbody2D>().linearVelocity.magnitude >= 0.1f)
                        {
                            if (layer.layerScenes.Count == 0)
                            {
                                EnableLayer(layer);
                            }
                            else
                            {
                                foreach (string scene in layer.layerScenes)
                                {
                                    if (scene == currentScene.name)
                                    {
                                        EnableLayer(layer);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch (System.Exception e)
                    {
                        print(e.ToString());
                    }
                }
                else if (layer.enableTrigger == TrackLayer.EnableTrigger.ConstantForce)
                {
                    GameObject player = GameObject.Find(layer.triggerName);
                    try
                    {
                        if (player != null && player.GetComponent<ConstantForce2D>().force.magnitude >= 0.1f)
                        {
                            if (layer.layerScenes.Count == 0)
                            {
                                EnableLayer(layer);
                            }
                            else
                            {
                                foreach (string scene in layer.layerScenes)
                                {
                                    if (scene == currentScene.name)
                                    {
                                        EnableLayer(layer);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch (System.Exception e)
                    {
                        print(e.ToString());
                    }
                }
                else if (layer.enableTrigger == TrackLayer.EnableTrigger.Toggle)
                {
                    GameObject toggle = GameObject.Find(layer.triggerName);
                    try
                    {
                        if (toggle != null && toggle.GetComponent<ToggleBehavior>().state == ToggleBehavior.ToggleState.active)
                        {
                            if (layer.layerScenes.Count == 0)
                            {
                                EnableLayer(layer);
                            }
                            else
                            {
                                foreach (string scene in layer.layerScenes)
                                {
                                    if (scene == currentScene.name)
                                    {
                                        EnableLayer(layer);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch (System.Exception e)
                    {
                        print(e.ToString());
                    }

                }
                else if (layer.enableTrigger == TrackLayer.EnableTrigger.Button)
                {
                    GameObject button = GameObject.Find(layer.triggerName);
                    try
                    {
                        if (button != null && button.GetComponent<ButtonBehavior>().state == ButtonBehavior.ButtonState.pressed)
                        {
                            if (layer.layerScenes.Count == 0)
                            {
                                EnableLayer(layer);
                            }
                            else
                            {
                                foreach (string scene in layer.layerScenes)
                                {
                                    if (scene == currentScene.name)
                                    {
                                        EnableLayer(layer);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch (System.Exception e)
                    {
                        print(e.ToString());
                    }

                }
                else if (layer.enableTrigger == TrackLayer.EnableTrigger.Goal)
                {
                    GameObject goal = GameObject.Find(layer.triggerName);
                    try
                    {
                        if (goal != null && goal.GetComponent<Goal>().isActivated)
                        {
                            if (layer.layerScenes.Count == 0)
                            {
                                EnableLayer(layer);
                            }
                            else
                            {
                                foreach (string scene in layer.layerScenes)
                                {
                                    if (scene == currentScene.name)
                                    {
                                        EnableLayer(layer);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch (System.Exception e)
                    {
                        print(e.ToString());
                    }

                }
                else if (layer.enableTrigger == TrackLayer.EnableTrigger.EndOfLevel)
                {
                    if (FindFirstObjectByType<LevelEnd>().ending)
                    {
                        EnableLayer(layer);
                    }
                }
                else if (layer.enableTrigger == TrackLayer.EnableTrigger.ElectromagneticPulse)
                {
                    if (Camera.main.GetComponent<CameraFollow>().playTheTrack)
                    {
                        EnableLayer(layer);
                    }
                }
                else if (layer.enableTrigger == TrackLayer.EnableTrigger.Collectible)
                {
                    if (FindFirstObjectByType<Collectible>().playTheTrack)
                    {
                        EnableLayer(layer, "collectibleEnabled");
                    }
                }
                else
                {
                    foreach (string scene in layer.layerScenes)
                    {
                        if (scene == currentScene.name)
                        {
                            EnableLayer(layer);
                            break;
                        }
                    }
                }
            }
        }
    }

    private void CheckForRestartability()
    {
        bool restart = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            AudioSource child = transform.GetChild(i).GetComponent<AudioSource>();
            if (child != null)
            {
                if (!child.isPlaying)
                {
                    restart = true;
                    break;
                }
            }
        }

        if (!restart) return;

        for (int i = 0; i < transform.childCount; i++)
        {
            AudioSource child = transform.GetChild(i).GetComponent<AudioSource>();

            if (child != null)
            {
                child.Stop();
                child.Play();
            }
        }
    }

    private void EnableLayer(TrackLayer layer, string parameter = "enabled")
    {
        transform.Find(layer.layerName).GetComponent<Animator>().SetBool(parameter, true);
    }

    private void DisableLayer(TrackLayer layer)
    {
        foreach (AnimatorControllerParameter parameter in transform.Find(layer.layerName).GetComponent<Animator>().parameters)
        {
            transform.Find(layer.layerName).GetComponent<Animator>().SetBool(parameter.name, false);
        }
    }

    public static Scene GetActiveSceneNotStatistics()
    {
        for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
        {
            if (SceneManager.GetSceneAt(sceneIndex).name != "Statistics Manager Scene")
            {
                return SceneManager.GetSceneAt(sceneIndex);
            }
        }
        return SceneManager.GetSceneByBuildIndex(0);
    }

    public void Stop()
    {
        StartCoroutine(DestroyTrack());
    }

    public IEnumerator DestroyTrack()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
#endif
