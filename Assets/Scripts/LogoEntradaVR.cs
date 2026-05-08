using System.Collections;
using UnityEngine;

public class LogoEntradaVR : MonoBehaviour
{
    [Header("Referencias")]
    public Transform logo;

    public CanvasGroup canvasGroup;

    [Header("Movimiento")]
    public float velocidad = 5f;

    public float crecimiento = 1.5f;

    public float distanciaDesaparecer = 0.5f;

    [Header("Fade")]
    public float duracionFade = 0.4f;

    private bool activado = false;

    private Transform camaraVR;

    private Vector3 direccion;

    void Start()
    {
        camaraVR = Camera.main.transform;

        canvasGroup.alpha = 0f;

        logo.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (activado) return;

        if (other.CompareTag("Player"))
        {
            activado = true;

            direccion =
                (camaraVR.position - logo.position).normalized;

            StartCoroutine(AnimacionLogo());
        }
    }

    IEnumerator AnimacionLogo()
    {
        // BLOQUEAR TODO
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;

        logo.gameObject.SetActive(true);

        // FADE IN
        yield return StartCoroutine(Fade(0f, 1f));

        while (true)
        {
            // Movimiento usando unscaledDeltaTime
            logo.position +=
                direccion *
                velocidad *
                Time.unscaledDeltaTime;

            // Escala
            logo.localScale +=
                Vector3.one *
                crecimiento *
                Time.unscaledDeltaTime;

            float distancia =
                Vector3.Distance(
                    logo.position,
                    camaraVR.position
                );

            // Cuando atraviesa jugador
            if (distancia <= distanciaDesaparecer)
            {
                break;
            }

            yield return null;
        }

        // FADE OUT
        yield return StartCoroutine(Fade(1f, 0f));

        logo.gameObject.SetActive(false);

        // RESTAURAR
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
    }

    IEnumerator Fade(float inicio, float fin)
    {
        float tiempo = 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.unscaledDeltaTime;

            canvasGroup.alpha =
                Mathf.Lerp(
                    inicio,
                    fin,
                    tiempo / duracionFade
                );

            yield return null;
        }

        canvasGroup.alpha = fin;
    }
}