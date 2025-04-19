using System.Collections;
using UnityEngine;
using Game;
using Music;
using Player;

namespace Game
{
    /// <summary>
    /// Manages the day-night cycle by transitioning between different sky and cloud sprites.
    /// </summary>
    public class DayNightCycle : MonoBehaviour
    {
        /// <summary>
        /// Sprite representing the sky during the day.
        /// </summary>
        public SpriteRenderer daySky;

        /// <summary>
        /// Sprite representing the sky during the evening.
        /// </summary>
        public SpriteRenderer eveningSky;

        /// <summary>
        /// Sprite representing the sky during the night.
        /// </summary>
        public SpriteRenderer nightSky;

        /// <summary>
        /// Sprite representing the back clouds during the day.
        /// </summary>
        public SpriteRenderer dayBackClouds;

        /// <summary>
        /// Sprite representing the back clouds during the evening.
        /// </summary>
        public SpriteRenderer eveningBackClouds;

        /// <summary>
        /// Sprite representing the back clouds during the night.
        /// </summary>
        public SpriteRenderer nightBackClouds;

        /// <summary>
        /// Sprite representing the front clouds during the day.
        /// </summary>
        public SpriteRenderer dayFrontClouds;

        /// <summary>
        /// Sprite representing the front clouds during the evening.
        /// </summary>
        public SpriteRenderer eveningFrontClouds;

        /// <summary>
        /// Sprite representing the front clouds during the night.
        /// </summary>
        public SpriteRenderer nightFrontClouds;

        /// <summary>
        /// Duration of the transition between different phases of the day-night cycle.
        /// </summary>
        public float transitionDuration = 5f;

        /// <summary>
        /// Duration of each phase (day, evening, night) before transitioning to the next phase.
        /// </summary>
        public float cycleDuration = 60f;

        /// <summary>
        /// Initializes the day-night cycle by setting the initial alpha values and starting the cycle routine.
        /// </summary>
        private void Start()
        {
            // Set initial alpha (only day visible)
            SetAlpha(daySky, 1);
            SetAlpha(eveningSky, 0);
            SetAlpha(nightSky, 0);

            SetAlpha(dayBackClouds, 1);
            SetAlpha(eveningBackClouds, 0);
            SetAlpha(nightBackClouds, 0);

            SetAlpha(dayFrontClouds, 1);
            SetAlpha(eveningFrontClouds, 0);
            SetAlpha(nightFrontClouds, 0);

            // Start the cycle
            StartCoroutine(DayNightCycleRoutine());
        }

        /// <summary>
        /// Coroutine that manages the day-night cycle by transitioning between phases in a loop.
        /// </summary>
        private IEnumerator DayNightCycleRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(cycleDuration);
                yield return StartCoroutine(FadeTransition(
                    new SpriteRenderer[] { daySky, dayBackClouds, dayFrontClouds },
                    new SpriteRenderer[] { eveningSky, eveningBackClouds, eveningFrontClouds },
                    transitionDuration));

                yield return new WaitForSeconds(cycleDuration);
                yield return StartCoroutine(FadeTransition(
                    new SpriteRenderer[] { eveningSky, eveningBackClouds, eveningFrontClouds },
                    new SpriteRenderer[] { nightSky, nightBackClouds, nightFrontClouds },
                    transitionDuration));

                yield return new WaitForSeconds(cycleDuration);
                yield return StartCoroutine(FadeTransition(
                    new SpriteRenderer[] { nightSky, nightBackClouds, nightFrontClouds },
                    new SpriteRenderer[] { daySky, dayBackClouds, dayFrontClouds },
                    transitionDuration));
            }
        }

        /// <summary>
        /// Coroutine that handles the fade transition between two sets of sprites over a specified duration.
        /// </summary>
        /// <param name="from">Array of sprites to fade out.</param>
        /// <param name="to">Array of sprites to fade in.</param>
        /// <param name="duration">Duration of the fade transition.</param>
        private IEnumerator FadeTransition(SpriteRenderer[] from, SpriteRenderer[] to, float duration)
        {
            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = elapsedTime / duration;

                // Apply fading to all elements simultaneously
                for (int i = 0; i < from.Length; i++)
                {
                    SetAlpha(from[i], 1 - alpha);
                    SetAlpha(to[i], alpha);
                }

                yield return null;
            }

            // Ensure final alpha values are set correctly
            for (int i = 0; i < from.Length; i++)
            {
                SetAlpha(from[i], 0);
                SetAlpha(to[i], 1);
            }
        }

        /// <summary>
        /// Sets the alpha value of a sprite and its child sprites (if any).
        /// </summary>
        /// <param name="sprite">The sprite renderer to modify.</param>
        /// <param name="alpha">The alpha value to set (0 to 1).</param>
        private void SetAlpha(SpriteRenderer sprite, float alpha)
        {
            if (sprite)
            {
                Color color = sprite.color;
                color.a = alpha;
                sprite.color = color;
                foreach (Transform child in sprite.transform)
                {
                    if (child.TryGetComponent(out SpriteRenderer childSprite))
                    {
                        childSprite.color = color;
                    }
                }
            }
        }
    }
}