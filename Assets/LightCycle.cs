using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightCycle : MonoBehaviour
{
    public List<Color> colors;

    public int targetColorIndex = 1;

    public float speed = 1.0f;

    private Light2D light2D;

    void Start()
    {
        light2D = GetComponent<Light2D>();
    }

    void Update()
    {
        if (light2D.color == colors[targetColorIndex])
        {
            targetColorIndex = (targetColorIndex + 1) % colors.Count;
        }

        light2D.color = Color.Lerp(light2D.color, colors[targetColorIndex], Time.deltaTime * speed);
    }
}
