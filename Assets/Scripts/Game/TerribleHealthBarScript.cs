using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TerribleHealthBarScript : MonoBehaviour
{
    public Color fullHealthColor;
    public Color fullDeathColor;
    public Color subtractionColor;
    public GameObject healthVisual;
    public GameObject actualHealthVisual;
    public GameObject deathVisual;
    public float smoothSpeed = 0.1f;
    public TextMeshProUGUI text;
    private Damageable healthScript;

    private Vector3 initialScale;
    private Vector3 initialPosition;
    private Vector3 targetScale;
    private Vector3 targetPosition;
    private Color targetActualColor;

    public GameObject player;

    void Start()
    {
        InitializePlayer(player);
    }

    void Update() // Updates each player's health bar to display their current health
    {
        if (player == null || healthScript == null)
        {
            return;
        }
        float healthRatio = (healthScript.maxDamage - healthScript.damage) / healthScript.maxDamage;

        targetActualColor = Color.Lerp(fullDeathColor, fullHealthColor, healthRatio);
        targetScale = new Vector3(Mathf.Lerp(0, 1, healthRatio) * initialScale.x, healthVisual.transform.localScale.y, healthVisual.transform.localScale.z);
        targetPosition = new Vector3(Mathf.Lerp(-0.5f, 0, healthRatio), healthVisual.transform.localPosition.y, healthVisual.transform.localPosition.z);
        text.text = (healthScript.maxDamage - healthScript.damage).ToString() + "/" + healthScript.maxDamage.ToString();
        actualHealthVisual.transform.localScale = targetScale;
        actualHealthVisual.transform.localPosition = targetPosition;
        healthVisual.transform.localScale = Vector3.Lerp(healthVisual.transform.localScale, targetScale, smoothSpeed);
        healthVisual.transform.localPosition = Vector3.Lerp(healthVisual.transform.localPosition, targetPosition, smoothSpeed);
        actualHealthVisual.GetComponent<SpriteRenderer>().color = Color.Lerp(actualHealthVisual.GetComponent<SpriteRenderer>().color, targetActualColor, smoothSpeed);
        deathVisual.GetComponent<SpriteRenderer>().color = Color.Lerp(deathVisual.GetComponent<SpriteRenderer>().color, targetActualColor * 0.5f, smoothSpeed);
        healthVisual.GetComponent<SpriteRenderer>().color = subtractionColor;
    }

    public void SetPlayer(GameObject player)
    {
        InitializePlayer(player);
    }

    private void InitializePlayer(GameObject player) // Adds a health bar for each player
    {
        this.player = player;
        if (this.player == null)
        {
            return;
        }
        healthScript = player.GetComponent<Damageable>();
        if (healthScript == null)
        {
            return;
        }
        Initialize();
    }

    private void Initialize() // Sets up the health bars
    {
        initialScale = healthVisual.transform.localScale;
        initialPosition = healthVisual.transform.position;
        targetScale = initialScale;
        targetPosition = initialPosition;
        targetActualColor = actualHealthVisual.GetComponent<SpriteRenderer>().color;
    }
}
