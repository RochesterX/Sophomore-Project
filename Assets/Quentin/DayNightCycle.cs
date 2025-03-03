using System.Collections;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public SpriteRenderer daySky, eveningSky, nightSky;
    public SpriteRenderer dayBackClouds, eveningBackClouds, nightBackClouds;
    public SpriteRenderer dayFrontClouds, eveningFrontClouds, nightFrontClouds;

    public float transitionDuration = 5f; // Duration of transition
    public float cycleDuration = 60f; // Time before transitioning to next phase

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

    private void SetAlpha(SpriteRenderer sprite, float alpha)
    {
        if (sprite)
        {
            Color color = sprite.color;
            color.a = alpha;
            sprite.color = color;
        }
    }
}
