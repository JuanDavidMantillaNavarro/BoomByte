using System.Collections;
using UnityEngine;

public class EnemyEffectPopup : MonoBehaviour
{
    [Header("Canvas Efecto")]
    public GameObject canvasEfecto;

    [Header("Duración")]
    public float duracion = 5f;

    [Header("Fade")]
    public float velocidadFade = 2f;

    private bool mostrando = false;

    private CanvasGroup canvasGroup;

    void Start()
    {
        if (canvasEfecto != null)
        {
            canvasGroup =
                canvasEfecto.GetComponent<CanvasGroup>();

            if (canvasGroup == null)
            {
                canvasGroup =
                    canvasEfecto.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = 0f;

            canvasEfecto.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (mostrando)
            return;

        StartCoroutine(
            MostrarPopup()
        );
    }

    IEnumerator MostrarPopup()
    {
        mostrando = true;

        canvasEfecto.SetActive(true);

        yield return StartCoroutine(
            FadeCanvas(0f, 1f)
        );

        yield return new WaitForSeconds(
            duracion
        );

        yield return StartCoroutine(
            FadeCanvas(1f, 0f)
        );

        canvasEfecto.SetActive(false);

        mostrando = false;
    }

    IEnumerator FadeCanvas(
        float inicio,
        float final
    )
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * velocidadFade;

            canvasGroup.alpha =
                Mathf.Lerp(inicio, final, t);

            yield return null;
        }

        canvasGroup.alpha = final;
    }
}