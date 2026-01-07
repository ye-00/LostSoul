using UnityEngine;
using System.Collections;

public class WorldInteractTrigger : MonoBehaviour
{
    public CanvasGroup interactUI;
    public float fadeDuration = 0.2f;

    Coroutine fadeRoutine;

    void Start()
    {
        SetUIInstant(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FadeUI(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FadeUI(false);
        }
    }

    void FadeUI(bool show)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(Fade(show));
    }

    IEnumerator Fade(bool show)
    {
        float start = interactUI.alpha;
        float end = show ? 1f : 0f;

        interactUI.interactable = show;
        interactUI.blocksRaycasts = show;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            interactUI.alpha = Mathf.Lerp(start, end, t / fadeDuration);
            yield return null;
        }

        interactUI.alpha = end;
    }

    void SetUIInstant(bool show)
    {
        interactUI.alpha = show ? 1f : 0f;
        interactUI.interactable = show;
        interactUI.blocksRaycasts = show;
    }
}
