using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShadowUpdater : MonoBehaviour
{
    public bool restrictSelfShadows = false;
    private void Start()
    {
        gameObject.AddComponent<ShadowCaster2D>();
        if (restrictSelfShadows)
        {
            ShadowCaster2D shadowCaster = GetComponent<ShadowCaster2D>();
            shadowCaster.selfShadows = false;
        }
    }
}
