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
        if (!gameObject.activeInHierarchy) return;

        if (other.CompareTag("Player"))
        {
            FadeUI(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!gameObject.activeInHierarchy) return;

        if (other.CompareTag("Player"))
        {
            FadeUI(false);
        }
    }

    void FadeUI(bool show)
    {
        // 🔥 FIX UTAMA: jangan start coroutine kalau object inactive
        if (!gameObject.activeInHierarchy)
            return;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(Fade(show));
    }

    IEnumerator Fade(bool show)
    {
        if (interactUI == null)
            yield break;

        float start = interactUI.alpha;
        float end = show ? 1f : 0f;

        interactUI.interactable = show;
        interactUI.blocksRaycasts = show;

        float t = 0f;
        while (t < fadeDuration)
        {
            // 🔥 SAFETY: kalau object mati di tengah fade
            if (!gameObject.activeInHierarchy)
                yield break;

            t += Time.deltaTime;
            interactUI.alpha = Mathf.Lerp(start, end, t / fadeDuration);
            yield return null;
        }

        interactUI.alpha = end;
    }

    void SetUIInstant(bool show)
    {
        if (interactUI == null) return;

        interactUI.alpha = show ? 1f : 0f;
        interactUI.interactable = show;
        interactUI.blocksRaycasts = show;
    }
}
