using System.Collections;
using UnityEngine;

public class Nivel2Tutorial : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject canvasNivel2;

    [Header("Movimiento")]
    public MonoBehaviour movimientoJugador;

    [Header("Duración")]
    public float duracion = 6f;

    [Header("Fade")]
    public float velocidadFade = 2f;

    private bool activado = false;

    private CanvasGroup canvasGroup;

    void Start()
    {
        if (canvasNivel2 != null)
        {
            canvasGroup =
                canvasNivel2.GetComponent<CanvasGroup>();

            if (canvasGroup == null)
            {
                canvasGroup =
                    canvasNivel2.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = 0f;

            canvasNivel2.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (activado)
            return;

        activado = true;

        StartCoroutine(
            MostrarTutorial()
        );
    }

    IEnumerator MostrarTutorial()
    {
        canvasNivel2.SetActive(true);

        if (movimientoJugador != null)
            movimientoJugador.enabled = false;

        yield return StartCoroutine(
            FadeCanvas(0f, 1f)
        );

        yield return new WaitForSeconds(
            duracion
        );

        yield return StartCoroutine(
            FadeCanvas(1f, 0f)
        );

        canvasNivel2.SetActive(false);

        if (movimientoJugador != null)
            movimientoJugador.enabled = true;
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