using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PrimerEnemigoTutorial : MonoBehaviour
{
    [Header("Canvas Intro")]
    public GameObject canvasPrimerEnemigo;

    [Header("Canvas Efecto")]
    public GameObject canvasEfecto;

    [Header("Fade Negro")]
    public Image fondoNegro;

    [Header("Duraciones")]
    public float duracionIntro = 8f;
    public float duracionEfecto = 5f;

    [Header("Fade")]
    public float velocidadFade = 2f;

    private bool activado = false;

    private CanvasGroup introGroup;
    private CanvasGroup efectoGroup;
    private CanvasGroup fondoGroup;

    void Start()
    {
        PrepararCanvas(canvasPrimerEnemigo, out introGroup);
        PrepararCanvas(canvasEfecto, out efectoGroup);

        if (fondoNegro != null)
        {
            fondoGroup =
                fondoNegro.GetComponent<CanvasGroup>();

            if (fondoGroup == null)
                fondoGroup =
                    fondoNegro.gameObject.AddComponent<CanvasGroup>();

            fondoGroup.alpha = 0f;

            fondoNegro.gameObject.SetActive(false);
        }
    }

    void PrepararCanvas(
        GameObject canvas,
        out CanvasGroup group
    )
    {
        group = null;

        if (canvas == null) return;

        group = canvas.GetComponent<CanvasGroup>();

        if (group == null)
            group = canvas.AddComponent<CanvasGroup>();

        group.alpha = 0f;

        canvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (activado)
            return;

        activado = true;

        StartCoroutine(
            SecuenciaTutorial()
        );
    }

    IEnumerator SecuenciaTutorial()
    {
        fondoNegro.gameObject.SetActive(true);

        // =========================
        // PRIMER ENEMIGO
        // =========================

        canvasPrimerEnemigo.SetActive(true);

        yield return StartCoroutine(
            FadeCanvas(
                introGroup,
                0f,
                1f
            )
        );

        yield return StartCoroutine(
            FadeCanvas(
                fondoGroup,
                0f,
                0.8f
            )
        );

        yield return new WaitForSeconds(
            duracionIntro
        );

        yield return StartCoroutine(
            FadeCanvas(
                introGroup,
                1f,
                0f
            )
        );

        canvasPrimerEnemigo.SetActive(false);

        // =========================
        // EFECTO BLOQUEO CREATIVO
        // =========================

        canvasEfecto.SetActive(true);

        yield return StartCoroutine(
            FadeCanvas(
                efectoGroup,
                0f,
                1f
            )
        );

        yield return new WaitForSeconds(
            duracionEfecto
        );

        yield return StartCoroutine(
            FadeCanvas(
                efectoGroup,
                1f,
                0f
            )
        );

        yield return StartCoroutine(
            FadeCanvas(
                fondoGroup,
                0.8f,
                0f
            )
        );

        canvasEfecto.SetActive(false);

        fondoNegro.gameObject.SetActive(false);
    }

    IEnumerator FadeCanvas(
        CanvasGroup group,
        float inicio,
        float final
    )
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * velocidadFade;

            group.alpha =
                Mathf.Lerp(inicio, final, t);

            yield return null;
        }

        group.alpha = final;
    }
}