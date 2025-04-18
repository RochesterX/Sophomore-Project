using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Music;
using Player;
using TMPro;

namespace Game
{
    /// <summary>
    /// This class manages the health bar visuals for a player, including updating
    /// the health bar's size, position, and color based on the player's current health.
    /// </summary>
    public class TerribleHealthBarScript : MonoBehaviour
    {
        /// <summary>
        /// The color of the health bar when the player is at full health.
        /// </summary>
        public Color fullHealthColor;

        /// <summary>
        /// The color of the health bar when the player is at zero health.
        /// </summary>
        public Color fullDeathColor;

        /// <summary>
        /// The color used to represent health subtraction.
        /// </summary>
        public Color subtractionColor;

        /// <summary>
        /// The visual representation of the health bar.
        /// </summary>
        public GameObject healthVisual;

        /// <summary>
        /// The actual health bar that reflects the player's current health.
        /// </summary>
        public GameObject actualHealthVisual;

        /// <summary>
        /// The visual representation of the player's death state.
        /// </summary>
        public GameObject deathVisual;

        /// <summary>
        /// The speed at which the health bar updates smoothly.
        /// </summary>
        public float smoothSpeed = 0.1f;

        /// <summary>
        /// The text element displaying the player's current and maximum health.
        /// </summary>
        public TextMeshProUGUI text;

        /// <summary>
        /// The <see cref="Damageable"/> component of the player, used to track health.
        /// </summary>
        private Damageable healthScript;

        /// <summary>
        /// The initial scale of the health bar.
        /// </summary>
        private Vector3 initialScale;

        /// <summary>
        /// The initial position of the health bar.
        /// </summary>
        private Vector3 initialPosition;

        /// <summary>
        /// The target scale of the health bar based on the player's current health.
        /// </summary>
        private Vector3 targetScale;

        /// <summary>
        /// The target position of the health bar based on the player's current health.
        /// </summary>
        private Vector3 targetPosition;

        /// <summary>
        /// The target color of the actual health bar based on the player's current health.
        /// </summary>
        private Color targetActualColor;

        /// <summary>
        /// The player associated with this health bar.
        /// </summary>
        public GameObject player;

        /// <summary>
        /// Initializes the health bar for the specified player.
        /// </summary>
        private void Start()
        {
            InitializePlayer(player);
        }

        /// <summary>
        /// Updates the health bar visuals to reflect the player's current health.
        /// </summary>
        private void Update()
        {
            if (player == null || healthScript == null)
            {
                return;
            }

            // Calculate the health ratio and update the health bar visuals
            float healthRatio = (healthScript.maxDamage - healthScript.damage) / healthScript.maxDamage;
            targetActualColor = Color.Lerp(fullDeathColor, fullHealthColor, healthRatio);
            targetScale = new Vector3(Mathf.Lerp(0, 1, healthRatio) * initialScale.x, healthVisual.transform.localScale.y, healthVisual.transform.localScale.z);
            targetPosition = new Vector3(Mathf.Lerp(-0.5f, 0, healthRatio), healthVisual.transform.localPosition.y, healthVisual.transform.localPosition.z);
            text.text = (healthScript.maxDamage - healthScript.damage).ToString() + "/" + healthScript.maxDamage.ToString();

            // Smoothly update the health bar's scale, position, and color
            actualHealthVisual.transform.localScale = targetScale;
            actualHealthVisual.transform.localPosition = targetPosition;
            healthVisual.transform.localScale = Vector3.Lerp(healthVisual.transform.localScale, targetScale, smoothSpeed);
            healthVisual.transform.localPosition = Vector3.Lerp(healthVisual.transform.localPosition, targetPosition, smoothSpeed);
            actualHealthVisual.GetComponent<SpriteRenderer>().color = Color.Lerp(actualHealthVisual.GetComponent<SpriteRenderer>().color, targetActualColor, smoothSpeed);
            deathVisual.GetComponent<SpriteRenderer>().color = Color.Lerp(deathVisual.GetComponent<SpriteRenderer>().color, targetActualColor * 0.5f, smoothSpeed);
            healthVisual.GetComponent<SpriteRenderer>().color = subtractionColor;
        }

        /// <summary>
        /// Sets the player associated with this health bar.
        /// </summary>
        /// <param name="player">The player to associate with this health bar.</param>
        public void SetPlayer(GameObject player)
        {
            InitializePlayer(player);
        }

        /// <summary>
        /// Initializes the health bar for the specified player.
        /// </summary>
        /// <param name="player">The player to initialize the health bar for.</param>
        private void InitializePlayer(GameObject player)
        {
            this.player = player;
            if (this.player == null)
            {
                return;
            }

            // Get the Damageable component of the player
            healthScript = player.GetComponent<Damageable>();
            if (healthScript == null)
            {
                return;
            }

            Initialize();
        }

        /// <summary>
        /// Sets up the initial state of the health bar.
        /// </summary>
        private void Initialize()
        {
            initialScale = healthVisual.transform.localScale;
            initialPosition = healthVisual.transform.position;
            targetScale = initialScale;
            targetPosition = initialPosition;
            targetActualColor = actualHealthVisual.GetComponent<SpriteRenderer>().color;
        }
    }
}